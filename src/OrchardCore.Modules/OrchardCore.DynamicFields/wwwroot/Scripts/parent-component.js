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
        this.addEventListener('value-changed', (event) => {
            this.updateValue(event.detail);
        });
        this.addEventListener('initial-value', async (event) => {
            /* we could only notify event.target, but let's notify all children */
            //event.target.dispatchEvent(new CustomEvent('value-changed', {
            //    detail: JSON.parse(this.unescapeHTML(this.getAttribute('value'))),
            //    composed: true,
            //}));
            await this.notifyChildren(this.getAttribute('value'));
        });

        this.shadowRoot.querySelector('slot').addEventListener('slotchange', async (event) => {
            await this.notifyChildren(this.getAttribute('value'));
        });
    }

    async attributeChangedCallback(name, oldVal, newVal) {
        if (name === 'value' && oldVal !== newVal) {
            this._internals.setFormValue(newVal);
            await this.notifyChildren(newVal);
        }
    }

    async notifyChildren(str) {
        const slot = this.shadowRoot.querySelector('slot');
        const assignedElements = slot.assignedElements();
        const object = JSON.parse(this.unescapeHTML(str));

        for (const el of assignedElements) {
            if (el.tagName.includes('-')) {
                await customElements.whenDefined(el.tagName.toLowerCase());
            }

            if (el && typeof el.updateValue === 'function') {
                el.updateValue(object);
            }

            el.dispatchEvent(new CustomEvent('value-changed', {
                detail: object,
                composed: true,
            }));
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
        return this.getAttribute('value');
    }

    set value(newVal) {
        this.setAttribute('value', newVal);
    }

    updateValue(object) {
        this.setAttribute('value', JSON.stringify(object));
    }
}

customElements.define('parent-component', ParentComponent);