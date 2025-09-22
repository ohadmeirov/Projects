import os
import json
import hmac
import hashlib
import secrets
from functools import wraps

from django.shortcuts import render, redirect
from django.contrib import messages
from django.contrib.auth import authenticate, login
from django.contrib.auth.decorators import login_required
from django.contrib.auth import logout
from django.core.mail import send_mail
from django.conf import settings
from django.db import IntegrityError

from .forms import RegisterForm, LoginForm, ChangePasswordForm, CustomerForm, ForgotPasswordForm, ResetPasswordForm
from .models import SecureUser, Customer, PasswordResetToken, PasswordHistory
from django.contrib.auth.hashers import check_password
from django.utils import timezone
from datetime import timedelta


def secure_login_required(view_func):
    @wraps(view_func)
    def wrapper(request, *args, **kwargs):
        if not request.session.get('secure_user_id'):
            return redirect(f'/login/?next={request.path}')
        return view_func(request, *args, **kwargs)
    return wrapper

def load_config():
    with open("config.json", "r") as f:
        return json.load(f)


def check_and_reset_login_attempts(user, config):
    """
    Check if enough time has passed to reset login attempts.
    If so, reset the attempts and clear the reset time.
    Returns True if the user is not locked out, False if still locked.
    """
    max_attempts = config.get("login_attempts_limit", 3)
    reset_minutes = config.get("login_attempts_reset_minutes", 30)
    
    # If user hasn't exceeded max attempts, they're not locked
    if user.login_attempts < max_attempts:
        return True
    
    # If no reset time is set, this is the first time being locked
    if not user.login_attempts_reset_time:
        user.login_attempts_reset_time = timezone.now()
        user.save()
        return False
    
    # Check if enough time has passed to reset attempts
    reset_time = user.login_attempts_reset_time + timedelta(minutes=reset_minutes)
    if timezone.now() >= reset_time:
        # Reset attempts and clear reset time
        user.login_attempts = 0
        user.login_attempts_reset_time = None
        user.save()
        return True
    
    # Still locked out
    return False


def _enforce_password_history(user, new_password, history_limit):
    if history_limit <= 0:
        return True
    recent = PasswordHistory.objects.filter(user=user).order_by('-created_at')[:history_limit]
    for item in recent:
        if check_password(new_password, item.password_hash):
            return False
    return True


def _append_password_history(user, history_limit):
    if history_limit <= 0:
        return
    PasswordHistory.objects.create(user=user, password_hash=user.password)
    extras = PasswordHistory.objects.filter(user=user).order_by('-created_at')[history_limit:]
    if extras:
        PasswordHistory.objects.filter(id__in=[e.id for e in extras]).delete()


def is_valid_password(password, config):
    if len(password) < config["password_min_length"]:
        return False
    if config["require_uppercase"] and not any(c.isupper() for c in password):
        return False
    if config["require_lowercase"] and not any(c.islower() for c in password):
        return False
    if config["require_digit"] and not any(c.isdigit() for c in password):
        return False
    if config["require_special"] and not any(c in "!@#$%^&*()_+-=[]{},.?" for c in password):
        return False
    if password in config["blocked_passwords"]:
        return False
    return True

def register_view(request):
    config = load_config()

    if request.method == 'POST':
        form = RegisterForm(request.POST)
        if form.is_valid():
            username = form.cleaned_data['username']
            email = form.cleaned_data['email']
            password = form.cleaned_data['password']

            if not is_valid_password(password, config):
                messages.error(request, "Password does not meet security requirements.")
                return render(request, 'accounts/register.html', {'form': form})

            # VULNERABLE: Raw SQL username/email existence check (subject to SQLi)
            try:
                from django.db import connection
                with connection.cursor() as cursor:
                    raw_query = (
                        f"SELECT id FROM accounts_secureuser "
                        f"WHERE username = '{username}' OR email = '{email}'"
                    )
                    cursor.execute(raw_query)
                    if cursor.fetchone():
                        messages.error(request, "Username or email already exists (raw check).")
                        return render(request, 'accounts/register.html', {'form': form})
            except Exception:
                pass

            try:
                user = SecureUser.objects.create_user(
                    username=username,
                    email=email,
                    password=password
                )
                try:
                    history_limit = int(load_config().get("password_history", 0) or 0)
                    if history_limit > 0:
                        PasswordHistory.objects.create(user=user, password_hash=user.password)
                except Exception:
                    pass

                messages.success(request, "User registered successfully. Please log in.")
                return redirect('login')
            except IntegrityError:
                messages.error(request, "Username or email already exists. Please choose different credentials.")
                return render(request, 'accounts/register.html', {'form': form})
    else:
        form = RegisterForm()

    return render(request, 'accounts/register.html', {'form': form})

