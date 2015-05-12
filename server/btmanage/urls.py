#!/usr/bin/python
# -*- coding: utf-8 -*-

from django.conf.urls import *


urlpatterns = patterns(('btmanage.views'),
    url(r'^getlist/$', 'getlist', name='getlist'),
)
