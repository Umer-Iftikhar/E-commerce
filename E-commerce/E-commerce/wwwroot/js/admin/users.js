document.addEventListener("DOMContentLoaded", function () {

    const deleteButtons = document.querySelectorAll(".delete-user-btn");

    deleteButtons.forEach(button => {

        button.addEventListener("click", async function () {

            const userId = this.dataset.id;

            if (!confirm("Are you sure you want to delete this user?")) {
                return;
            }

            try {

                const response = await fetch(
                    `/Admin/Users/Delete?id=${userId}`,
                    {
                        method: "POST",
                        headers: {
                            "RequestVerificationToken": getCsrfToken()
                        }
                    }
                );

                const data = await response.json();

                if (data.responseCode === 200) {

                    showToast(
                        data.responseMessage,
                        "success"
                    );

                    setTimeout(() => {
                        window.location.reload();
                    }, 1000);

                }
                else {

                    showToast(data.responseMessage, "error"
                    );

                }

            }
            catch (error) {

                console.error(error);

                showToast("Something went wrong while deleting the user.", "error"
                );

            }

        });

    });

    const restoreButtons = document.querySelectorAll(".restore-user-btn");

    restoreButtons.forEach(button => {

        button.addEventListener("click", async function () {

            const userId = this.dataset.id;

            if (!confirm("Are you sure you want to restore this user?")) {
                return;
            }

            try {

                const response = await fetch(
                    `/Admin/Users/Restore?id=${userId}`,
                    {
                        method: "POST",
                        headers: {
                            "RequestVerificationToken": getCsrfToken()
                        }
                    }
                );

                const data = await response.json();

                if (data.responseCode === 200) {

                    showToast(
                        data.responseMessage,
                        "success"
                    );

                    setTimeout(() => {
                        window.location.reload();
                    }, 1000);

                }
                else {

                    showToast(
                        data.responseMessage,
                        "error"
                    );

                }

            }
            catch (error) {

                console.error(error);

                showToast(
                    "Something went wrong while restoring the user.",
                    "error"
                );

            }

        });

    });

});