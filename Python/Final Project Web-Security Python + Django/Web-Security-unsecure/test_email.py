import os
import django

# Setup Django
os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'Comunication_LTD.settings')
django.setup()

from django.core.mail import send_mail
from django.conf import settings

try:
    print("Testing email sending...")
    print(f"EMAIL_HOST: {settings.EMAIL_HOST}")
    print(f"EMAIL_PORT: {settings.EMAIL_PORT}")
    print(f"EMAIL_HOST_USER: {settings.EMAIL_HOST_USER}")
    print(f"EMAIL_HOST_PASSWORD: {settings.EMAIL_HOST_PASSWORD}")
    
    send_mail(
        subject='Test Email from Comunication_LTD',
        message='This is a test email to check if email sending works.',
        from_email=settings.DEFAULT_FROM_EMAIL,
        recipient_list=['test@example.com'],
        fail_silently=False
    )
    print("Email sent successfully!")
    
except Exception as e:
    print(f"Error sending email: {e}")
    print(f"Error type: {type(e).__name__}")
