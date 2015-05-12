#!/usr/bin/python
# -*- coding: utf-8 -*-

from django.conf.urls import patterns, include, url
from django.contrib import admin

admin.autodiscover()

urlpatterns = patterns('',
    url(r'^$', 'btuser.views.index', name='index'),
    url(r'^admin/', include(admin.site.urls)),
)

urlpatterns += patterns(('bullettitle.views'),
    url(r'^about/$','about', name = 'about'),
    url(r'^contact/$','contact', name = 'contact'),
)

urlpatterns += patterns((''),
    (r'^btuser/',include('btuser.urls')),
    (r'^manage/',include('btmanage.urls')),
)

urlpatterns += patterns((''),
    (r'^static/(?P<path>.*)$', 'django.views.static.serve',
        {'document_root':'./static'}
    ),
)
