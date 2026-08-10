document.addEventListener("DOMContentLoaded", function () {

    const deleteButtons = document.querySelectorAll(".delete-product-btn");

    deleteButtons.forEach(button => {

        button.addEventListener("click", async function () {

            const productId = this.dataset.id;

            if (!confirm("Are you sure you want to delete this product?")) {
                return;
            }

            try {

                const response = await fetch(
                    `/Admin/Products/Delete?id=${productId}`,
                    {
                        method: "POST",
                        headers: {
                            "RequestVerificationToken": getCsrfToken()
                        }
                    }
                );

                const data = await response.json();

                if (data.responseCode === 200) {

                    showToast(data.responseMessage, "success");

                    setTimeout(() => {
                        window.location.reload();
                    }, 1000);

                }
                else {

                    showToast(data.responseMessage, "error");

                }

            }
            catch (error) {

                console.error(error);

                showToast(
                    "Something went wrong while deleting the product.",
                    "error"
                );

            }

        });

    });

    const restoreButtons = document.querySelectorAll(".restore-product-btn");

    restoreButtons.forEach(button => {

        button.addEventListener("click", async function () {

            const productId = this.dataset.id;

            if (!confirm("Are you sure you want to restore this product?")) {
                return;
            }

            try {

                const response = await fetch(
                    `/Admin/Products/Restore?id=${productId}`,
                    {
                        method: "POST",
                        headers: {
                            "RequestVerificationToken": getCsrfToken()
                        }
                    }
                );

                const data = await response.json();

                if (data.responseCode === 200) {

                    showToast(data.responseMessage, "success");

                    setTimeout(() => {
                        window.location.reload();
                    }, 1000);

                }
                else {

                    showToast(data.responseMessage, "error");

                }

            }
            catch (error) {

                console.error(error);

                showToast(
                    "Something went wrong while restoring the product.",
                    "error"
                );

            }

        });

    });

});