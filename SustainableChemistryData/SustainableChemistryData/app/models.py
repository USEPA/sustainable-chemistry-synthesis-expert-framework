"""
Definition of models.
"""

from django.db import models
from django import forms
from django.urls import reverse
from app.fields import CICharField


# Create your models here.

class FunctionalGroup (models.Model):
    Name = CICharField(
        max_length=150, unique = True, help_text="The name of the functional group.")
    Smarts = models.CharField(
        max_length=150, unique = True, help_text="Structure of the functional group as a SMILES string.")
    Image = models.ImageField(upload_to='Images/FunctionalGroups/')

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        return reverse('FunctionalGroup_Detail', kwargs={'pk': self.pk})

def user_directory_path(instance, filename):
    ext = filename.split('.')[-1]
    if instance.pk:
        return '/images/FunctionalGroup/{}.{}'.format(instance.Name, ext)
    else:
        pass
        # do something if pk is not there yet


class NamedReaction (models.Model):
    Name = CICharField(max_length=150)
    Functional_Group = models.ForeignKey(
        'FunctionalGroup',
        on_delete=models.PROTECT
    )
    URL = models.URLField(blank=True)
    ReactantA = models.CharField(max_length=150, blank=True)
    ReactantB = models.CharField(max_length=150, blank=True)
    ReactantC = models.CharField(max_length=150, blank=True)
    Reactants = models.ManyToManyField('Reactant', related_name='Reactant')
    Product = models.CharField(max_length=150, blank=True)
    NONE = 'NA'
    HEAT = 'HE'
    HEAT_CHOICES = (
        ('NA', 'None'),
        ('HE', 'Heat'),
    )
    Heat = models.CharField(
        max_length=2,
        choices=HEAT_CHOICES,
        default=NONE,
        verbose_name='Heated Reaction',
    )
    ACID = 'AC'
    ACID_BASE = 'AB'
    BASE = 'BA'
    ACID_BASE_CHOICES = (
        ('AC', 'Acid'),
        ('AB', 'Acid Or Base'),
        ('BA', 'Base'),
        ('NA', 'Not Applicable'),
    )
    AcidBase = models.CharField(
        max_length=2,
        choices=ACID_BASE_CHOICES,
        default=NONE,
        verbose_name='Acid or Base Conditions:',
    )
    Catalyst = models.ForeignKey(
        'Catalyst',
        on_delete=models.PROTECT,
    )
    Solvent = models.ForeignKey(
        'Solvent',
        on_delete=models.PROTECT,
    )
    ByProducts = models.ManyToManyField(
        'Reactant',
        related_name='ByProduct',
        blank=True,
        verbose_name='Reaction By-Products:',
    )
    Image = models.ImageField(upload_to='Images/Reactions/')

    class Meta:
        unique_together = ('Functional_Group', 'Name')

    def __str__(self):
        return self.Name
    
    def get_absolute_url(self):
        return reverse('NamedReaction_Detail', kwargs={'pk': self.pk})
   

class Reference (models.Model):
    Functional_Group = models.ForeignKey(
        'FunctionalGroup',
        on_delete=models.PROTECT,
        null=True
    )
    Reaction = models.ForeignKey(
        'NamedReaction',
        on_delete=models.PROTECT,
        null=True
    )
    RISData = models.TextField()

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        return reverse('Reference_Detail', kwargs={'pk': self.pk})


class Compound (models.Model):
    Name = CICharField(max_length=150, unique = True, )
    Description = models.TextField()
    CasNumber = models.CharField(max_length=10, blank=True)

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        return reverse('Solvent_Detail', kwargs={'pk': self.pk})


class Solvent (models.Model):
    Name = CICharField(max_length=150, unique = True, )
    Description = models.TextField()

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        return reverse('Solvent_Detail', kwargs={'pk': self.pk})


class Reactant (models.Model):
    Name = CICharField(max_length=150, unique = True, )
    Description = models.TextField()
    FUNCTIONAL_GROUP = 'FG'
    COMPOUND = 'CO'
    REACTANT_TYPE_CHOICES = (
        ('FG', 'Functional Group'),
        ('CO', 'Compound'),
    )
    Temp2 = models.CharField(
        max_length=2,
        choices=REACTANT_TYPE_CHOICES,
        default=COMPOUND,
        verbose_name='Reactant Type:',
    )

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        return reverse('Reactant_Detail', kwargs={'pk': self.pk})


class Catalyst (models.Model):
    Name = CICharField(max_length=150, unique = True, )
    Description = models.TextField()

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        return reverse('Catalyst_Detail', kwargs={'pk': self.pk})

