# admin.py (app)
# !/usr/bin/env python3
# coding=utf-8
# barrett.williamm@epa.gov

"""
Defines classes used to generate the 'Accounts' Django Admin portion of the website.

There should be an Admin class for each Model that can be modified by an admin user.

Available functions:
- None for this module -- TBD (would like added to manage in Django Admin)
"""

from django.contrib import admin
# User profile information, see https://simpleisbetterthancomplex.com/tutorial/2016/11/23/how-to-add-user-profile-to-django-admin.html
from django.contrib.auth.admin import UserAdmin
from django.contrib.auth.models import User
from app.models import *
from .models import Profile


class ProfileInline(admin.StackedInline):
    """Add class docstring"""

    model = Profile
    can_delete = False
    verbose_name_plural = 'Profile'
    fk_name = 'user'


class CustomUserAdmin(UserAdmin):
    """Add class docstring"""

    inlines = (ProfileInline, )
    list_display = ('username', 'email', 'first_name', 'last_name', 'is_staff', 'get_organization')
    list_select_related = ('profile', )

    def get_organization(self, instance):
        """Add method docstring"""
        return instance.profile.organization
    get_organization.short_description = 'Organization'

    def get_inline_instances(self, request, obj=None):
        """Add method docstring"""
        if not obj:
            return list()
        return super(CustomUserAdmin, self).get_inline_instances(request, obj)


admin.site.unregister(User)
admin.site.register(User, CustomUserAdmin)


# this
class NamedReactionAdmin(admin.ModelAdmin):
    """Add class docstring"""

    filter_horizontal = ('Reactants', 'ByProducts')


admin.site.register(NamedReaction, NamedReactionAdmin)
