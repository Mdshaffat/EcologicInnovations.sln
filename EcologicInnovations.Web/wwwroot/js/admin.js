document.addEventListener("DOMContentLoaded", function () {
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

    // Media library selector button (simple behavior: prompt for URL)
    const openMediaBtn = document.getElementById('openMediaLibrary');
    if (openMediaBtn) {
        openMediaBtn.addEventListener('click', function (e) {
            // Find the adjacent input (previous sibling)
            const input = openMediaBtn.parentElement.querySelector('input');
            const url = prompt('Enter media URL (absolute or site-relative, e.g. /uploads/media/...):', input.value || '');
            if (url !== null) {
                input.value = url.trim();
            }
        });
    }
});
