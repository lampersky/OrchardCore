class DynamicFields extends HTMLElement {
    static formAssociated = true;

    constructor() {
        super();
        this._internals = this.attachInternals();
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.innerHTML = `<slot></slot>`;
    }

    static get observedAttributes() {
        return ['value'];
    }

    connectedCallback() {
        this.shadowRoot.querySelector('slot').addEventListener('slotchange', async (event) => {
            const slot = this.shadowRoot.querySelector('slot');
            const assignedElements = slot.assignedElements();
            for (const el of assignedElements) {
                el.setAttribute('dynamic-field-parent-id', this.getAttribute('id'));
            }
        });

        this.methods = window.dynamicFields?.[this.getAttribute('id')]
        this.connectedCallbackFinished = true;
    }

    async attributeChangedCallback(name, oldVal, newVal) {
        if (name === 'value' && oldVal !== newVal) {
            this._internals.setFormValue(newVal);
            if (this.connectedCallbackFinished) {
                this.methods.notify();
            }
        }
    }

    unescapeHTML(str) {
        const parser = new DOMParser();
        const doc = parser.parseFromString(str, "text/html");
        return doc.documentElement.textContent;
    }

    get value() {
        return JSON.parse(this.getAttribute('value'));
    }

    set value(newValObject) {
        this.setAttribute('value', JSON.stringify(newValObject));
    }
}
if (!window.customElements.get('dynamic-fields')) {
    window.DynamicFields = DynamicFields;
    window.customElements.define('dynamic-fields', DynamicFields);
}

function init(id, pathBase, langDir) {
    window.dynamicFields = window.dynamicFields ?? {};
    window.dynamicFields[id] = (() => {
        let listeners = [];
        const getElement = () => document.getElementById(id);
        const getValue = () => getElement().value;
        const notify = () => {
            const newValue = getValue();
            listeners.forEach(l => l.callback(newValue));
        };
        const getPathBase = () => pathBase;
        const getLangDir = () => langDir;
        const getParentId = () => id;
        return {
            getElement,
            getValue,
            notify,
            getPathBase,
            getLangDir,
            getParentId,
            setValue: (newValue) => {
                if (newValue !== getElement().value) {
                    getElement().value = newValue;
                }
            },
            querySelector: (selector) => getElement().querySelector(selector),
            querySelectorAll: (selector) => getElement().querySelectorAll(selector),
            closest: (selector) => getElement().closest(selector),
            addEventListener: (type, callback, options) => {
                listeners.push({ type, callback });
                if (options?.init === true) {
                    callback(getValue());
                }
            },
            removeEventListener: (type, callback) => {
                listeners = listeners.filter(listener => !(listener.type === type && listener.callback === callback));
            }
        };
    })();
}
