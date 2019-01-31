# Case insensitive character fields from http://concisecoder.io/2018/10/27/case-insensitive-fields-in-django-models/
from django.db import models 

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
        converted = self.LOOKUP_CONVERSIONS.get(lookup_name, lookup_name)
        return super().get_lookup(converted)

class CICharField(CaseInsensitiveFieldMixin, models.CharField):
    pass

from django.db import migrations
class Migration(migrations.Migration):
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

