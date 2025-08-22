class ChildComponent extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
    }

    connectedCallback() {
        this.shadowRoot.innerHTML = `
            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css">
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

        this.latitude = this.shadowRoot.querySelector('input[name="latitude"]');
        this.longitude = this.shadowRoot.querySelector('input[name="longitude"]');

        const methods = window.dynamicFields?.[this.getAttribute('parentId')];
        console.log(methods);

        [this.latitude, this.longitude].forEach(input =>
            input.addEventListener('input', () => {
                const object = {
                    latitude: this.latitude.value,
                    longitude: this.longitude.value,
                };
                methods.setValue(object);
                //this.dispatchEvent(new CustomEvent('value-changed', {
                //    detail: {
                //        latitude: this.latitude.value,
                //        longitude: this.longitude.value,
                //    },
                //    bubbles: true,
                //    composed: true,
                //}));
            }));

        //this.addEventListener('value-changed', (event) => {
        //    this.updateValue(event.detail);
        //});

        //this.dispatchEvent(new CustomEvent('initial-value', { bubbles: true, composed: true }));
        this.updateValue(methods.getValue());
    }

    updateValue(detail) {
        this.latitude.value = detail?.latitude;
        this.longitude.value = detail?.longitude;
    }
}

customElements.define('child-component', ChildComponent);