document.addEventListener("DOMContentLoaded", () => {

    const buttons = document.querySelectorAll(".btn-order-details");

    buttons.forEach(button => {

        button.addEventListener("click", async () => {

            const orderId = button.dataset.orderId;

            const detailsRow = document.getElementById(`order-details-row-${orderId}`);
            const container = document.getElementById(`order-details-${orderId}`);

            // Already loaded 
            if (container.innerHTML.trim() !== "") {
                detailsRow.classList.toggle("d-none");
                return;
            }

            // First load
            detailsRow.classList.remove("d-none");

            showLoader(container);

            try {

                const response = await fetch(`/Order/Details/${orderId}`);

                if (!response.ok) {
                    throw new Error("Unable to load order details.");
                }

                const html = await response.text();

                container.innerHTML = html;

            }
            catch (error) {

                detailsRow.classList.add("d-none");

                showToast(error.message);

            }

        });

    });

});