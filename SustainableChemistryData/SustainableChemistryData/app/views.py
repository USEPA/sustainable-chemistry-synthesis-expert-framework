"""
Definition of views.
"""

from django.shortcuts import render, render_to_response, redirect
from django.http import HttpRequest, HttpResponse, HttpResponseRedirect
from django.template import RequestContext
from datetime import datetime
from app.models import *
from app.forms import *
from django.views.generic import *
from django.urls import reverse_lazy
from django.contrib.auth.mixins import LoginRequiredMixin

# Class-based Functional Group Views

class FunctionalGroupList(ListView):
    """Add class docstring"""

    model = FunctionalGroup


class FunctionalGroupDetail(DetailView):
    """Add class docstring"""

    model = FunctionalGroup

    def get_success_url(self):
        """Add method docstring"""
        return reverse('FunctionalGroup_List')


class FunctionalGroupCreate(LoginRequiredMixin, CreateView):
    """Add class docstring"""

    model = FunctionalGroup
    fields = ['Name', 'Smarts', 'Image']
    widgets = {
        'Image': AdminFileWidget(),
    }


class FunctionalGroupUpdate(LoginRequiredMixin, UpdateView):
    """Add class docstring"""

    model = FunctionalGroup
    fields = ['Name', 'Smarts', 'Image']


class FunctionalGroupDelete(LoginRequiredMixin, DeleteView):
    """Add class docstring"""

    model = FunctionalGroup
    success_url = reverse_lazy('FunctionalGroup_List')


# Class-based Named Reaction Views

class ReactionList(ListView):
    """Add class docstring"""

    model = NamedReaction


class ReactionDetail(DetailView):
    """Add class docstring"""

    model = NamedReaction


class ReactionCreate(LoginRequiredMixin, CreateView):
    """Add class docstring"""

    model = NamedReaction
    form_class = NamedReactionForm

    def get_success_url(self):
        return reverse('NamedReaction_List')


class ReactionUpdate(LoginRequiredMixin, UpdateView):
    """Add class docstring"""

    model = NamedReaction
    form_class = NamedReactionForm


class ReactionDelete(LoginRequiredMixin, DeleteView):
    """Add class docstring"""

    model = NamedReaction
    success_url = reverse_lazy('NamedReaction_List')

# Class-based Reference Views

class ReferenceList(ListView):
    """Add class docstring"""

    model = Reference


class ReferenceCreate(LoginRequiredMixin, CreateView):
    """Add class docstring"""

    model = Reference
    fields = ['Reaction', 'Functional_Group', 'RISData']

    def get_success_url(self):
        return reverse('Reference_List')


class ReferenceDetail(DetailView):
    """Add class docstring"""

    model = Reference


class ReferenceUpdate(LoginRequiredMixin, UpdateView):
    """Add class docstring"""

    model = Reference
    fields = ['Reaction', 'Functional_Group', 'RISData']


class ReferenceDelete(LoginRequiredMixin, DeleteView):
    """Add class docstring"""

    model = Reference
    success_url = reverse_lazy('Reference_List')


# Class-based Solvent Views

class SolventList(ListView):
    """Add class docstring"""

    model = Solvent


class SolventCreate(LoginRequiredMixin, CreateView):
    """Add class docstring"""

    model = NamedReaction
    form_class = NamedReactionForm

    def get_success_url(self):
        return reverse('NamedReaction_List')

class SolventUpdate(LoginRequiredMixin, UpdateView):
    """Add class docstring"""

    model = Solvent
    fields = ['Name', ]


# Class-based Catalyst Views

class CatalystList(ListView):
    """Add class docstring"""

    model = Catalyst


class CatalystCreate(LoginRequiredMixin, CreateView):
    """Add class docstring"""

    model = NamedReaction
    form_class = NamedReactionForm

    def get_success_url(self):
        return reverse('NamedReaction_List')

class CatalystUpdate(LoginRequiredMixin, UpdateView):
    """Add class docstring"""

    model = Catalyst
    fields = ['Name', ]


# Class-based Reactant Views

class ReactantList(ListView):
    """Add class docstring"""

    model = Reactant


class ReactantCreate(LoginRequiredMixin, CreateView):
    """Add class docstring"""

    model = Reactant
    fields = ['Name', 'Description', 'Temp2']

    def get_form_kwargs(self, **kwargs):
        kwargs = super(ReactantCreate, self).get_form_kwargs()
        redirect = self.request.GET.get('next')
        if redirect:
            if 'initial' in kwargs.keys():
                kwargs['initial'].update({'next': redirect})
            else:
                kwargs['initial'] = {'next': redirect}
        return kwargs

    def form_invalid(self, form):
        import pdb;pdb.set_trace()  # debug example
        # inspect the errors by typing the variable form.errors
        # in your command line debugger. See the pdb package for
        # more useful keystrokes
        return super(ReactantCreate, self).form_invalid(form)

    def form_valid(self, form):
        redirect = form.initial.get('next')
        if redirect:
            self.success_url = redirect
        return super(ReactantCreate, self).form_valid(form)


class ReactantUpdate(LoginRequiredMixin, UpdateView):
    """Add class docstring"""

    model = Reactant
    fields = ['Name', 'Description', 'Temp2']


# def functionalGroups(request):
#    functionalGroups = FunctionalGroup.objects.all
#    return render_to_response('app/FunctionalGroup_List.html', { 'functionalGroups': functionalGroups})

# def functionalGroupDetails(request, id):
#    functionalGroup = FunctionalGroup.objects.get(pk = id)
#    return render_to_response('app/FunctionalGroup_Detail.html', { 'FunctionalGroup': functionalGroup})

# def functionalGroupCreate(request):
#    if request.method == 'GET':
#        form = FunctionalGroupForm()
#        return render(request, 'app/Create.html', { 'form':form })
#    if request.method == 'POST':
#        form = FunctionalGroupForm(request.POST)
#        form.save()
#        return HttpResponseRedirect('/FunctionalGroups', { 'form':form })


def home(request):
    """Renders the home page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'index.html',
        {
            'title': 'Home Page',
            'year': datetime.now().year,
        }
    )


def contact(request):
    """Renders the contact page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'contact.html',
        {
            'title': 'Contact',
            'message': 'Your contact page.',
            'year': datetime.now().year,
        }
    )


def about(request):
    """Renders the about page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'about.html',
        {
            'title': 'About',
            'message': 'Your application description page.',
            'year': datetime.now().year,
        }
    )


# New user sign up from https://simpleisbetterthancomplex.com/tutorial/2017/02/18/how-to-create-user-sign-up-view.html
from django.contrib.auth import login, authenticate
from django.contrib.auth.forms import UserCreationForm

def signup(request):
    """Add function docstring"""
    if request.method == 'POST':
        form = SignUpForm(request.POST)
        if form.is_valid():
            form.save()
            username = form.cleaned_data.get('username')
            raw_password = form.cleaned_data.get('password1')
            user = authenticate(username=username, password=raw_password)
            login(request, user)
            return redirect('home')
    else:
        form = SignUpForm()
    return render(request, 'registration/signup.html', {'form': form})
