"""
Definition of forms.
"""

from django import forms
from django.contrib.auth.forms import AuthenticationForm
from django.utils.translation import ugettext_lazy as _
from django_select2.forms import HeavySelect2Widget
from django.contrib.admin.widgets import FilteredSelectMultiple
from app.models import *

class NamedReactionForm(forms.ModelForm):
    class Meta:
        model = NamedReaction
        fields = ['Name', 'Functional_Group', 'Product', 'Reactants', 'ByProd', 'Temp', 'Temp2', 'Catal', 'Solv', 'Image']
        widgets = {
            'Reactants': FilteredSelectMultiple('Reactant', False),
            'ByProd': FilteredSelectMultiple('Reactant', False)
        }

    class Media:
        css = {'all': ('/static/admin/css/widgets.css',),}
        js = ('/jsi18n',)
 
class BootstrapAuthenticationForm(AuthenticationForm):
    """Authentication form which uses boostrap CSS."""
    username = forms.CharField(max_length=254,
                               widget=forms.TextInput({
                                   'class': 'form-control',
                                   'placeholder': 'User name'}))
    password = forms.CharField(label=_("Password"),
                               widget=forms.PasswordInput({
                                   'class': 'form-control',
                                   'placeholder':'Password'}))
