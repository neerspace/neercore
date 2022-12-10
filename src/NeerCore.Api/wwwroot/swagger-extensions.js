runWhen(() => !!document.querySelector(".description"), main);


const arrowClosed = `<svg class="arrow" width="20" height="20" aria-hidden="true" focusable="false"><use href="#large-arrow-down" xlink:href="#large-arrow-down"></use></svg>`;
const arrowOpened = `<svg class="arrow" width="20" height="20" aria-hidden="true" focusable="false"><use href="#large-arrow-up" xlink:href="#large-arrow-up"></use></svg>`;
const buttonWithArrow = `<button aria-expanded="true" class="expand-operation" title="Collapse operation">${arrowClosed}</button>`;

function createTitle(text) {
    return `<span>${text}</span><small></small>`;
}


function main() {
    initDescription();
    initParamsHiding();
}


function initDescription() {
    const description = document.querySelector('.description');
    const headerTriggers = description.querySelectorAll('.renderedMarkdown div>h2');

    headerTriggers.forEach(trigger => {
        const block = trigger.nextElementSibling;
        block.classList.add('hidden');

        trigger.classList.add('opblock-tag');
        trigger.classList.add('no-desc');
        trigger.innerHTML = createTitle(trigger.innerHTML) + buttonWithArrow;

        trigger.onclick = e => {
            const block = trigger.nextElementSibling;
            if (block.classList.contains('hidden')) {
                trigger.innerHTML = createTitle(trigger.innerText) + arrowOpened;
                block.classList.remove('hidden');
            } else {
                trigger.innerHTML = createTitle(trigger.innerText) + arrowClosed;
                block.classList.add('hidden');
            }
        };
    });
}


function initParamsHiding() {
    const blockOpeningContainers = document.querySelectorAll('.opblock');

    blockOpeningContainers.forEach(container => {
        const trigger = container.querySelector('.opblock-summary-control');

        trigger.onclick = e => {
            runWhen(() => !!container.querySelector('.opblock-section-header'), () => {
                const parametersHeader = container.querySelector('.opblock-section-header');
                const parametersTable = container.querySelector('.parameters-container');

                const openCloseTrigger = container.querySelector('.opblock-section .opblock-section-header');
                openCloseTrigger.onclick = e => {
                    parametersTable.classList.toggle('hidden');
                };
            });
        }
    });
}


// It takes the document a sec to load the swagger stuff loop until we find it
function runWhen(condition, action) {
    const checkExist = setInterval(() => {
        if (condition()) {
            clearInterval(checkExist);
            action();
        }
    }, 100); // check every 100ms
}
