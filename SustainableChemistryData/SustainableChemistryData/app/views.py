"""
Definition of views.
"""

from django.shortcuts import render, render_to_response
from django.http import HttpRequest, HttpResponse, HttpResponseRedirect
from django.template import RequestContext
from datetime import datetime
from app.models import *
from app.forms import *
from django.views.generic import *
from django.urls import reverse_lazy

# Class-based Functional Group Views

class FunctionalGroupList(ListView):
    model = FunctionalGroup


class FunctionalGroupDetail(DetailView):
    model = FunctionalGroup

    def get_success_url(self):
        return reverse('FunctionalGroup_List')


class FunctionalGroupCreate(CreateView):
    model = FunctionalGroup
    fields = ['Name', 'Smarts', 'Image']


class FunctionalGroupUpdate(UpdateView):
    model = FunctionalGroup
    fields = ['Name', 'Smarts', 'Image']


class FunctionalGroupDelete(DeleteView):
    model = FunctionalGroup
    success_url = reverse_lazy('FunctionalGroup_List')


# Class-based Named Reaction Views

class ReactionList(ListView):
    model = NamedReaction


class ReactionDetail(DetailView):
    model = NamedReaction


class ReactionCreate(CreateView):
    model = NamedReaction
    form_class = NamedReactionForm
    
    def get_success_url(self):
        return reverse('NamedReaction_List')


class ReactionUpdate(UpdateView):
    model = NamedReaction
    form_class = NamedReactionForm


class ReactionDelete(DeleteView):
    model = NamedReaction
    success_url = reverse_lazy('NamedReaction_List')

# Class-based Reference Views

class ReferenceList(ListView):
    model = Reference


class ReferenceCreate(CreateView):
    model = Reference
    fields = ['Reaction', 'Functional_Group', 'RISData']
    
    def get_success_url(self):
        return reverse('Reference_List')


class ReferenceDetail(DetailView):
    model = Reference


class ReferenceUpdate(UpdateView):
    model = Reference
    fields = ['Reaction', 'Functional_Group', 'RISData']


class ReferenceDelete(DeleteView):
    model = Reference
    success_url = reverse_lazy('Reference_List')


# Class-based Solvent Views

class SolventList(ListView):
    model = Solvent


class SolventUpdate(UpdateView):
    model = Solvent
    fields = ['Name', ]


# Class-based Catalyst Views

class CatalystList(ListView):
    model = Catalyst


class CatalystUpdate(UpdateView):
    model = Catalyst
    fields = ['Name', ]


# Class-based Reactant Views

class ReactantList(ListView):
    model = Reactant


class ReactantCreate(CreateView):
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


class ReactantUpdate(UpdateView):
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
        'app/index.html',
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
        'app/contact.html',
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
        'app/about.html',
        {
            'title': 'About',
            'message': 'Your application description page.',
            'year': datetime.now().year,
        }
    )
