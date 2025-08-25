class SimpleImageGallery extends HTMLElement {
    constructor() {
        super();
        this.seed = 50;
    }
    connectedCallback() {
        this.parentId = this.getAttribute('dynamic-field-parent-id');
        this.innerHTML = `
            <div class="position-relative d-inline-block" data-bs-toggle="modal" data-bs-target="#${this.parentId}_imageModal">
              <div class="d-flex justify-content-center">
                  <div class="spinner-border" role="status">
                      <span class="visually-hidden">Loading...</span>
                  </div>
              </div>
              <img src="" class="selected-img img-fluid rounded d-none"
                   onload="this.previousElementSibling.classList.add('d-none'); this.classList.remove('d-none');">
            </div>

            <div class="modal fade" id="${this.parentId}_imageModal" tabindex="-1" aria-labelledby="${this.parentId}_imageModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="${this.parentId}_imageModalLabel">Choose Image</h5>
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
        const selectedImageEl = this.querySelector('.selected-img');
        selectedImageEl.previousElementSibling.classList.remove('d-none');
        selectedImageEl.classList.add('d-none');
        selectedImageEl.src = `${imageUrl}?v=${new Date().toJSON()}`;
    }
    onChange(image) {
        console.log(image);
    }
}
customElements.define('simple-image-gallery', SimpleImageGallery);