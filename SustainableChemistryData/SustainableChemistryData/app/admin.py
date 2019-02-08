from django.contrib import admin
from app.models import *


# User profile information, see https://simpleisbetterthancomplex.com/tutorial/2016/11/23/how-to-add-user-profile-to-django-admin.html
from django.contrib.auth.admin import UserAdmin
from django.contrib.auth.models import User

from .models import Profile

class ProfileInline(admin.StackedInline):
    model = Profile
    can_delete = False
    verbose_name_plural = 'Profile'
    fk_name = 'user'

class CustomUserAdmin(UserAdmin):
    inlines = (ProfileInline, )
    list_display = ('username', 'email', 'first_name', 'last_name', 'is_staff', 'get_organization')
    list_select_related = ('profile', )

    def get_organization(self, instance):
        return instance.profile.organization
    get_organization.short_description = 'Organization'

    def get_inline_instances(self, request, obj=None):
        if not obj:
            return list()
        return super(CustomUserAdmin, self).get_inline_instances(request, obj)


admin.site.unregister(User)
admin.site.register(User, CustomUserAdmin)


# this 
class NamedReactionAdmin(admin.ModelAdmin):
    filter_horizontal = ('Reactants', 'ByProducts')


admin.site.register(NamedReaction, NamedReactionAdmin)
