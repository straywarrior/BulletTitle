from django.shortcuts import render
from django.shortcuts import render_to_response
from django.http import Http404
from django.http import HttpResponse
from django.http import HttpResponseRedirect
from django.template import RequestContext
from btuser.models import BulletItem
from django.views.decorators.csrf import csrf_exempt
import django
import json
from time import mktime
import sys
reload(sys)
sys.setdefaultencoding('utf-8')

@csrf_exempt
def getlist(request):
    all_items = BulletItem.objects.all()
    total_row = len(all_items)
    response = HttpResponse()
    response.write('<?xml version="1.0" encoding="UTF-8"?><bullets>')
    response.write('<info><total_row>'+str(total_row)+'</total_row></info>')
    for i in range(0, total_row):
        response.write('<row><text>'+str(all_items[i].bullet_text)+'</text><time>'+str(all_items[i].bullet_time)+'</time><pos>'+str(all_items[i].bullet_pos)+'</pos></row>')
    response.write('</bullets>')
    return response
