// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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

// ✅ Your original helper kept
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
// TOAST (UNCHANGED)
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
// CENTRALIZED MODAL CLEANUP  ✅
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
// DIRTY TRACKING ✅
// ================================

document.addEventListener("input", function (e) {
    const form = e.target.closest("form");
    if (!form) return;

    form.dataset.isDirty = "true";
    e.target.classList.add("dirty-field");

    const badge = document.getElementById("dirtyBadgeModal");
    badge?.classList.remove("d-none");
});

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



// ================================
// AUTO ENABLE/DISABLE SUBMIT BUTTON
// ================================

function updateSubmitButtonState(form) {
    if (!form) return;

    const submitBtn = form.querySelector(".js-form-submit");
    if (!submitBtn) return;

    // jQuery unobtrusive validation check
    if ($(form).valid()) {
        submitBtn.disabled = false;
    } else {
        submitBtn.disabled = true;
    }
}

// ✅ Initial check on page load (full-page forms)
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("form").forEach(form => {
        updateSubmitButtonState(form);
    });
});

// ✅ Re-check on every input / change
document.addEventListener("input", function (e) {
    const form = e.target.closest("form");
    if (!form) return;

    updateSubmitButtonState(form);
});

document.addEventListener("change", function (e) {
    const form = e.target.closest("form");
    if (!form) return;

    updateSubmitButtonState(form);
});

// ✅ After modal content is loaded via fetch (important!)
document.addEventListener("shown.bs.modal", function (e) {
    const modal = e.target;
    const form = modal.querySelector("form");
    if (!form) return;

    updateSubmitButtonState(form);
});

// ✅ After modal validation error HTML is injected (important!)
/*function rebindFormValidationAndButtons() {
    const form = document.querySelector("#appModalContent form");
    if (!form) return;

    $.validator.unobtrusive.parse(form);
    updateSubmitButtonState(form);
}*/
function rebindFormValidationAndButtons(container) {
    const form = container
        ? container.querySelector("form")
        : document.querySelector("#appModalContent form");

    if (!form) return;

    // ✅ Destroy old validator (prevents stale bindings)
    if ($(form).data("validator")) {
        $(form).removeData("validator");
        $(form).removeData("unobtrusiveValidation");
    }

    // ✅ Re-parse unobtrusive validation
    $.validator.unobtrusive.parse(form);

    // ✅ Immediately update submit button state
    updateSubmitButtonState(form);
}



// ================================
// FULL-PAGE VALIDATION REBIND
// ================================

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("form").forEach(form => {
        // ✅ Only care about forms that have submit buttons we manage
        if (!form.querySelector(".js-form-submit")) return;

        // ✅ Destroy any stale validator
        if ($(form).data("validator")) {
            $(form).removeData("validator");
            $(form).removeData("unobtrusiveValidation");
        }

        // ✅ Re-parse unobtrusive validation
        $.validator.unobtrusive.parse(form);

        // ✅ Initialize button state correctly
        updateSubmitButtonState(form);
    });
});






// =======================================
// HARD ENFORCE MAXLENGTH WHILE TYPING
// =======================================

document.addEventListener("input", function (e) {
    const el = e.target;

    if (!el.hasAttribute("maxlength")) return;

    const max = parseInt(el.getAttribute("maxlength"));
    if (!max) return;

    if (el.value.length > max) {
        el.value = el.value.substring(0, max);
    }
});

// =======================================
// LIVE MAXLENGTH CHARACTER COUNTER
// + SOFT WARNING AT 90%
// =======================================

function updateCharCounter(el) {
    const max = parseInt(el.getAttribute("maxlength"));
    if (!max) return;

    let counter = el.parentElement.querySelector(".char-counter");
    if (!counter) return;

    const length = el.value.length;
    counter.textContent = `${length} / ${max}`;

    // ✅ SOFT WARNING when 90% reached
    if (length >= max * 0.9) {
        counter.classList.add("text-danger", "fw-bold");
    } else {
        counter.classList.remove("text-danger", "fw-bold");
    }
}

// Run on every keystroke
document.addEventListener("input", function (e) {
    const el = e.target;
    if (!el.matches("[data-char-counter]")) return;

    updateCharCounter(el);
});

// Init on full page load
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("[data-char-counter]").forEach(updateCharCounter);
});

// Init when modal opens
document.addEventListener("shown.bs.modal", function () {
    document.querySelectorAll("[data-char-counter]").forEach(updateCharCounter);
});

// =======================================
// AUTO "REQUIRED" BADGE FROM DATA-VAL
// =======================================

function markRequiredFields(container) {
    container = container || document;

    container.querySelectorAll("input, textarea, select").forEach(el => {

        if (!el.hasAttribute("data-val-required")) return;

        const id = el.getAttribute("id");
        if (!id) return;

        const label = container.querySelector(`label[for="${id}"]`);
        if (!label || label.querySelector(".required-badge")) return;

        const badge = document.createElement("span");
        badge.className = "required-badge ms-1 text-danger fw-bold";
        badge.textContent = "*";

        label.appendChild(badge);
    });
}

// Full page
document.addEventListener("DOMContentLoaded", () => {
    markRequiredFields(document);
});

// Modal
document.addEventListener("shown.bs.modal", function () {
    const modal = document.getElementById("appModalContent");
    if (modal) markRequiredFields(modal);
});


// ================================
// CUSTOM USERNAME VALIDATION
// ================================

document.addEventListener("blur", function (e) {
    if (e.target.matches("input[name$='.Name']")) {
        e.target.value = e.target.value.trim();
    }
}, true);

$.validator.addMethod("username", function (value, element, params) {
    if (!value) return true; // [Required] handles empties

    const regex = new RegExp(params);
    return regex.test(value);
});

$.validator.unobtrusive.adapters.add("username", ["pattern"], function (options) {
    options.rules["username"] = options.params.pattern;
    options.messages["username"] = options.message;
});

