# manage.py (scsef)
# !/usr/bin/env python3
# coding=utf-8

"""Command-line utility for administrative tasks."""

import os
import sys

if __name__ == "__main__":
    os.environ.setdefault(
        "DJANGO_SETTINGS_MODULE",
        "SustainableChemistryData.settings"
    )

    from django.core.management import execute_from_command_line

    execute_from_command_line(sys.argv)
