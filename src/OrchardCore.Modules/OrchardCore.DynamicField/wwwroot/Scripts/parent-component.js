class ParentComponent extends HTMLElement {
    static formAssociated = true;

    static get observedAttributes() {
        return ['value'];
    }

    constructor() {
        super();
        this._internals = this.attachInternals();
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.innerHTML = `<slot></slot>`;
    }

    connectedCallback() {

        this.addEventListener('value-changed', (event) => {
            this.value = JSON.stringify(event.detail);
        });

        this.shadowRoot.querySelector('slot').addEventListener('slotchange', (event) => {
            requestAnimationFrame(() => {
                this.notifyChildren(this.value);
            });
        });
    }

    attributeChangedCallback(name, oldVal, newVal) {
        if (name === 'value' && oldVal !== newVal) {
            this._internals.setFormValue(newVal);
            this.notifyChildren(newVal);
        }
    }

    notifyChildren(str) {
        const slot = this.shadowRoot.querySelector('slot');
        const assignedElements = slot.assignedElements();
        assignedElements.forEach(el => {
            el.dispatchEvent(new CustomEvent('value-changed', {
                detail: JSON.parse(str),
                bubbles: true,
                composed: true,
            }));
        });
    }

    get value() {
        return this.getAttribute('value');
    }

    set value(newVal) {
        this.setAttribute('value', newVal);
    }
}

customElements.define('parent-component', ParentComponent);