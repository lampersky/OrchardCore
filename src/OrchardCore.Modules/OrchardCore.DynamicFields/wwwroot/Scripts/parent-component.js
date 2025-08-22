class ParentComponent extends HTMLElement {
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
        //this.addEventListener('value-changed', (event) => {
        //    this.updateValue(event.detail);
        //});
        //this.addEventListener('initial-value', async (event) => {
        //    /* we could only notify event.target, but let's notify all children */
        //    //event.target.dispatchEvent(new CustomEvent('value-changed', {
        //    //    detail: JSON.parse(this.unescapeHTML(this.getAttribute('value'))),
        //    //    composed: true,
        //    //}));
        //    await this.notifyChildren(this.value/*this.getAttribute('value')*/);
        //});
        console.log('attr id:', this.getAttribute('id'));

        this.shadowRoot.querySelector('slot').addEventListener('slotchange', async (event) => {
            await this.notifyChildren(this.value/*this.getAttribute('value')*/);
        });
    }

    async attributeChangedCallback(name, oldVal, newVal) {
        if (name === 'value' && oldVal !== newVal) {
            this._internals.setFormValue(newVal);
            await this.notifyChildren(JSON.parse(this.unescapeHTML(newVal)));
        }
    }

    async notifyChildren(object) {
        const slot = this.shadowRoot.querySelector('slot');
        const assignedElements = slot.assignedElements();

        for (const el of assignedElements) {
            el.setAttribute('parentId', this.getAttribute('id'));

            if (el.tagName.includes('-')) {
                await customElements.whenDefined(el.tagName.toLowerCase());
            }

            if (el && typeof el.updateValue === 'function') {
                el.updateValue(object);
            }

            //el.dispatchEvent(new CustomEvent('value-changed', {
            //    detail: object,
            //    composed: true,
            //}));
        }
    }

    //unescapeHTML(str) {
    //    const temp = document.createElement("textarea");
    //    temp.innerHTML = str;
    //    return temp.value;
    //}

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

customElements.define('parent-component', ParentComponent);

function init(id) {
    console.log(id);
    window.dynamicFields = window.dynamicFields ?? {};
    window.dynamicFields[id] = {
        getValue: function () {
            return document.getElementById(id).value;
        },
        setValue: function (newValue) {
            document.getElementById(id).value = newValue;
        },
        querySelector: function (selector) {
            return document.getElementById(id).querySelector(selector);
        },
        querySelectorAll: function (selector) {
            return document.getElementById(id).querySelectorAll(selector);
        },
        closest: function (selector) {
            return document.getElementById(id).closest(selector);
        },
    };
}