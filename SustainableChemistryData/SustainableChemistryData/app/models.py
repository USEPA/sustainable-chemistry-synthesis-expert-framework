"""
Definition of models.
"""

from django.db import models
from django import forms
from django.core.urlresolvers import reverse

# Create your models here.

class FunctionalGroup (models.Model):
    Name = models.CharField(max_length=150)
    Smarts = models.CharField(max_length=150)
    Image = models.ImageField(upload_to='Images/FunctionalGroups/')

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        return reverse('FunctionalGroup_Detail', kwargs={ 'pk': self.pk})

def user_directory_path(instance, filename):
    ext = filename.split('.')[-1]
    if instance.pk:
        return '/images/FunctionalGroup/{}.{}'.format(instance.Name, ext)
    else:
        pass
        # do something if pk is not there yet

class FunctionalGroupForm(forms.ModelForm):
    class Meta:
        model = FunctionalGroup
        fields = ['Name', 'Smarts', 'Image']

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
