console.log("cart.js loaded");

function showToast(message) {
    const toastElement = document.getElementById("cartToast");
    toastElement.querySelector(".toast-body").textContent = message;
    const toast = new bootstrap.Toast(toastElement);
    toast.show();
}


const addToCartButtons = document.querySelectorAll(".add-to-cart-btn");

addToCartButtons.forEach(button => {
    button.addEventListener("click", async function () {
        const productId = button.dataset.productId;
        console.log(productId);

        const csrfToken = document.querySelector('meta[name="csrf-token"]').content;
        try {
            button.disabled = true;
            button.innerText = "Adding...";

            const response = await fetch("/Cart/AddToCart", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": csrfToken
                },
                body: JSON.stringify({
                    productId: productId
                })
            });
            const data = await response.json();
            showToast(data.responseMessage);

            if (response.ok) {
                button.innerText = "Added";
            }
        }
        catch (error) {
            console.error(error);
            showToast("Network error. Please try again.");
        }
        finally {
            setTimeout(() => {
                button.disabled = false;
                button.innerText = "Add to Cart";
            }, 1500);
        }
    });   
});