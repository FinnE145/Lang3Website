ALWAYS_REGENERATE_HTML = True

import os
import markdown
import datetime as dt
from rq import Queue
from redis import Redis
from iformat import iprint
from codeRunner import runLang3
from string import ascii_uppercase
from flask import Flask, render_template, abort, request, flash, url_for

#os.chdir("C:/Users/finne/OneDrive/Documents/0coding/Lang3/Lang3/Tools/Website")

errStr = "<span>{} Please try to run your code again, or <a href=\"/contact\">contact us</a> if the problem persists.</span>"

queueName = "RunCode"
jobQueue = Queue(queueName, connection=Redis(port=7777))

app = Flask(__name__)
app.secret_key = "idkSomeText4ndNumb3rs4nd$ym&0!$"
app.config["MAX_CONTENT_LENGTH"] =  16 * 1024 * 1024 # 16mb

downloadCount = 0

validDocs = [f.split(".")[0] for f in os.listdir("docs") if f.endswith(".md")]
docsPages = list(zip(validDocs, ["".join([doc[0].upper()] + [f" {l}" if l in ascii_uppercase else l for l in doc[1:]]) for doc in validDocs]))
#iprint(docsPages)

validFiles = [f for f in os.listdir("static/files") if os.path.isfile(f"static/files/{f}")]

with open("veryProfessionalDatastore.txt", "r") as f:
    try:
        downloadCount = int(f.read().strip())
    except ValueError:
        pass

@app.route("/")
def home():
    return render_template("index.html", docsPages = docsPages)

@app.route("/download")
def download():
    return render_template("download.html", docsPages = docsPages, downloadCount = downloadCount)

@app.route("/docs/<docPage>")
def docs(docPage):
    if docPage not in validDocs:
        return abort(404)

    if os.path.exists(f"templates/{docPage}.html") and not ALWAYS_REGENERATE_HTML:
        return render_template(f"{docPage}.html")
    elif os.path.exists(f"docs/{docPage}.md"):
        iprint(f"Generating new HTML file for docs/{docPage}.md")
        with open(f"templates/docs/{docPage}.html", "w") as htmlFile:
            with open(f"docs/{docPage}.md", "r") as mdFile:
                htmlFile.write("{% extends 'layout.html' %}\n{% block content %}\n")
                htmlFile.write(markdown.markdown(mdFile.read()))
                htmlFile.write("\n{% endblock %}")
        return render_template(f"docs/{docPage}.html", docsPages = docsPages)
    else:
        return abort(404)

@app.route("/run")
def run():
    return render_template("run.html", docsPages = docsPages, errStr = errStr.format("There was an error."))

@app.route("/run", methods=["POST"])
def runPost():
    code = request.form.get("code")
    print(code)
    file = request.files.get("file")
    codeSent = code is not None and code.strip() != ""
    print(f"codeSent: {codeSent}")
    fileSent = file is not None and file.filename != ""
    print(f"fileSent: {fileSent}")
    #print(file)
    if codeSent and fileSent:
        flash("Both code and a file were provided, so code was used", "warning")
    if codeSent:
        fileName = f"./{url_for('static', filename='files/userCode/')}{dt.datetime.now().isoformat().replace('-', '').replace(':', '').replace('.', '')}.l3"
        with open(fileName, "w") as f:
            f.write(code)
    elif fileSent:
        fileName = f"./{url_for('static', filename='files/userCode/')}{dt.datetime.now().isoformat().replace('-', '').replace(':', '').replace('.', '')}.l3"
        file.save(fileName)
        file.stream.seek(0)
        code = file.stream.read().decode("utf-8")
        file.stream.close()
    else:
        flash("No code or file provided", "danger")
        return render_template("run.html", docsPages = docsPages)
    job = jobQueue.enqueue_call(runLang3, [fileName])   # add timeout=30 (it doesnt seem to work), or ttl=30 to limit the time that the task lasts to 30 seconds
    return render_template("run.html", docsPages = docsPages, input = code, outputJobId = f"?jid={job.id}", errStr = errStr.format("There was an error."))

@app.route("/run/getJob")
def runJob():
    jobId = request.args.get("jid")
    if jobId is not None:
        job = jobQueue.fetch_job(jobId)
        if job is None:
            return {"eCode": 1, "body": errStr.format("Your job ID is invalid or has expired.")}
        jobStatus = job.get_status()
        print(jobStatus)
        print(job.result)
        if jobStatus == "finished":
            if not job.result[0] or job.result[1]:
                return {"eCode": 1, "body": errStr.format(f"Compilation failed with compiler return code {job.result[2]}\n{job.result[0]}\n{job.result[1]}\n")}
            return {"eCode": 0, "body": job.result[0]}
        elif jobStatus == "queued":
            return {"eCode": -1, "body": "Waiting to complile on our servers"}
        elif jobStatus == "started":
            return {"eCode": -2, "body": "Compiling code"}
        elif jobStatus == "failed":
            return {"eCode": 1, "body": errStr.format("Job failed with no further information.") if job.result is None else errStr.format(f"Job failed with compiler return code {job.result[2]}\n{job.result[0]}\n{job.result[1]}\n")}
        else:
            return {"eCode": 2, "body": errStr.format(f"Unexpected status of {jobStatus}")}
    else:
        return {"eCode": 0, "body": "Output will appear here"}

@app.route("/contact")
def contact():
    return render_template("contact.html", docsPages = docsPages)

@app.route("/files/<fileName>")
def file(fileName):
    if fileName not in validFiles:
        return abort(404)
    global downloadCount
    downloadCount += 1
    with open("veryProfessionalDatastore.txt", "w") as f:
        f.write(str(downloadCount))
    return f"<h1>{fileName} will be here</h1>"

if __name__ == "__main__":
    app.run(debug=True)
