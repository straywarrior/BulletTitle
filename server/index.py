#-*- coding:utf-8 -*-
import os
import sys
from django.core.handlers.wsgi import WSGIHandler
from bae.core.wsgi import WSGIApplication


os.environ['DJANGO_SETTINGS_MODULE'] = 'bullettitle.settings'
path = os.path.dirname(os.path.abspath(__file__))
if path not in sys.path:
    sys.path.insert(1,path)

application = WSGIApplication(WSGIHandler())
