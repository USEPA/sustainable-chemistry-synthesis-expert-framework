# Generated by Django 2.1.5 on 2019-01-29 22:06

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('app', '0016_auto_20190129_1252'),
    ]

    operations = [
        migrations.AlterUniqueTogether(
            name='namedreaction',
            unique_together={('Functional_Group', 'Name')},
        ),
    ]
