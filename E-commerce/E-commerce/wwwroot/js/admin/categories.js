document.addEventListener("DOMContentLoaded", () => {

    const editModalElement = document.getElementById("editModal");
    const editModal = new bootstrap.Modal(editModalElement);

    const categoryId = document.getElementById("categoryId");
    const categoryName = document.getElementById("categoryName");
    const saveButton = document.getElementById("saveCategoryBtn");

    document.querySelectorAll(".edit-btn").forEach(button => {

        button.addEventListener("click", () => {

            categoryId.value = button.dataset.id;
            categoryName.value = button.dataset.name;

            editModal.show();
        });

    });

    saveButton.addEventListener("click", async () => {

        const response = await fetch("/Admin/Categories/Edit", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": getCsrfToken()
            },
            body: JSON.stringify({
                id: parseInt(categoryId.value),
                name: categoryName.value
            })
        });

        const result = await response.json();

        if (result.responseCode === 200) {
            showToast(result.responseMessage);
            location.reload();
        }
        else {
            showToast(result.responseMessage, "error");
        }
    });

    document.querySelectorAll(".delete-btn").forEach(button => {

        button.addEventListener("click", async () => {

            if (!confirm("Delete this category?")) {
                return;
            }

            const response = await fetch(`/Admin/Categories/Delete?id=${button.dataset.id}`, {
                method: "POST",
                headers: {
                    "RequestVerificationToken": getCsrfToken()
                }
            });

            const result = await response.json();

            if (result.responseCode === 200) {
                showToast(result.responseMessage);
                location.reload();
            }
            else {
                showToast(result.responseMessage, "error");
            }

        });

    });

    document.querySelectorAll(".restore-btn").forEach(button => {

        button.addEventListener("click", async () => {

            const response = await fetch(`/Admin/Categories/Restore?id=${button.dataset.id}`, {
                method: "POST",
                headers: {
                    "RequestVerificationToken": getCsrfToken()
                }
            });

            const result = await response.json();

            if (result.responseCode === 200) {
                showToast(result.responseMessage);
                location.reload();
            }
            else {
                showToast(result.responseMessage, "error");
            }

        });

    });

});