# local_settings.py (scsef)
# !/usr/bin/env python3
# coding=utf-8
# young.daniel@epa.gov

"""
Local Django settings for GEMM project.

Available functions:
NOTE: All development uses 'localhost' not private or EPA servers
- Email settings
- PostgreSQL database settings
"""

SITE_NAME = 'localhost'

# Settings when installing scsef on the EPA RedHat email Server.
DEFAULT_FROM_EMAIL = 'dyoung11@engineering4sustainability.com'
EMAIL_HOST = '127.0.0.1'
EMAIL_HOST_USER = 'dyoung11'
EMAIL_HOST_PASSWORD = 'Evelynj1!'
EMAIL_PORT = 25
EMAIL_FILE_PATH = '/var/www/html/scsef/uploads'

EMAIL_SUPPORT = 'barrett.williamm@epa.gov'

USER_CONFIRMATION_EMAILS = [
    'barrett.williamm@epa.gov',
]

# SECURITY WARNING: keep the secret key used in production secret!
SECRET_KEY = '730582e9-a785-42ee-91b3-ca414e270615'

# SECURITY WARNING: do not run with debug turned on in production!
DEBUG = True

if DEBUG is True:
    ALLOWED_HOSTS = ['127.0.0.1', 'localhost', 'testserver']
else:
    ALLOWED_HOSTS = ['127.0.0.1', 'localhost', '.engineering4sustainability.com', 'testserver']

# This sets URL to run as localhost for development on VS 2017
BASE_URL = 'http://127.0.0.1'
DJANGO_SETTINGS_MODULE = 'scsef.settings'

ROOT_URLCONF = 'scsef.urls'

# From GEMM local_settings, trying to get Admin to work properly.
STATIC_ROOT = 'scsef/static/'
STATIC_URL = '/static/'

