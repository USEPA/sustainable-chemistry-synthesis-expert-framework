"""
Definition of views.
"""

from django.shortcuts import render, render_to_response
from django.http import HttpRequest, HttpResponse, HttpResponseRedirect
from django.template import RequestContext
from datetime import datetime
from app.models import *

def functionalGroup(request):
    functionalGroups = FunctionalGroup.objects.all
    return render_to_response('app/FunctionalGroups.html', { 'functionalGroups': functionalGroups})

def functionalGroupDetails(request, id):
    functionalGroup = FunctionalGroup.objects.get(pk = id)
    return render_to_response('app/FunctionalGroupDetails.html', { 'functionalGroup': functionalGroup})

def FunctionalGroup_Create(request):
    if request.Method == 'GET':
        form = FunctionalGroupForm()
        return render(request, 'app/Create.html', { 'form':form })
    if request.Method == 'POST':
        form = FunctionalGroupForm(request.POST)
        form.save()
        return render(request, '/FunctionalGroups', { 'form':form })

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
