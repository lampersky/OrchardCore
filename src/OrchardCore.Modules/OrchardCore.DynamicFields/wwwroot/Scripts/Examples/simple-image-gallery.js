class SimpleImageGallery extends HTMLElement {
    constructor() {
        super();
        this.seed = 50;
    }
    connectedCallback() {
        this.innerHTML = `
            <img src="https://picsum.photos/200" class="selected-img img-fluid rounded" data-bs-toggle="modal" data-bs-target="#imageModal">
            <div class="modal fade" id="imageModal" tabindex="-1" aria-labelledby="imageModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="imageModalLabel">Choose Image</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="container">
                    <div class="row">
                        ${Array.from({ length: 9 }, (_, i) => `
                        <div class="col-4 p-2">
                            <img src="https://picsum.photos/id/${this.seed + i}/200" class="gallery-img img-fluid rounded" data-bs-dismiss="modal" />
                        </div>
                        `).join('')}
                    </div>
                    </div>
                </div>
                </div>
            </div>
            </div>
        `;
        const saveState = (src) => {
            this.onChange({ src });
        };
        this.querySelectorAll('.gallery-img').forEach(img => {
            img.addEventListener('click', (e) => saveState(e.target.src));
        });
    }
    setImage(imageUrl) {
        this.querySelector('.selected-img').src = imageUrl;
    }
    onChange(image) {
        console.log(image);
    }
}
customElements.define('simple-image-gallery', SimpleImageGallery);