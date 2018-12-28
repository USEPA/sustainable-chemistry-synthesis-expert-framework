"""
Definition of urls for SustainableChemistryData.
"""

from datetime import datetime
from django.conf.urls import url
import django.contrib.auth.views

import app.forms
import app.views

# Adding static filese per https://docs.djangoproject.com/en/1.11/howto/static-files/
from django.conf import settings
from django.conf.urls.static import static

# Uncomment the next lines to enable the admin:
# from django.conf.urls import include
# from django.contrib import admin
# admin.autodiscover()

urlpatterns = [

    #FunctionalGroups
    #url(r'^FunctionalGroups$', app.views.functionalGroups, name='functionalGroup'),
    url(r'^FunctionalGroups$', app.views.FunctionalGroupList.as_view(), name='FunctionalGroup_List'),
    #url(r'^FunctionalGroup/(?P<id>\d+)$', app.views.functionalGroupDetails, name='functionalGroupDetails'),
    url(r'^FunctionalGroup/Create$', app.views.FunctionalGroupCreate.as_view(), name='FunctionalGroup_Create'),
    #url(r'^FunctionalGroup/Create$', app.views.functionalGroupCreate, name='functionalGroupCreate'),
    url(r'^FunctionalGroup/(?P<pk>\d+)$', app.views.FunctionalGroupDetail.as_view(), name='FunctionalGroup_Detail'),
    url(r'^FunctionalGroup/Update/(?P<pk>\d+)$', app.views.FunctionalGroupUpdate.as_view(), name='FunctionalGroup_Update'),
    url(r'^FunctionalGroup/Delete/(?P<pk>\d+)$', app.views.FunctionalGroupDelete.as_view(), name='FunctionalGroup_Delete'),

    # Examples:
    url(r'^$', app.views.home, name='home'),
    url(r'^contact$', app.views.contact, name='contact'),
    url(r'^about$', app.views.about, name='about'),
    url(r'^login/$',
        django.contrib.auth.views.login,
        {
            'template_name': 'app/login.html',
            'authentication_form': app.forms.BootstrapAuthenticationForm,
            'extra_context':
            {
                'title': 'Log in',
                'year': datetime.now().year,
            }
        },
        name='login'),
    url(r'^logout$',
        django.contrib.auth.views.logout,
        {
            'next_page': '/',
        },
        name='logout'),

    # Uncomment the admin/doc line below to enable admin documentation:
    # url(r'^admin/doc/', include('django.contrib.admindocs.urls')),

    # Uncomment the next line to enable the admin:
    # url(r'^admin/', include(admin.site.urls)),
]


if settings.DEBUG is True:
    urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)