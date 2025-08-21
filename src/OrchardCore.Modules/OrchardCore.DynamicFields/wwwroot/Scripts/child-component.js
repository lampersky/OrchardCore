class ChildComponent extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
    }

    connectedCallback() {
        this.shadowRoot.innerHTML = `
            <div>
              <label for="latitude">Latitude:</label>
              <input name="latitude" />

              <label for="longitude">Longitude:</label>
              <input name="longitude" />
            </div>
        `;

        this.latitude = this.shadowRoot.querySelector('input[name="latitude"]');
        this.longitude = this.shadowRoot.querySelector('input[name="longitude"]');

        [this.latitude, this.longitude].forEach(input =>
            input.addEventListener('input', () => {
                this.dispatchEvent(new CustomEvent('value-changed', {
                    detail: {
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

        this.dispatchEvent(new CustomEvent('initial-value', { bubbles: true, composed: true }));
    }

    updateValue(detail) {
        this.latitude.value = detail?.latitude;
        this.longitude.value = detail?.longitude;
    }
}

customElements.define('child-component', ChildComponent);