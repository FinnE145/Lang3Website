/* if (navigator.userAgent.indexOf("Win") != -1) {
  document.querySelector('.mac-linux').classList.add('div-collapsed');
} else {
    document.querySelector('.windows').classList.add('div-collapsed');
}

function toggleDivs(e) {
    console.log(this.classList)
    if (this.classList.contains('div-collapsed')) {
        document.querySelector('.mac-linux').classList.toggle('div-collapsed');
        document.querySelector('.windows').classList.toggle('div-collapsed');
    }
}

document.querySelectorAll('.os-div').forEach((div) => {
    div.addEventListener('click', toggleDivs);
}); */

class ClassWatcher {

    constructor(targetNode, classToWatch, classAddedCallback, classRemovedCallback) {
        this.targetNode = targetNode
        this.classToWatch = classToWatch
        this.classAddedCallback = classAddedCallback
        this.classRemovedCallback = classRemovedCallback
        this.observer = null
        this.lastClassState = targetNode.classList.contains(this.classToWatch)

        this.init()
    }

    init() {
        this.observer = new MutationObserver(this.mutationCallback)
        this.observe()
    }

    observe() {
        this.observer.observe(this.targetNode, { attributes: true })
    }

    disconnect() {
        this.observer.disconnect()
    }

    mutationCallback = mutationsList => {
        for(let mutation of mutationsList) {
            if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                let currentClassState = mutation.target.classList.contains(this.classToWatch)
                if(this.lastClassState !== currentClassState) {
                    this.lastClassState = currentClassState
                    if(currentClassState) {
                        this.classAddedCallback()
                    }
                    else {
                        this.classRemovedCallback()
                    }
                }
            }
        }
    }
}

const progress = document.getElementById('progress');
const menuHeader = document.getElementById('hiddenMenuHeader');
const menu = document.getElementById('hiddenMenu');
const menuBlur = document.getElementById('menuBlur');

progress.style.width = '100%';

document.addEventListener('DOMContentLoaded', () => {
    setTimeout(() => {
        progress.style.width = '0%';
        progress.style.transition = 'width 0.5s';
        progress.style.transitionTimingFunction = 'ease-in';
        progress.style.float = 'right';
    }, 1000);
});

const classWatcher = new ClassWatcher(menu, 'collapsing', () => {
    menuBlur.classList.toggle('blur');
    menuHeader.classList.toggle('blur');
});

const highlightLanguageMap = {
    lang3: 'lang3',
    text: 'plain_text',
    plaintext: 'plain_text',
    plain: 'plain_text',
    sh: 'sh',
    bash: 'sh',
    shell: 'sh',
    py: 'python',
    python: 'python',
    js: 'javascript',
    javascript: 'javascript',
    ts: 'typescript',
    typescript: 'typescript',
    json: 'json',
    html: 'html',
    css: 'css'
};

const docsHighlightThemeId = 'ace/theme/l3t';
let docsHighlightThemePromise = null;
const docsHighlightModePromises = {};

function deriveAceMode(codeElm) {
    const langClass = Array.from(codeElm.classList || []).find(cls => cls.startsWith('language-'));
    const langAttr = codeElm.getAttribute('data-lang') || codeElm.getAttribute('lang');
    const lang = (langClass ? langClass.replace('language-', '') : (langAttr || 'plain_text')).toLowerCase();
    return highlightLanguageMap[lang] || lang || 'plain_text';
}

function ensureDocsThemeLoaded() {
    if (docsHighlightThemePromise) {
        return docsHighlightThemePromise;
    }

    docsHighlightThemePromise = new Promise((resolve) => {
        if (typeof ace === 'undefined') {
            resolve(null);
            return;
        }

        try {
            const themeModule = ace.require(docsHighlightThemeId);
            if (themeModule) {
                resolve(themeModule);
                return;
            }
        } catch (err) {
            // ignored
        }

        ace.config.loadModule(['theme', docsHighlightThemeId], (themeModule) => resolve(themeModule));
    });

    return docsHighlightThemePromise;
}

function ensureModeLoaded(modeName) {
    const moduleId = `ace/mode/${modeName}`;
    if (docsHighlightModePromises[moduleId]) {
        return docsHighlightModePromises[moduleId];
    }

    docsHighlightModePromises[moduleId] = new Promise((resolve) => {
        if (typeof ace === 'undefined') {
            resolve(moduleId);
            return;
        }

        let resolved = false;
        try {
            const modeModule = ace.require(moduleId);
            if (modeModule) {
                resolved = true;
                resolve(moduleId);
            }
        } catch (err) {
            // ignored
        }

        if (!resolved) {
            ace.config.loadModule(['mode', moduleId], () => resolve(moduleId));
        }
    });

    return docsHighlightModePromises[moduleId];
}

function highlightBlock(codeElm, themeModule, staticHighlight) {
    if (!codeElm || codeElm.dataset.highlighted === 'true') {
        return;
    }

    const modeName = deriveAceMode(codeElm);

    ensureModeLoaded(modeName).then(() => {
        staticHighlight.highlight(codeElm, {
            mode: `ace/mode/${modeName}`,
            theme: themeModule || docsHighlightThemeId,
            startLineNumber: 1,
            showGutter: false,
            trim: true
        }, () => {
            codeElm.dataset.highlighted = 'true';
            const parentPre = codeElm.closest('pre');
            if (parentPre) {
                parentPre.classList.add('ace-highlighted-pre');
            }
        });
    });
}

function highlightDocsCode() {
    if (typeof ace === 'undefined' || !document) {
        return;
    }

    const staticHighlight = ace.require && ace.require('ace/ext/static_highlight');
    if (!staticHighlight || typeof staticHighlight.highlight !== 'function') {
        return;
    }

    ensureDocsThemeLoaded().then((themeModule) => {
        document.querySelectorAll('pre > code').forEach((codeElm) => {
            highlightBlock(codeElm, themeModule, staticHighlight);
        });
    });
}

if (document?.body?.dataset?.page === 'docs') {
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', highlightDocsCode);
    } else {
        highlightDocsCode();
    }
}