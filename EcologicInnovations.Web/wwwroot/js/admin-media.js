document.addEventListener("DOMContentLoaded", () => {
    const copyButtons = document.querySelectorAll(".js-copy-text");

    copyButtons.forEach(button => {
        button.addEventListener("click", async () => {
            const text = button.getAttribute("data-copy-text");
            if (!text) {
                return;
            }

            try {
                await navigator.clipboard.writeText(text);

                const originalText = button.innerHTML;
                button.innerHTML = '<i class="bi bi-check2 me-1"></i> Copied';
                button.classList.remove("btn-outline-primary", "btn-outline-success");
                button.classList.add("btn-success");

                setTimeout(() => {
                    button.innerHTML = originalText;
                    button.classList.remove("btn-success");

                    if (originalText.includes("Copy HTML")) {
                        button.classList.add("btn-outline-success");
                    } else {
                        button.classList.add("btn-outline-primary");
                    }
                }, 1200);
            } catch {
                alert("Copy failed. Please copy manually.");
            }
        });
    });
});
