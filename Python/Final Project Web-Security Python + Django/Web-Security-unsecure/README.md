## Web-Security-unsecure (Django) — Setup, Usage, and SQLi Demo

### Prerequisites

- Python 3.10+ (tested with 3.11)
- Windows PowerShell or any shell (instructions include Windows-specific commands)

Windows (PowerShell):

```powershell
cd "C:\Users\<YOUR-USERNAME>\Desktop\Web-Security-unsecure"
```

macOS/Linux:

```bash
cd ~/Desktop/Web-Security-unsecure
```

### Create and activate a virtual environment

Windows (PowerShell):

```powershell
python -m venv venv
& ./venv/Scripts/Activate.ps1
```

macOS/Linux:

```bash
python3 -m venv venv
source venv/bin/activate
```

### Install dependencies

This project uses Django 4.2.x and standard Django dependencies. If you have a `requirements.txt`, install it directly:

Windows (PowerShell):

```powershell
pip install -r requirements.txt
```

macOS/Linux:

```bash
pip3 install -r requirements.txt
```

### Features

This project includes:

- **Time-based Login Attempt Reset**: Accounts automatically unlock after 30 minutes (configurable in `config.json`)
- **Password Change**: Now available from the main system screen (Add Customer page)
- **Password History**: Prevents reusing the last 3 passwords
- **Strong Password Policy**: Configurable via `config.json`
- **Account Lockout**: Protection against brute force attacks (3 failed attempts)

### Password policy

The `config.json` controls min length, required character classes, blocked passwords, password history, and login attempt reset timing.

### Apply migrations

Windows (PowerShell):

```powershell
python manage.py migrate
```

macOS/Linux:

```bash
python3 manage.py migrate
```

### Run the development server

Windows (PowerShell):

```powershell
python manage.py runserver
```

macOS/Linux:

```bash
python3 manage.py runserver
```

Open the app at `http://127.0.0.1:8000/`
