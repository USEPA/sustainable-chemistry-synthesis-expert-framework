"""
Definition of views.
"""

from django.shortcuts import render, render_to_response
from django.http import HttpRequest, HttpResponse, HttpResponseRedirect
from django.template import RequestContext
from datetime import datetime
from app.models import *
from django.views.generic import *
from django.core.urlresolvers import reverse_lazy

class FunctionalGroupList(ListView):
    model = FunctionalGroup

class FunctionalGroupDetail(DetailView):
    model = FunctionalGroup

class FunctionalGroupCreate(CreateView):
    model = FunctionalGroup
    fields = ['Name', 'Smarts', 'Image']

class FunctionalGroupUpdate(UpdateView):
    model = FunctionalGroup
    fields = ['Name', 'Smarts', 'Image']

class FunctionalGroupDelete(DeleteView):
    model = FunctionalGroup
    success_url = reverse_lazy('FunctionalGroup_List') 

def functionalGroups(request):
    functionalGroups = FunctionalGroup.objects.all
    return render_to_response('app/FunctionalGroup_List.html', { 'functionalGroups': functionalGroups})

def functionalGroupDetails(request, id):
    functionalGroup = FunctionalGroup.objects.get(pk = id)
    return render_to_response('app/FunctionalGroup_Detail.html', { 'FunctionalGroup': functionalGroup})

def functionalGroupCreate(request):
    if request.method == 'GET':
        form = FunctionalGroupForm()
        return render(request, 'app/Create.html', { 'form':form })
    if request.method == 'POST':
        form = FunctionalGroupForm(request.POST)
        form.save()
        return HttpResponseRedirect('/FunctionalGroups', { 'form':form })

def home(request):
    """Renders the home page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'app/index.html',
        {
            'title':'Home Page',
            'year':datetime.now().year,
        }
    )

def contact(request):
    """Renders the contact page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'app/contact.html',
        {
            'title':'Contact',
            'message':'Your contact page.',
            'year':datetime.now().year,
        }
    )

def about(request):
    """Renders the about page."""
    assert isinstance(request, HttpRequest)
    return render(
        request,
        'app/about.html',
        {
            'title':'About',
            'message':'Your application description page.',
            'year':datetime.now().year,
        }
    )
