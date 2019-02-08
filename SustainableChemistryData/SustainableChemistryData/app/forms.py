"""
Definition of forms.
"""

from django import forms
from django.contrib.auth.forms import AuthenticationForm, UserCreationForm
from django.contrib.auth.models import User
from django.utils.translation import ugettext_lazy as _
from app.models import NamedReaction
from django.forms.utils import ErrorList
from django.core.exceptions import NON_FIELD_ERRORS

# Import the Admin FilteredMultipleSelect input widget
from django.contrib.admin.widgets import FilteredSelectMultiple, AdminFileWidget

#https://docs.djangoproject.com/en/2.1/ref/forms/api/#customizing-the-error-list-format
class DivErrorList(ErrorList):
    def __str__(self):
        return self.as_divs()
    def as_divs(self):
        if not self: return ''
        return '<div class="errorlist">%s</div>' % ''.join(['<div class="alert alert-danger" role="alert">%s</div>' % e for e in self])

# New user sign up from https://simpleisbetterthancomplex.com/tutorial/2017/02/18/how-to-create-user-sign-up-view.html
class SignUpForm(UserCreationForm):
    first_name = forms.CharField(max_length=30, help_text='Required.')
    last_name = forms.CharField(max_length=30, help_text='Required.')
    email = forms.EmailField(max_length=254, help_text='Required. Please enter your email address.')

    class Meta:
        model = User
        fields = ('username', 'first_name', 'last_name', 'email', 'password1', 'password2', )


# ModelForm to use the FilteredMultipleSelect Widget
class NamedReactionForm(forms.ModelForm):
    class Meta:
        model = NamedReaction
        fields = ['Name', 'Functional_Group', 'Product', 'URL', 'Reactants',
                  'ByProducts', 'Heat', 'AcidBase', 'Catalyst', 'Solvent', 'Image']
        widgets = {
            'Reactants': FilteredSelectMultiple('Reactants', False),
            'ByProducts': FilteredSelectMultiple('By Products', False),
            'Image': AdminFileWidget(),
        }

    #Required for the FiliteredMultipleSelected Widegt
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
