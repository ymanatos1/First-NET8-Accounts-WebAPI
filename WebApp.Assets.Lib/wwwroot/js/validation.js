

// ================================
// SUBMIT ENABLE / DISABLE
// ================================

function updateSubmitButtonState(form) {
    if (!form) return;

    const submitBtn = form.querySelector(".js-form-submit");
    if (!submitBtn) return;

    if ($(form).valid()) {
        submitBtn.disabled = false;
    } else {
        submitBtn.disabled = true;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("form").forEach(form => {
        updateSubmitButtonState(form);
    });
});

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

document.addEventListener("shown.bs.modal", function (e) {
    const modal = e.target;
    const form = modal.querySelector("form");
    if (!form) return;
    updateSubmitButtonState(form);
});

function rebindFormValidationAndButtons(container) {
    const form = container
        ? container.querySelector("form")
        : document.querySelector("#appModalContent form");

    if (!form) return;

    if ($(form).data("validator")) {
        $(form).removeData("validator");
        $(form).removeData("unobtrusiveValidation");
    }

    $.validator.unobtrusive.parse(form);
    updateSubmitButtonState(form);
}

// ================================
// HARD MAXLENGTH
// ================================

document.addEventListener("input", function (e) {
    const el = e.target;
    if (!el.hasAttribute("maxlength")) return;

    const max = parseInt(el.getAttribute("maxlength"));
    if (!max) return;

    if (el.value.length > max) {
        el.value = el.value.substring(0, max);
    }
});

// ================================
// CHARACTER COUNTER
// ================================

function updateCharCounter(el) {
    const max = parseInt(el.getAttribute("maxlength"));
    if (!max) return;

    let counter = el.parentElement.querySelector(".char-counter");
    if (!counter) return;

    const length = el.value.length;
    counter.textContent = `${length} / ${max}`;

    if (length >= max * 0.9) {
        counter.classList.add("text-danger", "fw-bold");
    } else {
        counter.classList.remove("text-danger", "fw-bold");
    }
}

document.addEventListener("input", function (e) {
    const el = e.target;
    if (!el.matches("[data-char-counter]")) return;
    updateCharCounter(el);
});

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("[data-char-counter]").forEach(updateCharCounter);
});

document.addEventListener("shown.bs.modal", function () {
    document.querySelectorAll("[data-char-counter]").forEach(updateCharCounter);
});

// ================================
// REQUIRED BADGES
// ================================

function markRequiredFields(container) {
    container = container || document;

    container.querySelectorAll("input, select").forEach(el => {
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

document.addEventListener("DOMContentLoaded", () => {
    markRequiredFields(document);
});

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
    if (!value) return true;
    const regex = new RegExp(params);
    return regex.test(value);
});

$.validator.unobtrusive.adapters.add("username", ["pattern"], function (options) {
    options.rules["username"] = options.params.pattern;
    options.messages["username"] = options.message;
});


