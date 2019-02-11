
"""
Definition of models.
"""

from django.db import models
from django import forms
from django.urls import reverse
from app.fields import CICharField

# User profile information, see https://simpleisbetterthancomplex.com/tutorial/2016/11/23/how-to-add-user-profile-to-django-admin.html

from django.contrib.auth.models import User
from django.db.models.signals import post_save
from django.dispatch import receiver

class Profile(models.Model):
    """Add class docstring"""
    user = models.OneToOneField(User, on_delete=models.CASCADE)
    organization = models.CharField(max_length=150, blank=True, verbose_name='Organization')
    address1 = models.CharField(max_length=150, blank=True, verbose_name='Address 1')
    address2 = models.CharField(max_length=150, blank=True, verbose_name='Address 2')
    city = models.CharField(max_length=150, blank=True, verbose_name='City')
    state = models.CharField(max_length=150, blank=True, verbose_name='State')
    country = models.CharField(max_length=150, blank=True, verbose_name='Country')

    def __str__(self):  # __unicode__ for Python 2
        return self.user.username

@receiver(post_save, sender=User)
def create_or_update_user_profile(sender, instance, created, **kwargs):
    """Add function docstring"""
    if created:
        Profile.objects.create(user=instance)
        instance.profile.save()


# Create your models here.

class FunctionalGroup(models.Model):
    """Add class docstring"""

    Name = CICharField(
        max_length=150, unique=True, help_text="The name of the functional group.")
    Smarts = models.CharField(
        max_length=150, unique=True, help_text="Structure of the functional group as a SMILES string.")
    Image = models.ImageField(upload_to='Images/FunctionalGroups/')

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        """Add method docstring"""
        return reverse('FunctionalGroup_Detail', kwargs={'pk': self.pk})

def user_directory_path(instance, filename):
    """Add function docstring"""
    ext = filename.split('.')[-1]
    if instance.pk:
        return '/images/FunctionalGroup/{}.{}'.format(instance.Name, ext)
    else:
        pass
        # do something if pk is not there yet


class NamedReaction(models.Model):
    """Add class docstring"""

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
        """Add class docstring"""

        unique_together = ('Functional_Group', 'Name')

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        """Add method docstring"""
        return reverse('NamedReaction_Detail', kwargs={'pk': self.pk})

class Reference(models.Model):
    """Add class docstring"""

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
        """Add method docstring"""
        return reverse('Reference_Detail', kwargs={'pk': self.pk})


class Compound(models.Model):
    """Add class docstring"""

    Name = CICharField(max_length=150, unique=True,)
    Description = models.TextField()
    CasNumber = models.CharField(max_length=10, blank=True)

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        """Add method docstring"""
        return reverse('Solvent_Detail', kwargs={'pk': self.pk})


class Solvent(models.Model):
    """Add class docstring"""

    Name = CICharField(max_length=150, unique=True,)
    Description = models.TextField()

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        """Add method docstring"""
        return reverse('Solvent_Detail', kwargs={'pk': self.pk})


class Reactant(models.Model):
    """Add class docstring"""

    Name = CICharField(max_length=150, unique=True,)
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
        """Add method docstring"""
        return reverse('Reactant_Detail', kwargs={'pk': self.pk})


class Catalyst(models.Model):
    """Add class docstring"""

    Name = CICharField(max_length=150, unique=True,)
    Description = models.TextField()

    def __str__(self):
        return self.Name

    def get_absolute_url(self):
        """Add method docstring"""
        return reverse('Catalyst_Detail', kwargs={'pk': self.pk})
