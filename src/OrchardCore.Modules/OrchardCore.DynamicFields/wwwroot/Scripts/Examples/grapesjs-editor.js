class GrapesJsEditor extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        this.innerHTML = `
            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#editorModal">
                Open GrapesJS Editor
            </button>
            <div class="modal fade" id="editorModal" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable modal-fullscreen">
                    <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">GrapesJS Editor</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body p-0">
                        <div id="gjs">
                        <h1>Hello GrapesJS in Modal!</h1>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-success" id="saveBtn" data-bs-dismiss="modal">Save</button>
                    </div>
                    </div>
                </div>
            </div>
        `;
        this.init();
    }
    init() {
        const modalEl = document.getElementById('editorModal');
        modalEl.addEventListener('shown.bs.modal', () => {
            if (!this.editor) {
                this.editor = grapesjs.init({
                    container: '#gjs',
                    height: '100%',
                    fromElement: true,
                    storageManager: false,
                });
                if (this.objectToLoad) {
                    this.loadHtmlCss(this.objectToLoad);
                    this.objectToLoad = null;
                }
            }
        });
        const saveState = (object) => {
            this.onChange(object);
        };
        modalEl.querySelector('#saveBtn').addEventListener('click', (e) => {
            if (this.editor) {
                const html = this.editor.getHtml();
                const css = this.editor.getCss();
                const object = {
                    html,
                    css
                };

                saveState(object);
            }            
        });
    }
    loadHtmlCss(object) {
        if (this.editor) {
            const { html, css } = object;
            this.editor.setComponents(html);
            this.editor.setStyle(css);
        } else {
            this.objectToLoad = object;
        }
    }
    onChange(image) {
        console.log(image);
    }
}
customElements.define('grapesjs-editor', GrapesJsEditor);