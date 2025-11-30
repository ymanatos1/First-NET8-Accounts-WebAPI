// ================================
// FOCUS & VALIDATION HELPERS
// ================================

function focusFirstInput() {
    const modal = document.getElementById("appModalContent");
    if (!modal) return;

    const firstInput = modal.querySelector(
        "input.form-control:not([type=hidden]):not([disabled]), textarea.form-control, select.form-select"
    );

    if (firstInput) {
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

// ✅ Kept for compatibility
function highlightInvalidFields() {
    document.querySelectorAll('.field-validation-error').forEach(err => {
        const input = err.closest('.mb-3')?.querySelector('input, select, textarea');
        if (input) {
            input.classList.add('is-invalid');
        }
    });
}

// ================================
// SCREEN BLOCKER
// ================================

function showScreenBlocker() {
    const blocker = document.getElementById("screenBlocker");
    blocker?.classList.remove("d-none");
}

function hideScreenBlocker() {
    const blocker = document.getElementById("screenBlocker");
    blocker?.classList.add("d-none");
}

// ================================
// TOAST
// ================================

function showToast(message, type = "success", icon = "check-circle") {

    const toastEl = document.getElementById("appToast");
    const msgEl = document.getElementById("appToastMessage");
    const iconEl = document.getElementById("appToastIcon");

    toastEl.classList.remove("toast-success", "toast-warning", "toast-info", "toast-danger");
    toastEl.classList.add(`toast-${type}`);

    msgEl.innerText = message;
    iconEl.className = `bi bi-${icon}`;

    const toast = bootstrap.Toast.getOrCreateInstance(toastEl);
    toast.show();
}

// ================================
// CENTRALIZED MODAL CLEANUP
// ================================

function resetModalState() {

    hideScreenBlocker();

    document.querySelectorAll("form[data-is-dirty='true']")
        .forEach(f => {
            delete f.dataset.isDirty;
            delete f.dataset.submitting;
        });

    document.querySelectorAll(".dirty-field")
        .forEach(x => x.classList.remove("dirty-field"));

    const badge = document.getElementById("dirtyBadge");
    const modalBadge = document.getElementById("dirtyBadgeModal");
    [badge, modalBadge].forEach(b => b?.classList.add("d-none"));

    const placeholder = document.getElementById("popupAlertPlaceholder");
    if (placeholder) placeholder.innerHTML = "";
}

document.getElementById("appModal")
    ?.addEventListener("hidden.bs.modal", resetModalState);

// ================================
// MODAL CORE
// ================================

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
                modalEl.addEventListener("shown.bs.modal", focusFirstInput, { once: true });
                modal.show();

                rebindFormValidationAndButtons(modalEl);
            })
            .catch(err => console.error(err));
    },

    submit: function (form) {

        if (form.dataset.submitting === "true") return false;
        form.dataset.submitting = "true";

        showScreenBlocker();

        fetch(form.action, {
            method: "POST",
            body: new FormData(form)
        })
            .then(r => r.text())
            .then(result => {

                try {
                    const json = JSON.parse(result);

                    if (json.success) {

                        if (json.message) {
                            showToast(
                                json.message,
                                json.toastType || "success",
                                json.icon || "check-circle"
                            );
                        }

                        const modalEl = document.getElementById("appModal");
                        const modal = modalEl
                            ? bootstrap.Modal.getOrCreateInstance(modalEl)
                            : null;
                        if (modal) modal.hide();

                        setTimeout(() => {
                            const url = window.location.href.split("#")[0];
                            window.location.replace(url);
                        }, 350);

                        return false;
                    }
                } catch {
                    // Not JSON → HTML validation
                }

                // ✅ INVALID MODELSTATE
                hideScreenBlocker();
                delete form.dataset.submitting;

                document.getElementById("appModalContent").innerHTML = result;

                focusFirstInput();
                highlightFirstInvalidField();
                scrollToFirstInvalid();

                rebindFormValidationAndButtons();
                highlightInvalidFields();

                return false;
            })
            .catch(err => {
                console.error(err);

                hideScreenBlocker();
                delete form.dataset.submitting;

                showToast("Error submitting form", "danger", "exclamation-triangle");
            });

        return false;
    }
};

// ================================
// ALERT HANDLING
// ================================

function showPopupAlert(message, type = "danger") {
    const placeholder = document.getElementById("popupAlertPlaceholder");
    if (!placeholder) return;

    placeholder.innerHTML = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>`;
}

document.addEventListener('click', function (e) {
    if (e.target.matches('.alert .btn-close')) {

        const placeholder = e.target.closest('#popupAlertPlaceholder');
        if (placeholder) placeholder.innerHTML = "";

        if (placeholder && placeholder.dataset.closeModalOnAlert === "true") {
            const modalEl = document.getElementById('appModal');
            const modal = bootstrap.Modal.getInstance(modalEl);
            if (modal) modal.hide();
        }
    }
});

