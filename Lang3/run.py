from subprocess import Popen, PIPE
from random import choice
from tempfile import NamedTemporaryFile
from datetime import datetime
import sys

# Hard-coding is bad, but will do for now
binary = "/home/finn/Lang3/bin/Debug/net8.0/Lang3"
# Where to store test files
source_dir = "/home/finn/Lang3/uploads/"

try:
    
    # Create a file with a guarenteed unique file name, including a useful timestamp
    timestamp = datetime.now().strftime("%Y%m%d-%H%M%S")
    source_file = NamedTemporaryFile(prefix="source-code-test-" + timestamp + ".",
                                     suffix=".l3",dir=source_dir,mode="w",
                                     delete=False)

    # Write some code into the file
    source_content = """
a = 6 
print(r)
    """
    print(source_content, file=source_file)
    source_file.close()

    # The input file name (decided on by NamedTemporaryFile), to be passed
    # to Lang3 program
    arg1 = source_file.name

    print("=== Running Lang3 with input file: " + source_file.name )

    # Start an external program ( "PIPE" means we want to get the result
    # back into python, not printed on the console)
    p = Popen([binary, arg1], stdout=PIPE, stderr=PIPE)

    # Wait until the external program terminates, and collect the results
    # (i.e. whatever the program printed) into python variables
    (out, err) = p.communicate()

    if p.returncode == 0:
        print("=== Lang3 Succeeded.")
    else:
        print("=== Lang3 failed, returned error code = ", p.returncode)

    if out:
        print("==== Additional Output from Lang3 ====")
        out = out.decode('ascii','ignore') # convert from bytes to python unicode-string
        print(out)
        print("==== END of Output from Lang3 ====")
    
    if err:
        print("==== Additional Errors from Lang3 ====")
        err = err.decode('ascii','ignore') # convert from bytes to python unicode-string
        print(err)
        print("==== END of Errors from Lang3 ====")


except OSError as e:
    sys.exit("failed to execute program: " + binary + " (details: " + str(e) + ")")
