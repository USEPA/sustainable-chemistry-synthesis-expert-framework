# fields.py (app)
# !/usr/bin/env python3
# coding=utf-8
# barrett.williamm@epa.gov


"""Add module docstring."""

# Case insensitive character fields from http://concisecoder.io/2018/10/27/case-insensitive-fields-in-django-models/
from django.db import models
from django.db import migrations


class CaseInsensitiveFieldMixin:
    """
    Field mixin that uses case-insensitive lookup alternatives if they exist.
    """
    LOOKUP_CONVERSIONS = {
        'exact': 'iexact',
        'contains': 'icontains',
        'startswith': 'istartswith',
        'endswith': 'iendswith',
        'regex': 'iregex',
    }

    def get_lookup(self, lookup_name):
        """Add method docstring"""
        converted = self.LOOKUP_CONVERSIONS.get(lookup_name, lookup_name)
        return super().get_lookup(converted)


class CICharField(CaseInsensitiveFieldMixin, models.CharField):
    """Add class docstring"""


class Migration(migrations.Migration):
    """Add class docstring"""

    dependencies = [
        ('app', '0001_initial'),
    ]
    operations = [
        migrations.RunSQL(
            sql=r'CREATE UNIQUE INDEX name_upper_idx ON app_namedreaction(UPPER(Name));',
            reverse_sql=r'DROP INDEX name_upper_idx;'
        ),
        migrations.RunSQL(
            sql=r'CREATE UNIQUE INDEX name_upper_idx ON app_functionalgroup(UPPER(Name));',
            reverse_sql=r'DROP INDEX name_upper_idx;'
        ),
        migrations.RunSQL(
            sql=r'CREATE UNIQUE INDEX name_upper_idx ON app_catalyst(UPPER(Name));',
            reverse_sql=r'DROP INDEX name_upper_idx;'
        ),
        migrations.RunSQL(
            sql=r'CREATE UNIQUE INDEX name_upper_idx ON app_compound(UPPER(Name));',
            reverse_sql=r'DROP INDEX name_upper_idx;'
        ),
        migrations.RunSQL(
            sql=r'CREATE UNIQUE INDEX name_upper_idx ON app_reactant(UPPER(Name));',
            reverse_sql=r'DROP INDEX name_upper_idx;'
        ),
        migrations.RunSQL(
            sql=r'CREATE UNIQUE INDEX name_upper_idx ON app_solvent(UPPER(Name));',
            reverse_sql=r'DROP INDEX name_upper_idx;'
        ),
    ]
