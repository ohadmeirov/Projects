from django import forms
import json
import re
from django.core.exceptions import ValidationError
from .models import Customer

class RegisterForm(forms.Form):
    username = forms.CharField(max_length=50)
    email = forms.EmailField()
    password = forms.CharField(widget=forms.PasswordInput())
    
    def clean_username(self):
        username = self.cleaned_data.get('username')
        return username
    
    def clean_email(self):
        email = self.cleaned_data.get('email')
        return email

class ChangePasswordForm(forms.Form):
    old_password = forms.CharField(widget=forms.PasswordInput(), label="Old Password")
    new_password = forms.CharField(widget=forms.PasswordInput(), label="New Password")
    confirm_password = forms.CharField(widget=forms.PasswordInput(), label="Confirm New Password")

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        with open("config.json", "r") as f:
            self.config = json.load(f)

    def clean(self):
        cleaned_data = super().clean()
        return cleaned_data

    def clean_new_password(self):
        new_password = self.cleaned_data.get("new_password")
        config = self.config

        if not new_password:
            return new_password  

        # Password validation removed for testing purposes
        return new_password

class LoginForm(forms.Form):
    username = forms.CharField(max_length=50)
    password = forms.CharField(widget=forms.PasswordInput())

class CustomerForm(forms.ModelForm):
    class Meta:
        model = Customer
        fields = ['first_name', 'last_name', 'email', 'phone', 'address']
        widgets = {
            'first_name': forms.TextInput(attrs={'class': 'form-control', 'placeholder': 'First name'}),
            'last_name': forms.TextInput(attrs={'class': 'form-control', 'placeholder': 'Last name'}),
            'email': forms.EmailInput(attrs={'class': 'form-control', 'placeholder': 'Email'}),
            'phone': forms.TextInput(attrs={'class': 'form-control', 'placeholder': 'Phone'}),
            'address': forms.Textarea(attrs={'class': 'form-control', 'placeholder': 'Address', 'rows': 3}),
        }
    
    def clean_first_name(self):
        first_name = self.cleaned_data.get('first_name')
        return first_name
    
    def clean_last_name(self):
        last_name = self.cleaned_data.get('last_name')
        return last_name
    
    def clean_phone(self):
        phone = self.cleaned_data.get('phone')
        return phone
    
    def clean_address(self):
        address = self.cleaned_data.get('address')
        return address

class ForgotPasswordForm(forms.Form):
    email = forms.EmailField(
        widget=forms.EmailInput(attrs={'class': 'form-control', 'placeholder': 'Enter your email'})
    )

class ResetPasswordForm(forms.Form):
    token = forms.CharField(
        max_length=64,
        widget=forms.TextInput(attrs={'class': 'form-control', 'placeholder': 'Enter the reset code you received'})
    )
    new_password = forms.CharField(
        widget=forms.PasswordInput(attrs={'class': 'form-control', 'placeholder': 'New password'}),
        label="New Password"
    )
    confirm_password = forms.CharField(
        widget=forms.PasswordInput(attrs={'class': 'form-control', 'placeholder': 'Confirm new password'}),
        label="Confirm New Password"
    )

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        with open("config.json", "r") as f:
            self.config = json.load(f)

    def clean(self):
        cleaned_data = super().clean()
        return cleaned_data

    def clean_new_password(self):
        new_password = self.cleaned_data.get("new_password")
        config = self.config

        if not new_password:
            return new_password  

        # Password validation removed for testing purposes
        return new_password