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
            <div>
            <input name="latitude"></input>
            <input name="longitude"></input>
            </div>
          </div>`;

        this.latitude = this.shadowRoot.querySelector('input[name="latitude"]');
        this.longitude = this.shadowRoot.querySelector('input[name="longitude"]');

        [this.latitude, this.longitude].forEach(input =>
            input.addEventListener('input', () => {
                this.dispatchEvent(new CustomEvent('value-changed', {
                    detail: {
                        now: new Date().toJSON(),
                        latitude: this.latitude.value,
                        longitude: this.longitude.value,
                    },
                    bubbles: true,
                    composed: true,
                }));
            }));

        this.addEventListener('value-changed', (event) => {
            this.updateValue(event.detail);
        });
    }

    updateValue(detail) {
        this.latitude.value = detail.latitude;
        this.longitude.value = detail.longitude;
        this.shadowRoot.querySelector('#parentVal').textContent = JSON.stringify(detail);
    }
}

customElements.define('child-component', ChildComponent);