
(function () {

    const SHOW_Badges = false;

    const badge = document.getElementById("dirtyBadge");
    const modalBadge = document.getElementById("dirtyBadgeModal");

    function updateBadge(form) {
        const count = form.querySelectorAll(".dirty-field").length;

        [badge, modalBadge].forEach(b => {
            if (!b) return;

            b.textContent = `Unsaved (${count})`;
            if (SHOW_Badges && count > 0) {
                b.classList.remove("d-none");
            } else {
                b.classList.add("d-none");
            }
        });
    }

    let pendingNavigation = null;
    let confirmOpen = false;

    function showReset(form) {
        const btn = form?.querySelector(".dirty-reset");
        if (!btn) return;

        if (SHOW_Badges && btn.classList.contains("d-none")) {
            btn.classList.remove("d-none");

            btn.classList.add("dirty-reset-appear");
            btn.addEventListener("animationend", () => {
                btn.classList.remove("dirty-reset-appear");
            }, { once: true });
        }
    }

    function hideReset(form) {
        const btn = form?.querySelector(".dirty-reset");
        if (!btn) return;
        btn.classList.add("d-none");
    }

    document.querySelectorAll("form.dirty-form")
        .forEach(f => hideReset(f));

    function isValidDirtyInput(el) {
        const form = el.closest("form.dirty-form");
        return form &&
            el.type !== "hidden" &&
            !el.readOnly;
    }

    document.addEventListener("input", function (e) {
        if (!isValidDirtyInput(e.target)) return;

        const form = e.target.closest("form.dirty-form");
        if (!form) return;

        form.dataset.isDirty = "true";
        e.target.classList.add("dirty-field");

        showReset(form);
        updateBadge(form);
    });

    window.addEventListener("beforeunload", function (e) {
        const anyDirty = document.querySelector("form[data-is-dirty='true']");
        if (!anyDirty) return;

        e.preventDefault();
        e.returnValue = "";
    });

    document.addEventListener("submit", function (e) {
        if (!e.target.matches("form.dirty-form")) return;

        const form = e.target;

        delete form.dataset.isDirty;

        form.querySelectorAll(".dirty-field")
            .forEach(x => x.classList.remove("dirty-field"));

        hideReset(form);
        updateBadge(form);
    });

    document.addEventListener("hide.bs.modal", function (e) {
        const modalEl = e.target;
        const dirtyForm = modalEl.querySelector("form.dirty-form[data-is-dirty='true']");
        if (!dirtyForm || confirmOpen) return;

        confirmOpen = true;

        new bootstrap.Modal(
            document.getElementById("dirtyConfirmModal")
        ).show();

        e.preventDefault();

        pendingNavigation = () =>
            bootstrap.Modal.getInstance(modalEl).hide();
    });

    document.addEventListener("click", function (e) {
        if (!e.target.classList.contains("dirty-reset")) return;

        const btn = e.target;
        const form = btn.closest("form.dirty-form");
        if (!form) return;

        btn.classList.add("dirty-reset-click");
        btn.addEventListener("animationend", () => {
            btn.classList.remove("dirty-reset-click");
        }, { once: true });

        delete form.dataset.isDirty;

        form.querySelectorAll(".dirty-field")
            .forEach(x => x.classList.remove("dirty-field"));

        hideReset(form);
        updateBadge(form);
    });

    document.getElementById("dirtyContinue")?.addEventListener("click", function () {
        confirmOpen = false;

        document.querySelectorAll("form[data-is-dirty='true']")
            .forEach(f => delete f.dataset.isDirty);

        document.querySelectorAll(".dirty-field")
            .forEach(x => x.classList.remove("dirty-field"));

        document.querySelectorAll("form.dirty-form")
            .forEach(f => hideReset(f));

        [badge, modalBadge].forEach(b => b?.classList.add("d-none"));

        const confirmEl = document.getElementById("dirtyConfirmModal");
        const confirmModal = bootstrap.Modal.getInstance(confirmEl);
        if (confirmModal) confirmModal.hide();

        if (pendingNavigation) pendingNavigation();
        pendingNavigation = null;

        setTimeout(() => {
            const modalEl = document.getElementById("appModal");
            if (modalEl) rebindFormValidationAndButtons(modalEl);
        }, 50);
    });

    document.getElementById("dirtyCancel")?.addEventListener("click", function () {
        confirmOpen = false;
        pendingNavigation = null;

        const confirmEl = document.getElementById("dirtyConfirmModal");
        const confirmModal = bootstrap.Modal.getInstance(confirmEl);
        if (confirmModal) confirmModal.hide();
    });

})();

