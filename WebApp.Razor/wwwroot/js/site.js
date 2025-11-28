// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//// Make sure this file is included in _Layout.cshtml after bootstrap.bundle.js
//window.appModal = {
//    load: function (url) {
//        fetch(url, { credentials: "same-origin" })
//            .then(function (response) {
//                if (!response.ok) {
//                    throw new Error("Failed to load modal content: " + response.status);
//                }
//                return response.text();
//            })
//            .then(function (html) {
//                var contentElement = document.getElementById("appModalContent");
//                if (!contentElement) {
//                    console.error("appModalContent element not found in DOM.");
//                    return;
//                }

//                contentElement.innerHTML = html;

//                var modalElement = document.getElementById("appModal");
//                if (!modalElement) {
//                    console.error("appModal element not found in DOM.");
//                    return;
//                }

//                var modal = bootstrap.Modal.getOrCreateInstance(modalElement);
//                modal.show();
//            })
//            .catch(function (err) {
//                console.error(err);
//            });
//    }
//};

function focusFirstInput() {
    const modal = document.getElementById("appModalContent");
    if (!modal) return;

    const firstInput = modal.querySelector(
        //"input:not([type=hidden]):not([disabled]), select, textarea"
        //"input.form-control:not([type=hidden]):not([disabled])"
        "input.form-control:not([type=hidden]):not([disabled]), textarea.form-control, select.form-select"
    );

    if (firstInput) {
        // Delay ensures the element is fully rendered
        setTimeout(() => {
            firstInput.focus();
            if (firstInput.select) firstInput.select();
        }, 10);
    }
}

function scrollToFirstInvalid() {
    const container = document.getElementById("appModalContent");
    if (!container) return;

    const firstInvalid = container.querySelector(".input-validation-error, .is-invalid");

    if (firstInvalid) {
        firstInvalid.scrollIntoView({ behavior: "smooth", block: "center" });
    }
}

function highlightFirstInvalidField() {
    const container = document.getElementById("appModalContent");
    if (!container) return;

    const firstInvalid = container.querySelector(".input-validation-error, .is-invalid");

    if (firstInvalid) {
        firstInvalid.classList.add("popup-invalid-highlight");
        firstInvalid.focus();
        if (firstInvalid.select) firstInvalid.select();
    }
}

function showScreenBlocker() {
    const blocker = document.getElementById("screenBlocker");
    blocker.classList.remove("d-none");
}
function hideScreenBlocker() {
    const blocker = document.getElementById("screenBlocker");
    blocker.classList.add("d-none");
}

//function showToast(message) {
//    document.getElementById("appToastMessage").innerText = message;
//    const toastEl = document.getElementById("appToast");
//    const toast = bootstrap.Toast.getOrCreateInstance(toastEl);
//    toast.show();
//}
function showToast(message, type = "success", icon = "check-circle") {

    const toastEl = document.getElementById("appToast");
    const msgEl = document.getElementById("appToastMessage");
    const iconEl = document.getElementById("appToastIcon");

    // Clear previous color classes
    toastEl.classList.remove("toast-success", "toast-warning", "toast-info", "toast-danger");

    // Add new color class
    toastEl.classList.add(`toast-${type}`);

    // Set message and icon
    msgEl.innerText = message;
    iconEl.className = `bi bi-${icon}`;

    // Show toast
    const toast = bootstrap.Toast.getOrCreateInstance(toastEl);
    toast.show();
}


window.appModal = {
    load: function (url) {
        fetch(url, { credentials: "same-origin" })
            .then(r => r.text())
            .then(html => {
                document.getElementById("appModalContent").innerHTML = html;

                const modalEl = document.getElementById("appModal");
                if (!modalEl) {
                    console.warn("appModal element not found in DOM.");
                    return;
                }

                const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
                // ‼️ attach focus event BEFORE show()
                modalEl.addEventListener("shown.bs.modal", focusFirstInput, { once: true });
                modal.show();
            })
            .catch(err => console.error(err));
    },

    submit: function (form) {
        // 🔥 Block the entire screen during submit
        showScreenBlocker();

        fetch(form.action, {
            method: "POST",
            body: new FormData(form)
        })
            .then(r => r.text())
            .then(result => {
                // Try JSON first (success case)
                try {
                    const json = JSON.parse(result);
                    if (json.success) {
                        if (json.message)
                            //showToast(json.message || "Operation completed successfully!");
                            //showToast(json.message, "success", "check-circle");
                            showToast(json.message, json.toastType, json.icon);


                        const modalEl = document.getElementById("appModal");
                        const modal = modalEl ? bootstrap.Modal.getInstance(modalEl) : null;
                        if (modal) modal.hide();

                        // Optional: highlight updated row (if you re-render row-only)
                        //document.querySelector(`#row-${json.id}`).classList.add("popup-field-updated");

                        // delay reload so toast is visible
                        if (json.message)
                            setTimeout(() => location.reload(), 700);
                        else
                            location.reload();

                        return false;
                    }
                } catch {
                    // Not JSON → it's HTML (validation errors)
                }

                // ❗ INVALID MODELSTATE (HTML returned)
                // Unblock screen because we stay in popup
                hideScreenBlocker();

                // Load HTML (invalid model) back into modal and focus first field
                document.getElementById("appModalContent").innerHTML = result;
                focusFirstInput();   // modal already open, just refocus

                highlightFirstInvalidField();
                scrollToFirstInvalid();

                return false;
            })
            .catch(err => {
                console.error(err);
                // Unblock screen because we stay in popup
                hideScreenBlocker();
                showToast("Error submitting form", "danger", "exclamation-triangle");
            });

        return false; // prevent full page submit
    }
};



//window.loadModal = function (url) {
//    fetch(url)
//        .then(r => r.text())
//        .then(html => {
//            document.getElementById("dynamicModalContent").innerHTML = html;
//            new bootstrap.Modal(document.getElementById("dynamicModal")).show();
//        });
//};


function showPopupAlert(message, type = "danger") {
    const placeholder = document.getElementById("popupAlertPlaceholder");
    if (!placeholder) return;

    placeholder.innerHTML = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>`;
        // Similar to Razor: @await Html.PartialAsync("DTO/UI/MODAL/_DismissButton", "alert")
}





// When alert close button is clicked, close modal ONLY if "data-close-modal-on-alert=true"
document.addEventListener('click', function (e) {
    if (e.target.matches('.alert .btn-close')) {

        const placeholder = e.target.closest('#popupAlertPlaceholder');
        if (placeholder) placeholder.innerHTML = "";

        // Close modal only if _Details popup told us to
        if (placeholder && placeholder.dataset.closeModalOnAlert === "true") {
            const modalEl = document.getElementById('appModal');
            const modal = bootstrap.Modal.getInstance(modalEl);
            if (modal) modal.hide();
        }
    }
});

// When modal itself is closed, also clear alert content
document.getElementById('appModal').addEventListener('hidden.bs.modal', function () {
    const placeholder = document.getElementById('popupAlertPlaceholder');
    if (placeholder) placeholder.innerHTML = "";
});

// Highlight invalid fields on popup rendering
function highlightInvalidFields() {
    document.querySelectorAll('.field-validation-error').forEach(err => {
        const input = err.closest('.mb-3')?.querySelector('input, select, textarea');
        if (input) {
            input.classList.add('is-invalid');
        }
    });
}



