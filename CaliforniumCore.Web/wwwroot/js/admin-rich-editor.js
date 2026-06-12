(function () {
    "use strict";

    const defaultUploadUrl = "/Admin/Media/EditorUpload";

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll("textarea[data-rich-editor='true']").forEach(initRichEditor);
        initImageUrlUploads();
    });

    function initRichEditor(textarea) {
        if (textarea.dataset.richEditorInitialized === "true") {
            return;
        }

        textarea.dataset.richEditorInitialized = "true";

        const wrapper = document.createElement("div");
        wrapper.className = "admin-rich-editor";

        const toolbar = buildToolbar();
        const surface = document.createElement("div");
        surface.className = "admin-rich-editor-surface content-html";
        surface.contentEditable = "true";
        surface.setAttribute("role", "textbox");
        surface.setAttribute("aria-multiline", "true");
        surface.innerHTML = textarea.value && textarea.value.trim().length > 0
            ? textarea.value
            : "<p><br></p>";

        const source = document.createElement("textarea");
        source.className = "admin-rich-editor-code";
        source.spellcheck = false;
        source.value = textarea.value || "";

        const status = document.createElement("div");
        status.className = "admin-rich-editor-status";
        status.setAttribute("aria-live", "polite");

        textarea.classList.add("admin-rich-editor-hidden-source");
        textarea.parentNode.insertBefore(wrapper, textarea);
        wrapper.appendChild(toolbar);
        wrapper.appendChild(surface);
        wrapper.appendChild(source);
        wrapper.appendChild(status);

        let savedRange = null;
        let sourceMode = false;
        let activeImage = null;

        const selectImage = function (image) {
            surface.querySelectorAll(".admin-rich-editor-selected-image").forEach(function (item) {
                item.classList.remove("admin-rich-editor-selected-image");
            });

            activeImage = image || null;
            if (activeImage) {
                activeImage.classList.add("admin-rich-editor-selected-image");
            }
        };

        const saveSelection = function () {
            const selection = window.getSelection();
            if (!selection || selection.rangeCount === 0) {
                return;
            }

            const range = selection.getRangeAt(0);
            if (surface.contains(range.commonAncestorContainer)) {
                savedRange = range.cloneRange();
            }
        };

        const restoreSelection = function () {
            surface.focus();
            const selection = window.getSelection();
            if (!selection) {
                return;
            }

            selection.removeAllRanges();
            if (savedRange) {
                selection.addRange(savedRange);
            }
        };

        const syncToTextarea = function () {
            textarea.value = sourceMode ? source.value : getCleanEditorHtml(surface);
        };

        const setStatus = function (message, kind) {
            status.textContent = message || "";
            status.classList.toggle("is-error", kind === "error");
            status.classList.toggle("is-success", kind === "success");
        };

        const runCommand = function (command, value) {
            if (sourceMode) {
                return;
            }

            restoreSelection();
            document.execCommand("styleWithCSS", false, true);
            document.execCommand(command, false, value || null);
            syncToTextarea();
            saveSelection();
        };

        const applyInlineStyle = function (property, value) {
            if (sourceMode || !value) {
                return;
            }

            restoreSelection();
            const selection = window.getSelection();
            if (!selection || selection.rangeCount === 0) {
                return;
            }

            const range = selection.getRangeAt(0);
            const span = document.createElement("span");
            span.style[property] = value;

            if (range.collapsed) {
                span.appendChild(document.createTextNode("\u200b"));
                range.insertNode(span);
                range.setStart(span.firstChild, 1);
                range.setEnd(span.firstChild, 1);
            } else {
                span.appendChild(range.extractContents());
                range.insertNode(span);
                range.selectNodeContents(span);
            }

            selection.removeAllRanges();
            selection.addRange(range);
            syncToTextarea();
            saveSelection();
        };

        const toggleSourceMode = function (button) {
            if (!sourceMode) {
                source.value = getCleanEditorHtml(surface);
                textarea.value = source.value;
                sourceMode = true;
                wrapper.classList.add("is-source-mode");
                button.classList.add("is-active");
                source.focus();
                return;
            }

            surface.innerHTML = source.value && source.value.trim().length > 0
                ? source.value
                : "<p><br></p>";
            sourceMode = false;
            wrapper.classList.remove("is-source-mode");
            button.classList.remove("is-active");
            syncToTextarea();
            surface.focus();
        };

        toolbar.addEventListener("mousedown", function (event) {
            if (event.target.closest("button")) {
                event.preventDefault();
            }
        });

        toolbar.addEventListener("click", async function (event) {
            const button = event.target.closest("button");
            if (!button) {
                return;
            }

            if (button.dataset.command) {
                runCommand(button.dataset.command, button.dataset.value);
                return;
            }

            if (button.dataset.action === "link") {
                const current = prompt("Enter link URL:", "https://");
                if (current && current.trim().length > 0) {
                    runCommand("createLink", current.trim());
                }
                return;
            }

            if (button.dataset.action === "image") {
                await uploadAndInsertImage(textarea, surface, setStatus, restoreSelection, syncToTextarea, saveSelection);
                return;
            }

            if (button.dataset.action === "imageOptions") {
                await editSelectedImage(surface, activeImage, selectImage, setStatus, syncToTextarea);
                return;
            }

            if (button.dataset.action === "source") {
                toggleSourceMode(button);
            }
        });

        toolbar.addEventListener("change", function (event) {
            const target = event.target;

            if (target.matches("[data-block-format]")) {
                runCommand("formatBlock", target.value);
                target.value = "";
                return;
            }

            if (target.matches("[data-font-size]")) {
                applyInlineStyle("fontSize", target.value);
                target.value = "";
                return;
            }

            if (target.matches("[data-font-family]")) {
                applyInlineStyle("fontFamily", target.value);
                target.value = "";
                return;
            }

            if (target.matches("[data-text-color]")) {
                applyInlineStyle("color", target.value);
                return;
            }

            if (target.matches("[data-highlight-color]")) {
                applyInlineStyle("backgroundColor", target.value);
            }
        });

        surface.addEventListener("input", function () {
            syncToTextarea();
            saveSelection();
        });
        surface.addEventListener("click", function (event) {
            const image = event.target.closest("img");
            selectImage(image && surface.contains(image) ? image : null);
        });
        surface.addEventListener("keyup", saveSelection);
        surface.addEventListener("mouseup", saveSelection);
        surface.addEventListener("focus", saveSelection);

        source.addEventListener("input", syncToTextarea);

        const form = textarea.closest("form");
        if (form) {
            form.addEventListener("submit", function () {
                syncToTextarea();
            });
        }
    }

    function buildToolbar() {
        const toolbar = document.createElement("div");
        toolbar.className = "admin-rich-editor-toolbar";
        toolbar.setAttribute("role", "toolbar");
        toolbar.innerHTML = [
            '<select class="form-select form-select-sm" data-block-format title="Block format" aria-label="Block format">',
            '<option value="">Paragraph / heading</option>',
            '<option value="p">Paragraph</option>',
            '<option value="h2">Heading 2</option>',
            '<option value="h3">Heading 3</option>',
            '<option value="h4">Heading 4</option>',
            '<option value="blockquote">Quote</option>',
            '</select>',
            '<select class="form-select form-select-sm" data-font-size title="Font size" aria-label="Font size">',
            '<option value="">Font size</option>',
            '<option value="0.875rem">Small</option>',
            '<option value="1rem">Normal</option>',
            '<option value="1.25rem">Large</option>',
            '<option value="1.5rem">X Large</option>',
            '<option value="2rem">Display</option>',
            '</select>',
            '<select class="form-select form-select-sm" data-font-family title="Font family" aria-label="Font family">',
            '<option value="">Font family</option>',
            '<option value="Arial, Helvetica, sans-serif">Arial</option>',
            '<option value="Georgia, serif">Georgia</option>',
            '<option value="Verdana, Geneva, sans-serif">Verdana</option>',
            '<option value="ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace">Monospace</option>',
            '</select>',
            button("bold", "Bold", "bi-type-bold"),
            button("italic", "Italic", "bi-type-italic"),
            button("underline", "Underline", "bi-type-underline"),
            button("strikeThrough", "Strikethrough", "bi-type-strikethrough"),
            button("superscript", "Superscript", "bi-superscript"),
            button("subscript", "Subscript", "bi-subscript"),
            '<input type="color" class="form-control form-control-color" data-text-color value="#111827" title="Text color" aria-label="Text color" />',
            '<input type="color" class="form-control form-control-color" data-highlight-color value="#fff3bf" title="Highlight color" aria-label="Highlight color" />',
            button("justifyLeft", "Align left", "bi-text-left"),
            button("justifyCenter", "Align center", "bi-text-center"),
            button("justifyRight", "Align right", "bi-text-right"),
            button("justifyFull", "Justify", "bi-justify"),
            button("insertUnorderedList", "Bulleted list", "bi-list-ul"),
            button("insertOrderedList", "Numbered list", "bi-list-ol"),
            button("outdent", "Outdent", "bi-text-indent-left"),
            button("indent", "Indent", "bi-text-indent-right"),
            actionButton("link", "Add link", "bi-link-45deg"),
            button("unlink", "Remove link", "bi-link"),
            actionButton("image", "Upload image", "bi-image"),
            actionButton("imageOptions", "Image options", "bi-aspect-ratio"),
            button("insertHorizontalRule", "Horizontal line", "bi-dash-lg"),
            button("removeFormat", "Clear formatting", "bi-eraser"),
            button("undo", "Undo", "bi-arrow-counterclockwise"),
            button("redo", "Redo", "bi-arrow-clockwise"),
            actionButton("source", "Edit HTML source", "bi-code-slash")
        ].join("");

        return toolbar;
    }

    function button(command, title, icon) {
        return '<button type="button" class="btn btn-sm btn-outline-secondary admin-rich-editor-button" data-command="' +
            command + '" title="' + title + '" aria-label="' + title + '"><i class="bi ' + icon + '"></i></button>';
    }

    function actionButton(action, title, icon) {
        return '<button type="button" class="btn btn-sm btn-outline-secondary admin-rich-editor-button" data-action="' +
            action + '" title="' + title + '" aria-label="' + title + '"><i class="bi ' + icon + '"></i></button>';
    }

    async function uploadAndInsertImage(textarea, surface, setStatus, restoreSelection, syncToTextarea, saveSelection) {
        const file = await chooseImageFile();
        if (!file) {
            return;
        }

        setStatus("Uploading image...", null);

        try {
            const result = await uploadImage(
                file,
                textarea.dataset.editorUploadUrl || defaultUploadUrl,
                textarea.dataset.mediaGroup || "Editor",
                textarea.closest("form")
            );

            const options = await openImageOptionsDialog({
                src: result.url,
                altText: result.altText || "",
                caption: "",
                width: "100%",
                layout: "center",
                shape: "rounded"
            }, "Insert image");

            if (!options) {
                setStatus("Image uploaded to Media Library but not inserted.", "success");
                return;
            }

            restoreSelection();
            surface.appendChild(document.createTextNode(" "));
            document.execCommand("insertHTML", false, buildImageHtml(result.url, options));
            syncToTextarea();
            saveSelection();
            setStatus("Image uploaded and inserted.", "success");
        } catch (error) {
            setStatus(error.message || "Image upload failed.", "error");
        }
    }

    async function editSelectedImage(surface, activeImage, selectImage, setStatus, syncToTextarea) {
        const image = activeImage && surface.contains(activeImage)
            ? activeImage
            : getImageFromSelection(surface);

        if (!image) {
            setStatus("Select an image in the editor first.", "error");
            return;
        }

        const options = await openImageOptionsDialog(readImageOptions(image), "Update image");
        if (!options) {
            return;
        }

        const node = getImageContainer(image, surface);
        node.outerHTML = buildImageHtml(image.getAttribute("src") || image.src, options);
        selectImage(null);
        syncToTextarea();
        setStatus("Image options updated.", "success");
    }

    function getImageFromSelection(surface) {
        const selection = window.getSelection();
        if (!selection || selection.rangeCount === 0) {
            return null;
        }

        const node = selection.anchorNode;
        const element = node && node.nodeType === Node.ELEMENT_NODE ? node : node?.parentElement;
        const image = element?.closest("img");
        return image && surface.contains(image) ? image : null;
    }

    function getImageContainer(image, surface) {
        const figure = image.closest("figure");
        return figure && surface.contains(figure) ? figure : image;
    }

    function buildImageHtml(src, options) {
        const width = options.width || "100%";
        const layout = options.layout || "center";
        const shape = options.shape || "rounded";
        const altText = escapeAttribute(options.altText || "");
        const caption = (options.caption || "").trim();
        const imageClass = shape === "circle"
            ? "img-fluid rounded-circle"
            : (shape === "rounded" ? "img-fluid rounded" : "img-fluid");
        const imageStyle = buildImageStyle(width, layout);
        const imageHtml = '<img src="' + escapeAttribute(src) + '" alt="' + altText + '" class="' + imageClass + '" loading="lazy" style="' + imageStyle + '" />';

        if (layout === "inline") {
            return imageHtml;
        }

        const figureStyle = buildFigureStyle(width, layout);
        const figureClass = "admin-content-image" + (layout === "center" ? " text-center" : "");
        const captionHtml = caption.length > 0
            ? '<figcaption>' + escapeHtml(caption) + '</figcaption>'
            : "";

        return '<figure class="' + figureClass + '" style="' + figureStyle + '">' + imageHtml + captionHtml + '</figure>';
    }

    function buildImageStyle(width, layout) {
        const parts = ["max-width:100%", "height:auto"];

        if (layout === "inline") {
            parts.push("display:inline-block", "vertical-align:middle", "margin:0 .5rem");
            if (width !== "auto") {
                parts.push("width:" + width);
            }
            return parts.join(";") + ";";
        }

        if (width !== "auto") {
            parts.push("width:100%");
        }

        return parts.join(";") + ";";
    }

    function buildFigureStyle(width, layout) {
        const parts = ["max-width:100%"];

        if (width !== "auto") {
            parts.push("width:" + width);
        }

        if (layout === "wrap-left") {
            parts.push("float:left", "margin:0 1rem 1rem 0", "text-align:left");
        } else if (layout === "wrap-right") {
            parts.push("float:right", "margin:0 0 1rem 1rem", "text-align:right");
        } else if (layout === "left") {
            parts.push("margin:1rem auto 1rem 0", "text-align:left");
        } else if (layout === "right") {
            parts.push("margin:1rem 0 1rem auto", "text-align:right");
        } else {
            parts.push("margin:1rem auto", "text-align:center");
        }

        return parts.join(";") + ";";
    }

    function readImageOptions(image) {
        const figure = image.closest("figure");
        const caption = figure?.querySelector("figcaption")?.textContent || "";
        const width = (figure?.style.width || image.style.width || "100%").trim() || "100%";
        let layout = "center";

        if (figure?.style.float === "left") {
            layout = "wrap-left";
        } else if (figure?.style.float === "right") {
            layout = "wrap-right";
        } else if (image.style.display === "inline-block" && !figure) {
            layout = "inline";
        } else if (figure?.style.textAlign === "left") {
            layout = "left";
        } else if (figure?.style.textAlign === "right") {
            layout = "right";
        }

        let shape = "square";
        if (image.classList.contains("rounded-circle")) {
            shape = "circle";
        } else if (image.classList.contains("rounded")) {
            shape = "rounded";
        }

        return {
            src: image.getAttribute("src") || image.src,
            altText: image.getAttribute("alt") || "",
            caption: caption,
            width: normalizeWidthOption(width),
            layout: layout,
            shape: shape
        };
    }

    function normalizeWidthOption(width) {
        const allowed = ["auto", "25%", "33.333%", "50%", "75%", "100%"];
        return allowed.includes(width) ? width : "100%";
    }

    function openImageOptionsDialog(initial, title) {
        return new Promise(function (resolve) {
            const dialog = getImageOptionsDialog();
            const element = dialog.element;
            const modal = dialog.modal;
            const form = element.querySelector("form");
            const saveButton = element.querySelector("[data-image-options-save]");
            let settled = false;

            element.querySelector(".modal-title").textContent = title || "Image options";
            form.elements.altText.value = initial.altText || "";
            form.elements.caption.value = initial.caption || "";
            form.elements.width.value = initial.width || "100%";
            form.elements.layout.value = initial.layout || "center";
            form.elements.shape.value = initial.shape || "rounded";

            form.onsubmit = function (event) {
                event.preventDefault();
                saveButton.click();
            };

            saveButton.onclick = function () {
                settled = true;
                const options = {
                    altText: form.elements.altText.value.trim(),
                    caption: form.elements.caption.value.trim(),
                    width: form.elements.width.value,
                    layout: form.elements.layout.value,
                    shape: form.elements.shape.value
                };
                modal.hide();
                resolve(options);
            };

            element.addEventListener("hidden.bs.modal", function onHidden() {
                if (!settled) {
                    resolve(null);
                }
            }, { once: true });

            modal.show();
        });
    }

    function getImageOptionsDialog() {
        let element = document.getElementById("adminImageOptionsModal");
        if (!element) {
            element = document.createElement("div");
            element.id = "adminImageOptionsModal";
            element.className = "modal fade";
            element.tabIndex = -1;
            element.setAttribute("aria-hidden", "true");
            element.innerHTML = [
                '<div class="modal-dialog modal-dialog-centered">',
                '<div class="modal-content">',
                '<div class="modal-header">',
                '<h5 class="modal-title">Image options</h5>',
                '<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>',
                '</div>',
                '<div class="modal-body">',
                '<form>',
                '<div class="mb-3">',
                '<label class="form-label">Alt text</label>',
                '<input name="altText" type="text" class="form-control" />',
                '</div>',
                '<div class="mb-3">',
                '<label class="form-label">Caption</label>',
                '<input name="caption" type="text" class="form-control" />',
                '</div>',
                '<div class="row g-3">',
                '<div class="col-sm-6">',
                '<label class="form-label">Size</label>',
                '<select name="width" class="form-select">',
                '<option value="auto">Natural</option>',
                '<option value="25%">Small</option>',
                '<option value="33.333%">One third</option>',
                '<option value="50%">Half</option>',
                '<option value="75%">Large</option>',
                '<option value="100%">Full width</option>',
                '</select>',
                '</div>',
                '<div class="col-sm-6">',
                '<label class="form-label">Shape</label>',
                '<select name="shape" class="form-select">',
                '<option value="square">Square</option>',
                '<option value="rounded">Rounded</option>',
                '<option value="circle">Circle</option>',
                '</select>',
                '</div>',
                '<div class="col-12">',
                '<label class="form-label">Layout</label>',
                '<select name="layout" class="form-select">',
                '<option value="center">Centered block</option>',
                '<option value="left">Left block</option>',
                '<option value="right">Right block</option>',
                '<option value="inline">Inline with text</option>',
                '<option value="wrap-left">Tight wrap left</option>',
                '<option value="wrap-right">Tight wrap right</option>',
                '</select>',
                '</div>',
                '</div>',
                '</form>',
                '</div>',
                '<div class="modal-footer">',
                '<button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancel</button>',
                '<button type="button" class="btn btn-primary" data-image-options-save>Apply</button>',
                '</div>',
                '</div>',
                '</div>'
            ].join("");
            document.body.appendChild(element);
        }

        return {
            element: element,
            modal: bootstrap.Modal.getOrCreateInstance(element)
        };
    }

    function initImageUrlUploads() {
        document.querySelectorAll(".btn-upload-media-url").forEach(function (button) {
            button.addEventListener("click", async function () {
                const group = button.closest(".input-group");
                const input = group ? group.querySelector("input") : null;
                if (!input) {
                    return;
                }

                const file = await chooseImageFile();
                if (!file) {
                    return;
                }

                button.disabled = true;
                const originalHtml = button.innerHTML;
                button.innerHTML = '<span class="spinner-border spinner-border-sm" aria-hidden="true"></span>';

                try {
                    const result = await uploadImage(
                        file,
                        input.dataset.uploadUrl || button.dataset.uploadUrl || defaultUploadUrl,
                        input.dataset.mediaGroup || button.dataset.mediaGroup || "Editor",
                        input.closest("form")
                    );

                    input.value = result.url;
                    input.dispatchEvent(new Event("input", { bubbles: true }));
                    input.dispatchEvent(new Event("change", { bubbles: true }));
                } catch (error) {
                    alert(error.message || "Image upload failed.");
                } finally {
                    button.disabled = false;
                    button.innerHTML = originalHtml;
                }
            });
        });
    }

    function chooseImageFile() {
        return new Promise(function (resolve) {
            const input = document.createElement("input");
            input.type = "file";
            input.accept = "image/*";
            input.addEventListener("change", function () {
                resolve(input.files && input.files.length > 0 ? input.files[0] : null);
            }, { once: true });
            input.click();
        });
    }

    async function uploadImage(file, uploadUrl, mediaGroup, form) {
        const token = getAntiForgeryToken(form);
        const payload = new FormData();
        payload.append("file", file);
        payload.append("mediaGroup", mediaGroup || "Editor");
        payload.append("altText", file.name.replace(/\.[^.]+$/, ""));

        const headers = {};
        if (token) {
            headers.RequestVerificationToken = token;
        }

        const response = await fetch(uploadUrl || defaultUploadUrl, {
            method: "POST",
            headers: headers,
            body: payload
        });

        const result = await response.json().catch(function () {
            return null;
        });

        if (!response.ok || !result || result.success === false) {
            throw new Error(result && result.message ? result.message : "Image upload failed.");
        }

        return result;
    }

    function getAntiForgeryToken(form) {
        const scoped = form ? form.querySelector("input[name='__RequestVerificationToken']") : null;
        const global = document.querySelector("input[name='__RequestVerificationToken']");
        return scoped ? scoped.value : (global ? global.value : "");
    }

    function getCleanEditorHtml(surface) {
        const clone = surface.cloneNode(true);
        clone.querySelectorAll(".admin-rich-editor-selected-image").forEach(function (image) {
            image.classList.remove("admin-rich-editor-selected-image");
        });

        return normalizeEditorHtml(clone.innerHTML);
    }

    function normalizeEditorHtml(html) {
        const trimmed = (html || "").trim();
        return trimmed === "<p><br></p>" ? "" : trimmed;
    }

    function escapeHtml(value) {
        return (value || "")
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#39;");
    }

    function escapeAttribute(value) {
        return escapeHtml(value);
    }
})();
