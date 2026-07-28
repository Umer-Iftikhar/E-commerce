
function showToast(message) {
    const toastElement = document.getElementById("cartToast");

    toastElement.querySelector(".toast-body").textContent = message;

    const toast = new bootstrap.Toast(toastElement);

    toast.show();
}

function getCsrfToken() {
    return document.querySelector('meta[name="csrf-token"]').content;
}

// Add to Cart
const addToCartButtons = document.querySelectorAll(".add-to-cart-btn");

addToCartButtons.forEach(button => {
    button.addEventListener("click", async function () {

        const productId = button.dataset.productId;

        const csrfToken = getCsrfToken()
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

// Get Cart
const cartButton = document.getElementById("cartButton");
const cartOverlay = document.getElementById("cartOverlay");

if (cartButton) {
    cartButton.addEventListener("click", async function (event) {

        event.preventDefault();

        try {

            showLoader(cartOverlay, "Loading cart...");

            cartOverlay.classList.remove("d-none");

            const response = await fetch("/Cart/GetCart");

            if (!response.ok) {
                cartOverlay.innerHTML = `<p class="text-danger">Something went wrong. Please try again.</p>`;
                return;
            }

            const html = await response.text();

            cartOverlay.innerHTML = html;

            cartOverlay.classList.remove("d-none");
        }
        catch (error) {

            cartOverlay.innerHTML = '<p class="text-danger">Network error. Please try again.</p>';
        }
    });
}


function closeCart() {
    cartOverlay.classList.add("d-none");
    cartOverlay.innerHTML = "";
}

// document.addEventListener("click", function (e) {

//     const closeButton = e.target.closest("#closeCartButton");

//     if (closeButton) {
//         closeCart();
//     }
// });

async function refreshCart() {

    const response = await fetch("/Cart/GetCart");

    if (!response.ok) {
        throw new Error("Failed to refresh cart.");
    }

    cartOverlay.innerHTML = await response.text();
}


//--------------------------------------------------//

async function updateQuantity(button, increase) {

    const cartItemId = button.dataset.cartItemId;

    const quantityElement = button.parentElement.querySelector(".quantity-value");

    let quantity = parseInt(quantityElement.textContent);

    quantity = increase ? quantity + 1 : quantity - 1;

    if (quantity <= 0)
        return;

    showLoader(cartOverlay, "Loading cart...");

    try {

        const response = await fetch("/Cart/UpdateCartItemQuantity", {
            method: "POST",
            headers: {
                "RequestVerificationToken": getCsrfToken(),
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: new URLSearchParams({
                cartItemId,
                quantity
            })
        });

        const result = await response.json();

        if (result.responseCode !== 200) {
            showToast(result.responseMessage);
            return;
        }

        await refreshCart();
    }
    catch {
        showToast("Failed to update cart.");
    }
}


async function removeCartItem(button) {

    const cartItemId = button.dataset.cartItemId;

    showLoader(cartOverlay, "Loading cart...");

    try {

        const response = await fetch("/Cart/RemoveFromCart", {
            method: "POST",
            headers: {
                "RequestVerificationToken": getCsrfToken(),
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: new URLSearchParams({
                cartItemId
            })
        });

        const result = await response.json();

        if (result.responseCode !== 200) {
            showToast(result.responseMessage);
            return;
        }

        await refreshCart();
    }
    catch {
        showToast("Failed to remove item.");
    }
}


document.addEventListener("click", async function (event) {

    const closeButton = event.target.closest("#closeCartButton");
    if (closeButton) {
        closeCart();
        return;
    }

    const increaseButton = event.target.closest(".increase-quantity");
    if (increaseButton) {
        await updateQuantity(increaseButton, true);
        return;
    }

    const decreaseButton = event.target.closest(".decrease-quantity");
    if (decreaseButton) {
        await updateQuantity(decreaseButton, false);
        return;
    }

    const removeButton = event.target.closest(".remove-cart-item");
    if (removeButton) {
        await removeCartItem(removeButton);
        return;
    }

    if (cartOverlay.classList.contains("d-none")) {
        return;
    }

    if (
        cartOverlay.contains(event.target) ||
        cartButton.contains(event.target)
    ) {
        return;
    }

    closeCart();
});