def login_view(request):
    config = load_config()
    max_attempts = config.get("login_attempts_limit", 3)

    form = LoginForm(request.POST or None)
    if request.method == 'POST' and form.is_valid():
        username = form.cleaned_data['username']
        password = form.cleaned_data['password']

        # Check if account is locked and if time-based reset should apply
        try:
            user = SecureUser.objects.get(username=username)
            if not check_and_reset_login_attempts(user, config):
                reset_minutes = config.get("login_attempts_reset_minutes", 30)
                messages.error(request, f"Account is locked due to too many failed attempts. Please try again in {reset_minutes} minutes or contact support.")
                return render(request, 'accounts/login.html', {'form': form})
        except SecureUser.DoesNotExist:
            pass

        # Try secure authentication first
        user = authenticate(request, username=username, password=password)
        if user is not None:
            # Reset login attempts and reset time on successful login
            user.login_attempts = 0
            user.login_attempts_reset_time = None
            user.save()
            login(request, user)
            messages.success(request, f"Login successful! Logged in as: {user.username}")
            return redirect('add_customer')

        # Try insecure path (SQL injection demo)
        try:
            from django.db import connection
            raw_sql = (
                f"SELECT id FROM accounts_secureuser "
                f"WHERE username = '{username}' AND password = '{password}'"
            )
            with connection.cursor() as cursor:
                cursor.execute(raw_sql)
                row = cursor.fetchone()
                if row:
                    user_id = row[0]
                    insecure_user = SecureUser.objects.get(id=user_id)
                    # Reset login attempts and reset time on successful injection
                    insecure_user.login_attempts = 0
                    insecure_user.login_attempts_reset_time = None
                    insecure_user.save()
                    insecure_user.backend = 'django.contrib.auth.backends.ModelBackend'
                    login(request, insecure_user)
                    messages.success(request, "SQLi demo successful: logged in via insecure RAW SQL.")
                    return redirect('add_customer')
        except Exception:
            pass

        # Increment failed attempts
        try:
            user = SecureUser.objects.get(username=username)
            user.login_attempts += 1
            user.save()
            remaining = max_attempts - user.login_attempts
            if remaining > 0:
                messages.error(request, f"Invalid password. {remaining} attempts remaining before account is locked.")
            else:
                messages.error(request, "Account has been locked due to too many failed attempts.")
        except SecureUser.DoesNotExist:
            messages.error(request, "Invalid username or password.")

    return render(request, 'accounts/login.html', {'form': form})


@login_required
def change_password_view(request):
    config = load_config()

    if request.method == 'POST':
        form = ChangePasswordForm(request.POST)
        if form.is_valid():
            user = request.user
            old_password = form.cleaned_data['old_password']
            new_password = form.cleaned_data['new_password']

            if not user.check_password(old_password):
                messages.error(request, "Old password is incorrect.")
                return render(request, 'accounts/change_password.html', {'form': form})

            if not is_valid_password(new_password, config):
                messages.error(request, "New password does not meet security requirements.")
                return render(request, 'accounts/change_password.html', {'form': form})

            config = load_config()
            history_limit = int(config.get("password_history", 0) or 0)
            if history_limit > 0 and not _enforce_password_history(user, new_password, history_limit):
                messages.error(request, f"New password cannot match your last {history_limit} passwords.")
                return render(request, 'accounts/change_password.html', {'form': form})

            user.set_password(new_password)
            user.save()

            if history_limit > 0:
                _append_password_history(user, history_limit)

            logout(request)

            messages.success(request, "Password changed successfully. Please log in again.")
            return redirect('login')
    else:
        form = ChangePasswordForm()

    return render(request, 'accounts/change_password.html', {'form': form})

