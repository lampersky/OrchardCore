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
            this.setAttribute('value', JSON.stringify(event.detail));
        });
        this.addEventListener('initial-value', (event) => {
            event.target.dispatchEvent(new CustomEvent('value-changed', {
                detail: JSON.parse(this.unescapeHTML(this.getAttribute('value'))),
                composed: true,
            }));
        });

        this.shadowRoot.querySelector('slot').addEventListener('slotchange', (event) => {
            requestAnimationFrame(() => {
                this.notifyChildren(this.getAttribute('value'));
            });
        });
    }

    attributeChangedCallback(name, oldVal, newVal) {
        if (name === 'value' && oldVal !== newVal) {
            this._internals.setFormValue(newVal);
            requestAnimationFrame(() => {
                this.notifyChildren(newVal);
            });
        }
    }

    notifyChildren(str) {
        const slot = this.shadowRoot.querySelector('slot');
        const assignedElements = slot.assignedElements();
        assignedElements.forEach(el => {
            el.dispatchEvent(new CustomEvent('value-changed', {
                detail: JSON.parse(this.unescapeHTML(str)),
                composed: true,
            }));
        });
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
}

customElements.define('parent-component', ParentComponent);