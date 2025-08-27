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

function getLastIndex() {
    return Array.from(document.querySelectorAll('div.resources > .resource input')).reduce((max, input) => {
        const matches = input.id?.match(/(?:_(\d+)_)/);
        if (matches) {
            const found = parseInt(matches[1], 10);
            return Math.max(max, found);
        }
        return max;
    }, 0);
}

function reindex(resource, index) {
    const elements = resource.querySelectorAll('input, select, textarea, label');
    elements.forEach((element) => {
        if (element.hasAttribute('for')) {
            const newFor = element.getAttribute('for').replace(/(?:_(\d+)_)/, (match, offset) => `_${index}_`);
            element.setAttribute('for', newFor);
        } else {
            element.name = element.name.replace(/(?:\[(\d+)\])/, (match, offset) => `[${index}]`);
            element.id = element.id.replace(/(?:_(\d+)_)/, (match, offset) => `_${index}_`);
        }
    });
}

function reindexAll() {
    const resourcesContainer = document.querySelector('div.resources');
    const resources = document.querySelectorAll('div.resources > .resource');
    resources.forEach((resource, index) => {
        // detach element, to avoid trigerring radio button states, when there is name collision
        resource.remove();
        // change name, id, for
        reindex(resource, index);
    });
    resources.forEach((resource) => {
        // attach elements back
        resourcesContainer.appendChild(resource);
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

    reindexAll();
}

function removeResource(button) {
    const resourceToRemove = button.closest('.resource');
    const resources = resourceToRemove.parentElement;
    resources.removeChild(resourceToRemove);

    reindexAll();
}

function addResource() {
    const template = document.getElementById('resource-template');
    const cloned = template.content.cloneNode(true);
    const itemsContainer = document.querySelector('div.resources');

    reindex(cloned, getLastIndex() + 1);

    itemsContainer.appendChild(cloned);

    reindexAll();
}

document.addEventListener('DOMContentLoaded', () => clearQueryParams());