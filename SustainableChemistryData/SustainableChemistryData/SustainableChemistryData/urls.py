"""
Definition of urls for SustainableChemistryData.
"""

from datetime import datetime
from django.conf.urls import url
from django.contrib.auth.views import LoginView, LogoutView
import app.views
import app.forms

# Reverse after login/logut
from django.urls import reverse_lazy
LOGIN_REDIRECT_URL = reverse_lazy('dashboard')
LOGIN_URL = reverse_lazy('login')
LOGOUT_URL = reverse_lazy('logout')


# Adding static files per https://docs.djangoproject.com/en/1.11/howto/static-files/
from django.conf import settings
from django.conf.urls.static import static

# Uncomment the next lines to enable the admin:
from django.urls import path, include
from django.contrib import admin
admin.autodiscover()

# Required for FilteredMultipleSelect
from django.views.i18n import JavaScriptCatalog

urlpatterns = [
    # FunctionalGroups
    url(r'^FunctionalGroups$', app.views.FunctionalGroupList.as_view(),
        name='FunctionalGroup_List'),
    url(r'^FunctionalGroup/Create$', app.views.FunctionalGroupCreate.as_view(),
        name='FunctionalGroup_Create'),
    url(r'^FunctionalGroup/(?P<pk>\d+)$',
        app.views.FunctionalGroupDetail.as_view(), name='FunctionalGroup_Detail'),
    url(r'^FunctionalGroup/Update/(?P<pk>\d+)$',
        app.views.FunctionalGroupUpdate.as_view(), name='FunctionalGroup_Update'),
    url(r'^FunctionalGroup/Delete/(?P<pk>\d+)$',
        app.views.FunctionalGroupDelete.as_view(), name='FunctionalGroup_Delete'),

    # NamedReactions
    url(r'^Reactions$', app.views.ReactionList.as_view(), name='NamedReaction_List'),
    url(r'^Reaction/Create$', app.views.ReactionCreate.as_view(),
        name='NamedReaction_Create'),
    url(r'^Reaction/(?P<pk>\d+)$', app.views.ReactionDetail.as_view(),
        name='NamedReaction_Detail'),
    url(r'^Reaction/Update/(?P<pk>\d+)$',
        app.views.ReactionUpdate.as_view(), name='NamedReaction_Update'),
    url(r'^Reaction/Delete/(?P<pk>\d+)$',
        app.views.ReactionDelete.as_view(), name='NamedReaction_Delete'),

    # References
    url(r'^References$', app.views.ReferenceList.as_view(), name='Reference_List'),
    url(r'^Reference/(?P<pk>\d+)$',
        app.views.ReferenceDetail.as_view(), name='Reference_Detail'),
    url(r'^Reference/Create$', app.views.ReferenceCreate.as_view(),
        name='Reference_Create'),
    url(r'^Reference/Update/(?P<pk>\d+)$',
        app.views.ReferenceUpdate.as_view(), name='Reference_Update'),
    url(r'^Reference/Delete/(?P<pk>\d+)$',
        app.views.ReferenceDelete.as_view(), name='Reference_Delete'),

    # Reactants
    url(r'^Reactants$', app.views.ReactantList.as_view(), name='Reactant_List'),
    #url(r'^Reactant/(?P<pk>\d+)$',
    #    app.views.ReactantDetail.as_view(), name='Reactant_Detail'),
    url(r'^Reactant/Create$', app.views.ReactantCreate.as_view(),
        name='Reactant_Create'),
    url(r'^Reactant/Update/(?P<pk>\d+)$',
        app.views.ReactantUpdate.as_view(), name='Reactant_Update'),
    #url(r'^Reactant/Delete/(?P<pk>\d+)$',
    #    app.views.ReactantDelete.as_view(), name='Reactant_Delete'),

    # Solvents
    url(r'^Solvents$', app.views.SolventList.as_view(), name='Solvent_List'),
    #url(r'^Solvent/(?P<pk>\d+)$',
    #    app.views.SolventDetail.as_view(), name='Solvent_Detail'),
    url(r'^Solvent/Update/(?P<pk>\d+)$',
        app.views.SolventUpdate.as_view(), name='Solvent_Update'),
    #url(r'^Solvent/Delete/(?P<pk>\d+)$',
    #    app.views.SolventDelete.as_view(), name='Solvent_Delete'),

    # Catalysts
    url(r'^Catalysts$', app.views.CatalystList.as_view(), name='Catalyst_List'),
    #url(r'^Catalyst/(?P<pk>\d+)$',
    #    app.views.CatalystDetail.as_view(), name='Catalyst_Detail'),
    url(r'^Catalyst/Update/(?P<pk>\d+)$',
        app.views.CatalystUpdate.as_view(), name='Catalyst_Update'),
    #url(r'^Catalyst/Delete/(?P<pk>\d+)$',
    #    app.views.CatalystDelete.as_view(), name='Catalyst_Delete'),

    # Examples:
    url(r'^$', app.views.home, name='home'),
    url(r'^contact$', app.views.contact, name='contact'),
    url(r'^about$', app.views.about, name='about'),
    url(r'^login/$', LoginView.as_view(template_name='app/login.html'), name='login'),
    url(r'^logout$', LogoutView.as_view(next_page='/'), name='logout'),
    # url(r'^login/$',
    #    django.contrib.auth.views.login,
    #    {
    #        'template_name': 'app/login.html',
    #        'authentication_form': app.forms.BootstrapAuthenticationForm,
    #        'extra_context':
    #        {
    #            'title': 'Log in',
    #            'year': datetime.now().year,
    #        }
    #    },
    #    name='login'),
    # url(r'^logout$',
    #    django.contrib.auth.views.logout,
    #    {
    #        'next_page': '/',
    #    },
    #    name='logout'),

    # Uncomment the admin/doc line below to enable admin documentation:
    # url(r'^admin/doc/', include('django.contrib.admindocs.urls')),

    # Uncomment the next line to enable the admin:
    url(r'^admin', admin.site.urls),

    # Required for FilteredSelectMultiple
    url(r'^jsi18n/$', JavaScriptCatalog.as_view(), name='javascript-catalog'),
]

if settings.DEBUG is True:
    urlpatterns += static(settings.MEDIA_URL,
                          document_root=settings.MEDIA_ROOT)
