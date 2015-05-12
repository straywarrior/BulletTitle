#!/usr/bin/python
# -*- coding: utf-8 -*-

from django.conf.urls import *


urlpatterns = patterns(('btuser.views'),
    url(r'^index/$', 'index', name='index'),
)
