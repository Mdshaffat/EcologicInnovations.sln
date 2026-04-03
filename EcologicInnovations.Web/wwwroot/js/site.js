document.addEventListener("DOMContentLoaded", function () {
    const currentYearElements = document.querySelectorAll("[data-current-year]");
    currentYearElements.forEach(function (element) {
        element.textContent = new Date().getFullYear().toString();
    });

    const autoFocusInvalid = document.querySelector(".input-validation-error");
    if (autoFocusInvalid) {
        autoFocusInvalid.focus();
    }
});
