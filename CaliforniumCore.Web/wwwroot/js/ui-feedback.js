document.addEventListener("DOMContentLoaded", function () {
    const toastElements = document.querySelectorAll(".toast");
    toastElements.forEach(function (element) {
        if (window.bootstrap && bootstrap.Toast) {
            const toast = new bootstrap.Toast(element);
            toast.show();
        }
    });

    const autoFocusInvalid = document.querySelector(
        ".input-validation-error, .field-validation-error, .validation-summary-box"
    );

    if (autoFocusInvalid && typeof autoFocusInvalid.scrollIntoView === "function") {
        autoFocusInvalid.scrollIntoView({ behavior: "smooth", block: "center" });
    }
});
