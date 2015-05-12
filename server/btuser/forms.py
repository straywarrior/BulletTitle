from django import forms

class BulletSend(forms.Form):
    bullet_text = forms.CharField(widget=forms.TextInput(attrs={'class':'form-control'}), max_length=100)  
