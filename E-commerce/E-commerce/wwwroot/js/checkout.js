document.addEventListener("DOMContentLoaded", () => {

    const checkoutForm = document.getElementById("checkoutForm");

    if (!checkoutForm)
        return;

    checkoutForm.addEventListener("submit", submitCheckout);

});

async function submitCheckout(e) {

    e.preventDefault();

    const form = e.target;
    const submitButton = form.querySelector("button[type='submit']");

    submitButton.disabled = true;

    try {

        const formData = new FormData(form);

        const payload = Object.fromEntries(formData.entries());

        const response = await fetch(form.action, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": getCsrfToken()
            },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        if (result.responseCode === 200) {
            showToast(result.responseMessage, "success");
            setTimeout(() => {
                window.location.href = "/Order/GetOrders";
            }, 1500);
            return;
        }

        showToast(result.responseMessage, "error");


    }
    catch (error) {

        console.error(error);

        showToast("Something went wrong.", "error");

    }
    finally {

        submitButton.disabled = false;

    }

}

