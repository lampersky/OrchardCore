function clearQueryParams() {
    const url = new URL(window.location);
    ['contentType', 'contentField'].forEach(p => url.searchParams.delete(p));
    history.replaceState(null, '', url);
}

function toggleEditor(input) {
    input.closest('.resource').querySelector('textarea').classList.toggle('single-line-textarea');
}

function preventEnter(textarea, event) {
    if (textarea.classList.contains('single-line-textarea') && event.key === 'Enter') {
        event.preventDefault();
    }
}

function onResourceTypeChange(input) {
    input.closest('.dropdown-menu').querySelectorAll('.only-for-script').forEach(element => {
        if (input.value == "Script") {
            element.classList.remove('disabled');
        } else {
            element.classList.add('disabled');
        }
    });
}

function reindex(resource, index) {
    const elements = resource.querySelectorAll('input, select, textarea, label');
    elements.forEach((element) => {
        // name -> /(?:\[(\d+)\])/
        // id   -> /(?:_(\d+)_)/
        if (element.hasAttribute('for')) {
            const newFor = element.getAttribute('for').replace(/(?:_(\d+)_)/, (match, offset) => `_${index}_`);
            element.setAttribute('for', newFor);
        } else {
            element.name = element.name.replace(/(?:\[(\d+)\])/, (match, offset) => `[${index}]`);
            element.id = element.id.replace(/(?:_(\d+)_)/, (match, offset) => `_${index}_`);
        }
    });
}

function move(button, dir) {
    const resourceToMove = button.closest('.resource');
    const itemsContainer = document.querySelector('div.resources');
    const children = Array.from(itemsContainer.children);

    const idx = children.indexOf(resourceToMove);

    if (dir == -1 && idx > 0) {
        itemsContainer.insertBefore(resourceToMove, children[idx + dir]);
    } else if (dir == 1 && idx < children.length - 1) {
        itemsContainer.insertBefore(children[idx + dir], resourceToMove);
    }

    //todo
    document.querySelectorAll('div.resources > .resource').forEach((resource, index) => {
        reindex(resource, index);
    });
}

function removeResource(button) {
    const resourceToRemove = button.closest('.resource');
    const resources = resourceToRemove.parentElement;
    resources.removeChild(resourceToRemove);
    for (let index = 0; index < resources.children.length; index++) {
        reindex(resources.children[index], index);
    }
}

function addResource() {
    const template = document.getElementById('resource-template');
    const cloned = template.content.cloneNode(true);

    const itemsContainer = document.querySelector('div.resources');
    const items = document.querySelectorAll('div.resources > .resource');

    items.forEach((resource, index) => {
        reindex(resource, index);
    });

    reindex(cloned, items.length);
    itemsContainer.appendChild(cloned);
}

document.addEventListener('DOMContentLoaded', () => clearQueryParams());