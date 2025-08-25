class GeoCoordinatesComponent extends HTMLElement {
    constructor() {
        super();
        /*this.attachShadow({ mode: 'open' });*/
    }

    connectedCallback() {
        this./*shadowRoot.*/innerHTML = `
            <div class="card mt-3">
              <div class="card-body">
                <div class="mb-3">
                  <label for="latitude" class="form-label">Latitude:</label>
                  <input type="text" class="form-control" id="latitude" name="latitude" placeholder="Enter latitude">
                </div>
                <div class="mb-3">
                  <label for="longitude" class="form-label">Longitude:</label>
                  <input type="text" class="form-control" id="longitude" name="longitude" placeholder="Enter longitude">
                </div>
              </div>
            </div>
        `;

        this.latitude = this./*shadowRoot.*/querySelector('input[name="latitude"]');
        this.longitude = this./*shadowRoot.*/querySelector('input[name="longitude"]');

        const { addEventListener, getValue, setValue } = window.dynamicFields?.[this.getAttribute('dynamic-field-parent-id')];

        addEventListener('value', (value) => {
            this.updateValue(value);
        }, { init : true });

        [this.latitude, this.longitude].forEach(input =>
            input.addEventListener('input', () => {
                const object = {
                    latitude: this.latitude.value,
                    longitude: this.longitude.value,
                };
                setValue(object);
            }));
        // The initial value is set via a listener, alternatively, you can do this:
        // this.updateValue(getValue());
    }

    updateValue(detail) {
        this.latitude.value = detail?.latitude;
        this.longitude.value = detail?.longitude;
    }
}
if (!window.customElements.get('geo-coordinates')) {
    window.GeoCoordinatesComponent = GeoCoordinatesComponent;
    window.customElements.define('geo-coordinates', GeoCoordinatesComponent);
}