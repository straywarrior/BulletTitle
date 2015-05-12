from django.db import models

# Create your models here.

class BulletItem(models.Model):
    """This model is for the constants at the critical point """
    bullet_text = models.CharField(max_length = 100)
    bullet_time = models.DateTimeField(auto_now=True, auto_now_add=True)
    bullet_ifpub = models.SmallIntegerField()
    bullet_pos = models.SmallIntegerField()
    
    def __unicode__(self):
        return u'%s' %(self.bullet_text)
