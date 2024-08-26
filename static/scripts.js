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