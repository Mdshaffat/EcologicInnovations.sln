document.addEventListener("DOMContentLoaded", function () {
    registerRelativeOrAbsoluteUrlValidator();

    const sidebar = document.getElementById("adminSidebar");
    const sidebarToggle = document.getElementById("adminSidebarToggle");

    if (sidebar && sidebarToggle) {
        sidebarToggle.addEventListener("click", function () {
            sidebar.classList.toggle("show");
        });

        document.addEventListener("click", function (event) {
            const clickedInsideSidebar = sidebar.contains(event.target);
            const clickedToggle = sidebarToggle.contains(event.target);

            if (!clickedInsideSidebar && !clickedToggle && window.innerWidth < 992) {
                sidebar.classList.remove("show");
            }
        });
    }

    const toastElements = document.querySelectorAll(".admin-auto-toast");
    toastElements.forEach(function (element) {
        const toast = new bootstrap.Toast(element);
        toast.show();
    });

    // Media library selector buttons (simple behavior: prompt for URL)
    // Supports multiple buttons per page via a shared CSS class.
    document.querySelectorAll('.btn-select-media').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var input = btn.closest('.input-group').querySelector('input');
            var url = prompt('Enter media URL (absolute or site-relative, e.g. /uploads/media/...):', input.value || '');
            if (url !== null) {
                input.value = url.trim();
                input.dispatchEvent(new Event("input", { bubbles: true }));
                input.dispatchEvent(new Event("change", { bubbles: true }));
            }
        });
    });
});

function registerRelativeOrAbsoluteUrlValidator() {
    if (!window.jQuery || !window.jQuery.validator || !window.jQuery.validator.unobtrusive) {
        return;
    }

    window.jQuery.validator.addMethod("relativeorabsoluteurl", function (value, element) {
        if (this.optional(element)) {
            return true;
        }

        var trimmed = (value || "").trim();
        if (trimmed.charAt(0) === "/" && trimmed.substring(0, 2) !== "//") {
            return true;
        }

        return /^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$/i.test(trimmed);
    });

    window.jQuery.validator.unobtrusive.adapters.addBool("relativeorabsoluteurl");
}
