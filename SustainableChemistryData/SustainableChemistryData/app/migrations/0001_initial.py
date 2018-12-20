# -*- coding: utf-8 -*-
# Generated by Django 1.11.17 on 2018-12-19 19:47
from __future__ import unicode_literals

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='FunctionalGroup',
            fields=[
                ('Name', models.TextField(primary_key=True, serialize=False, verbose_name='Functional Group')),
                ('Smarts', models.TextField()),
            ],
        ),
        migrations.CreateModel(
            name='NamedReaction',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('Name', models.TextField(verbose_name='Named Reaction')),
                ('URL', models.TextField()),
                ('ReactantA', models.TextField()),
                ('ReactantB', models.TextField()),
                ('ReactantC', models.TextField()),
                ('Product', models.TextField()),
                ('Heat', models.TextField()),
                ('AcidBase', models.TextField()),
                ('Catalyst', models.TextField()),
                ('Solvent', models.TextField()),
                ('ByProducts', models.TextField()),
                ('Functional_Group', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.FunctionalGroup')),
            ],
        ),
        migrations.CreateModel(
            name='Reference',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('RISData', models.TextField()),
                ('Functional_Group', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.FunctionalGroup')),
                ('Named_Reaction', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='app.NamedReaction')),
            ],
        ),
    ]