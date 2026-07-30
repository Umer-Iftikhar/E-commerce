// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showLoader(element, message = "Loading...") {

    element.innerHTML = `
        <div class="loader-container">

            <div class="loader"></div>

            <p class="mt-3 mb-0 text-muted">
                ${message}
            </p>

        </div>
    `;

}


function getCsrfToken() {
    return document.querySelector('meta[name="csrf-token"]').content;
}
function showToast(message) {
    const toastElement = document.getElementById("cartToast");

    toastElement.querySelector(".toast-body").textContent = message;

    const toast = new bootstrap.Toast(toastElement);

    toast.show();
}