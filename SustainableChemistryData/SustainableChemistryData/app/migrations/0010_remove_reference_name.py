# Generated by Django 2.1.5 on 2019-01-25 04:28

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('app', '0009_auto_20190121_1412'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='reference',
            name='Name',
        ),
    ]