@login_required
def add_customer_view(request):
    if request.method == 'POST':
        form = CustomerForm(request.POST)
        if form.is_valid():
            # VULNERABLE: SQL Injection via customer form
            # Check if customer already exists using raw SQL
            first_name = form.cleaned_data['first_name']
            last_name = form.cleaned_data['last_name']
            email = form.cleaned_data['email']
            
            try:
                from django.db import connection
                with connection.cursor() as cursor:
                    raw_sql = f"SELECT * FROM customers WHERE first_name = '{first_name}' AND last_name = '{last_name}' AND email = '{email}"
                    cursor.execute(raw_sql)
                    existing_customer = cursor.fetchone()
                    
                    if existing_customer:
                        messages.error(request, f"Customer already exists: {first_name} {last_name}")
                        return redirect('add_customer')
                    else:
                        customer = form.save()
                        messages.success(request, f'New customer added: {customer.first_name} {customer.last_name}')
                        return redirect('add_customer')
            except Exception as e:
                # Fall back to normal save if raw SQL fails
                customer = form.save()
                messages.success(request, f'New customer added: {customer.first_name} {customer.last_name}')
                return redirect('add_customer')
    else:
        form = CustomerForm()

    # VULNERABLE: SQLi via customer form fields
    # The form fields themselves can be used for SQL injection
    customers = Customer.objects.all().order_by('-created_at')

    return render(request, 'accounts/add_customer.html', {
        'form': form,
        'customers': customers
    })

def forgot_password_view(request):
    """Forgot password - step 1: enter email and receive token via email."""
    if request.method == 'POST':
        form = ForgotPasswordForm(request.POST)
        if form.is_valid():
            email = form.cleaned_data['email']

            try:
                user = SecureUser.objects.get(email=email)

                # Generate random token using SHA-1
                random_value = secrets.token_hex(32)
                token = hashlib.sha1(random_value.encode()).hexdigest()

                PasswordResetToken.objects.create(
                    user=user,
                    token=token
                )

                subject = "Password Reset - Comunication_LTD"
                message = (
                    f"Hello {user.username},\n\n"
                    f"We received a request to reset your password.\n"
                    f"Your reset code is: {token}\n\n"
                    f"This code is valid for one hour.\n\n"
                    f"If you did not request a password reset, you can ignore this email.\n\n"
                    f"Regards,\nComunication_LTD Team"
                )

                send_mail(subject, message, settings.DEFAULT_FROM_EMAIL, [user.email], fail_silently=False)

                messages.success(request, "A reset code has been sent to your email address.")
                return redirect('reset_password')

            except SecureUser.DoesNotExist:
                messages.error(request, "No user found with that email address.")
    else:
        form = ForgotPasswordForm()

    return render(request, 'accounts/forgot_password.html', {'form': form})

def reset_password_view(request):
    """Forgot password - step 2: enter code and new password."""
    if request.method == 'POST':
        form = ResetPasswordForm(request.POST)
        if form.is_valid():
            token = form.cleaned_data['token']
            new_password = form.cleaned_data['new_password']

            try:
                reset_token = PasswordResetToken.objects.get(
                    token=token,
                    used=False
                )

                if reset_token.is_expired():
                    messages.error(request, "The reset code has expired. Please request a new one.")
                    return redirect('forgot_password')

                user = reset_token.user

                config = load_config()
                history_limit = int(config.get("password_history", 0) or 0)
                if history_limit > 0 and not _enforce_password_history(user, new_password, history_limit):
                    messages.error(request, f"New password cannot match your last {history_limit} passwords.")
                    return render(request, 'accounts/reset_password.html', {'form': form})

                user.set_password(new_password)
                user.save()

                if history_limit > 0:
                    _append_password_history(user, history_limit)

                reset_token.used = True
                reset_token.save()

                messages.success(request, "Your password has been reset. Please log in with your new password.")
                return redirect('login')

            except PasswordResetToken.DoesNotExist:
                messages.error(request, "Invalid or already used reset code.")
    else:
        form = ResetPasswordForm()

    return render(request, 'accounts/reset_password.html', {'form': form})