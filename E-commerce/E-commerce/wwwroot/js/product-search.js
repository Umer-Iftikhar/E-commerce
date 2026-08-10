
const dateInput = document.getElementById("CreatedDate");
const searchButton = document.getElementById("btnSearch");
const productContainer = document.getElementById("productContainer");

const minDate = "2025-12-01";
const maxDate = new Date().toISOString().split("T")[0];

dateInput.addEventListener("change", function () {

    const selectedDate = dateInput.value;

    if (selectedDate && (selectedDate < minDate || selectedDate > maxDate)) {
        searchButton.disabled = true;
    }
    else {
        searchButton.disabled = false;
    }
});

const searchForm = document.getElementById("searchForm");


searchForm.addEventListener("submit", async function (event) {

    event.preventDefault();

    const categoryId = document.getElementById("categoryId").value;
    const searchTerm = document.getElementById("searchTerm").value;
    const createdDate = document.getElementById("CreatedDate").value;

    const params = new URLSearchParams();
    if (searchTerm) {
        params.append("searchTerm", searchTerm);
    }

    if (categoryId) {
        params.append("categoryId", categoryId);
    }

    if (createdDate) {
        params.append("createdDate", createdDate);
    }

    const url = `/Product/Search?${params.toString()}`;

    try {
        const response = await fetch(url);
        if (!response.ok) {
            productContainer.innerHTML = `<p class="text-danger">Something went wrong. Please try again.</p>`;
            return;
        }

        const html = await response.text();

        productContainer.innerHTML = html;
    }
    catch (error) {
        console.error(error);
        productContainer.innerHTML = '<p class="text-danger">Network error. Please try again.</p>';
    } 
});



    
