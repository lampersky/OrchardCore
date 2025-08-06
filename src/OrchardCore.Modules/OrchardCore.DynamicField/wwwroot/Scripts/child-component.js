class ChildComponent extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
    }

    connectedCallback() {
        this.shadowRoot.innerHTML = `
          <div>
            <h4>Child Component</h4>
            <p>Parent value is: <span id="parentVal">Unknown</span></p>
            <input></input>
          </div>`;

        const input = this.shadowRoot.querySelector('input');
        input.addEventListener('input', () => {
            this.dispatchEvent(new CustomEvent('value-changed', {
                detail: {
                    now: new Date().toJSON(),
                    input: input.value
                },
                bubbles: true,
                composed: true,
            }));
        });

        this.addEventListener('value-changed', (event) => {
            this.updateValue(event.detail);
        });
    }

    updateValue(detail) {
        this.shadowRoot.querySelector('input').value = detail.input;
        this.shadowRoot.querySelector('#parentVal').textContent = JSON.stringify(detail);
    }
}

customElements.define('child-component', ChildComponent);