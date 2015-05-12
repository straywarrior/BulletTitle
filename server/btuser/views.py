from django.shortcuts import render
from django.shortcuts import render_to_response
from django.http import Http404
from django.http import HttpResponse
from django.http import HttpResponseRedirect
from django.template import RequestContext
from btuser.forms import BulletSend
from btuser.models import BulletItem
from django.views.decorators.csrf import csrf_exempt
import django

# Create your views here.
@csrf_exempt
def index(request):
    if (request.method == "POST"):
        form_post = BulletSend(request.POST);
        if (form_post.is_valid()):
            form_cd = form_post.cleaned_data
            bullet_text = form_cd['bullet_text']
            bullet_item_new = BulletItem(bullet_text = bullet_text, bullet_ifpub = 0, bullet_pos = 0)
            bullet_item_new.save();
            return render_to_response('bt_send.html',
                {'form': form_post, 'status':"save successfully"}, context_instance=RequestContext(request))
        else:
            form = {'bullet_text':''}
            form = BulletSend(form)
            return render_to_response('bt_send.html',
                {'form': form}, context_instance=RequestContext(request))
    else:
        form = {'bullet_text':'Input here'}
        form = BulletSend(form)
        return render_to_response('bt_send.html',
            {'form': form}, context_instance=RequestContext(request))
