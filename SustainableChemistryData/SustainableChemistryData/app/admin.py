from django.contrib import admin
from app.models import *

class NamedReactionAdmin(admin.ModelAdmin):
    filter_horizontal = ('Reactants', 'ByProd')

admin.site.register(NamedReaction, NamedReactionAdmin)
