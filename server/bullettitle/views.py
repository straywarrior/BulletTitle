from django.shortcuts import render
from django.shortcuts import render_to_response
from django.http import Http404
from django.http import HttpResponseRedirect
from django.template import RequestContext
from django.http import HttpResponse
import django

def index(request):
    return render_to_response('bt_send.html',{}, context_instance=RequestContext(request))
    
def contact(request):
    return render_to_response('contact.html',{}, context_instance=RequestContext(request))
    
def about(request):
    return render_to_response('about.html',{}, context_instance=RequestContext(request))
