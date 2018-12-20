"""
Definition of models.
"""

from django.db import models
from django import forms

# Create your models here.

class FunctionalGroup (models.Model):
    Name = models.TextField("Functional Group")
    Smarts = models.TextField()

class FunctionalGroupForm(forms.ModelForm):
    class Meta:
        model = FunctionalGroup
        fields = ['Name', 'Smarts']

class NamedReaction (models.Model):
    Name = models.TextField("Named Reaction")
    Functional_Group = models.TextField("Functional Group", null=True)
    URL = models.TextField(null=True)
    ReactantA = models.TextField(null=True)
    ReactantB = models.TextField(null=True)
    ReactantC = models.TextField(null=True)
    Product = models.TextField(null=True)
    Heat = models.TextField(null=True)
    AcidBase = models.TextField(null=True)
    Catalyst = models.TextField(null=True)
    Solvent = models.TextField(null=True)
    ByProducts = models.TextField(null=True)

class Reference (models.Model):
    Name = models.TextField("Named Reaction", null=True)
    Functional_Group = models.TextField("Functional Group")
    RISData = models.TextField()
