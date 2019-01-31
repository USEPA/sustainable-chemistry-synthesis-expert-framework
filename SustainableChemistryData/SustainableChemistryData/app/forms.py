"""
Definition of forms.
"""

from django import forms
from django.contrib.auth.forms import AuthenticationForm
from django.utils.translation import ugettext_lazy as _
from app.models import NamedReaction
from django.forms.utils import ErrorList
from django.core.exceptions import NON_FIELD_ERRORS
from django.forms import ClearableFileInput

# https://stackoverflow.com/questions/19774392/customize-the-styles-of-django-clearablefileinput-widget
class AvatarInput(ClearableFileInput):
    '''renders the input file as an avatar image, and removes the 'currently' html'''

    template_with_initial = u'%(initial)s %(clear_template)s<br />%(input_text)s: %(input)s'

    def render(self, name, value, attrs=None):
        substitutions = {
            'input_text': self.input_text,
            'clear_template': '',
            'clear_checkbox_label': self.clear_checkbox_label,
        }
        template = u'%(input)s'
        substitutions['input'] = super(AvatarInput, self).render(name, value, attrs)

        if value and hasattr(value, "url"):
            template = self.template_with_initial
            substitutions['initial'] = (u'<img src="%s" width="60" height="60"></img>'
                                    % (escape(value.url)))
            if not self.is_required:
                checkbox_name = self.clear_checkbox_name(name)
                checkbox_id = self.clear_checkbox_id(checkbox_name)
                substitutions['clear_checkbox_name'] = conditional_escape(checkbox_name)
                substitutions['clear_checkbox_id'] = conditional_escape(checkbox_id)
                substitutions['clear'] = CheckboxInput().render(checkbox_name, False, attrs={'id': checkbox_id})
                substitutions['clear_template'] = self.template_with_clear % substitutions

        return mark_safe(template % substitutions)


# Import the Admin FilteredMultipleSelect input widget
from django.contrib.admin.widgets import FilteredSelectMultiple

#https://docs.djangoproject.com/en/2.1/ref/forms/api/#customizing-the-error-list-format
class DivErrorList(ErrorList):
    def __str__(self):
        return self.as_divs()
    def as_divs(self):
        if not self: return ''
        return '<div class="errorlist">%s</div>' % ''.join(['<div class="alert alert-danger" role="alert">%s</div>' % e for e in self])


# ModelForm to use the FilteredMultipleSelect Widget
class NamedReactionForm(forms.ModelForm):
    class Meta:
        model = NamedReaction
        fields = ['Name', 'Functional_Group', 'Product', 'URL', 'Reactants',
                  'ByProducts', 'Heat', 'AcidBase', 'Catalyst', 'Solvent', 'Image']
        widgets = {
            'Reactants': FilteredSelectMultiple('Reactants', False),
            'ByProducts': FilteredSelectMultiple('By Products', False),
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
