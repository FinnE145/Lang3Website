#from time import sleep
#from os import path
from subprocess import Popen, PIPE
#TODO: implement cformat (code sxhl)
#from iformat import cformat

binary = "/home/finn/flask1/CompiledSource/Lang3Source"

def runLang3(fp):
    p = Popen([binary, fp], stdout=PIPE, stderr=PIPE)
    (out, err) = p.communicate()
    return out.decode('ascii','ignore'), err.decode('ascii','ignore'), p.returncode
