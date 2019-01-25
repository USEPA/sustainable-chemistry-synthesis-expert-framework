"""
Definition of forms.
"""

from django import forms
from django.contrib.auth.forms import AuthenticationForm
from django.utils.translation import ugettext_lazy as _
from django.contrib.admin.widgets import FilteredSelectMultiple
from app.models import FunctionalGroup, NamedReaction
from django.forms.utils import ErrorList
from django.core.exceptions import NON_FIELD_ERRORS

#https://docs.djangoproject.com/en/2.1/ref/forms/api/#customizing-the-error-list-format
class DivErrorList(ErrorList):
    def __str__(self):
        return self.as_divs()
    def as_divs(self):
        if not self: return ''
        return '<div class="errorlist">%s</div>' % ''.join(['<div class="alert alert-danger" role="alert">%s</div>' % e for e in self])

class FunctionalGroupForm(forms.ModelForm):
    class Meta:
        model = FunctionalGroup
        fields = ['Name', 'Smarts', 'Image']


class NamedReactionForm(forms.ModelForm):
    class Meta:
        model = NamedReaction
        fields = ['Name', 'Functional_Group', 'Product', 'Reactants',
                  'ByProducts', 'Heat', 'AcidBase', 'Catalyst', 'Solvent', 'Image']
        widgets = {
            'Reactants': FilteredSelectMultiple('Reactants', False),
            'ByProducts': FilteredSelectMultiple('By Products', False),
        }
        #error_messages = {
        #    NON_FIELD_ERRORS: {
        #        'unique_together': "%(model_name)s's %(field_labels)s are not unique.",
        #    }
        #}

    class Media:
        css = {'all': ('/static/admin/css/widgets.css',), }
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
                                   'placeholder': 'Password'}))
