class AnotherChildComponent extends HTMLElement {
    constructor() {
        super();
        /*this.attachShadow({ mode: 'open' });*/
    }

    connectedCallback() {
        this./*shadowRoot.*/innerHTML = `
          <div class="container mt-3">
            <div class="mb-3">
              <label for="latitude" class="form-label">Latitude:</label>
              <input type="text" class="form-control" id="latitude" name="latitude" placeholder="Enter latitude">
            </div>
            <div class="mb-3">
              <label for="longitude" class="form-label">Longitude:</label>
              <input type="text" class="form-control" id="longitude" name="longitude" placeholder="Enter longitude">
            </div>
          </div>
        `;

        this.latitude = this./*shadowRoot.*/querySelector('input[name="latitude"]');
        this.longitude = this./*shadowRoot.*/querySelector('input[name="longitude"]');

        const { addEventListener, getValue, setValue } = window.dynamicFields?.[this.getAttribute('dynamic-field-parent-id')];

        addEventListener('value', (value) => {
            this.updateValue(getValue());
        }, { init : true });

        [this.latitude, this.longitude].forEach(input =>
            input.addEventListener('input', () => {
                const object = {
                    latitude: this.latitude.value,
                    longitude: this.longitude.value,
                };
                setValue(object);
            }));
        // initial value is set via listener
    }

    updateValue(detail) {
        this.latitude.value = detail?.latitude;
        this.longitude.value = detail?.longitude;
    }
}

customElements.define('another-child-component', AnotherChildComponent);