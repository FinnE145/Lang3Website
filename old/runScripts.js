const editorClear = document.getElementById('editorClear');
const editorElm = document.getElementById('editor');
const outputElm = document.getElementById('output');

async function codeResponse(elm) {
    try {
        response = await fetch('/run/getJob').then((response) => {
            try {
                response.json().then((json) => {
                    if (json.eCode >= 0) {
                        clearInterval(intervalId);
                    }
                    elm.innerHTML = json.body;
                });
            } catch (error) {
                clearInterval(intervalId);
                elm.innerHTML = "There was an error. Please try to run your code again, or <a href=\"/contact\">contact us</a> if the problem persists";
            }
        });
    } catch (error) {
        clearInterval(intervalId);
        elm.innerHTML = "There was an error. Please try to run your code again, or <a href=\"/contact\">contact us</a> if the problem persists";
    }
}

editorClear.addEventListener('click', () => {
    document.getElementById('editor').value = '';
});
var editor = ace.edit("editor");
editor.setTheme("ace/theme/idle_fingers");
editor.session.setMode("ace/mode/lang3");

intervalId = setInterval(codeResponse, 1000, outputElm);

setTimeout(codeResponse, 250, outputElm);
setTimeout(codeResponse, 500, outputElm);
setTimeout(codeResponse, 750, outputElm);
