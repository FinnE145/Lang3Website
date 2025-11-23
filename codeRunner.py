#from time import sleep
import os
from subprocess import Popen, PIPE
#TODO: implement cformat (code sxhl)
#from iformat import cformat

binary = "/CompiledSource/Lang3"

def runLang3(fp):
    p = Popen([binary, fp, "!d"], stdout=PIPE, stderr=PIPE)
    (out, err) = p.communicate()
    return out.decode('ascii','ignore'), err.decode('ascii','ignore'), p.returncode
