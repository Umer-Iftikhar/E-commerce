let originalProfileName = "";
let originalProfileEmail = "";


document.addEventListener("DOMContentLoaded", () => {
    initializeChangePassword();
    initializeProfileSidebar();
});

function initializeChangePassword() {

    const form = document.getElementById("changePasswordForm");

    if (!form) {
        return;
    }

    form.addEventListener("submit", submitChangePassword);
}

function initializeProfileSidebar() {

    const profileButton = document.getElementById("profileButton");

    if (!profileButton) {
        return;
    }

    profileButton.addEventListener("click", openProfileSidebar);

    document.addEventListener("click", handleOutsideSidebarClick);

    document.addEventListener("click", async function (event) {

        const uploadButton = event.target.closest("#uploadProfileImageBtn");

        if (!uploadButton) {
            return;
        }

        await uploadProfilePicture();
    });
}

async function submitChangePassword(event) {

    event.preventDefault();

    const form = event.target;

    const currentPassword = form.querySelector("#CurrentPassword").value;
    const newPassword = form.querySelector("#NewPassword").value;
    const confirmPassword = form.querySelector("#ConfirmPassword").value;

    if (!currentPassword || !newPassword || !confirmPassword) {
        showToast("Please fill in all fields.", "warning");
        return;
    }

    if (newPassword !== confirmPassword) {
        showToast("New password and confirmation do not match.", "warning");
        return;
    }

    const button = form.querySelector("button[type='submit']");

    try {

        button.disabled = true;
        button.innerHTML = "Changing...";

        const response = await fetch("/Profile/ChangePassword", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": getCsrfToken()
            },
            body: JSON.stringify({
                currentPassword,
                newPassword
            })
        });

        const result = await response.json();

        showToast(
            result.responseMessage,
            result.responseCode === 200 ? "success" : "error"
        );

        if (result.responseCode === 200) {

            form.reset();

            setTimeout(() => {
                window.location.href = "/Profile/ChangePassword";
            }, 1500);
        }
    }
    catch (error) {

        console.error(error);

        showToast(
            "Something went wrong. Please try again.",
            "error"
        );
    }
    finally {

        button.disabled = false;
        button.innerText = "Change Password";
    }
}

async function openProfileSidebar() {

    const profileSidebar = document.getElementById("profileSidebar");
    const pageContent = document.getElementById("pageContent");

    if (profileSidebar.classList.contains("open")) {
        closeProfileSidebar();
        return;
    }

    if (profileSidebar.innerHTML.trim() === "") {

        try {

            const response = await fetch("/Profile/GetProfile");

            if (!response.ok) {
                showToast("Failed to load profile.", "error");
                return;
            }

            profileSidebar.innerHTML = await response.text();

            initializeProfileEditor();

        }
        catch (error) {

            console.error(error);

            showToast("Something went wrong.", "error");
            return;

        }
    }

    profileSidebar.classList.add("open");
    pageContent.classList.add("profile-open");
}

function closeProfileSidebar() {

    const profileSidebar = document.getElementById("profileSidebar");
    const pageContent = document.getElementById("pageContent");

    profileSidebar.classList.remove("open");
    pageContent.classList.remove("profile-open");
}

function handleOutsideSidebarClick(event) {

    const profileSidebar = document.getElementById("profileSidebar");
    const profileButton = document.getElementById("profileButton");

    if (!profileSidebar.classList.contains("open")) {
        return;
    }

    if (profileSidebar.contains(event.target)) {
        return;
    }

    if (profileButton && profileButton.contains(event.target)) {
        return;
    }

    closeProfileSidebar();
}

async function uploadProfilePicture() {

    const fileInput = document.getElementById("profileImageInput");

    if (!fileInput || fileInput.files.length === 0) {
        showToast("Please select an image.", "warning");
        return;
    }

    const button = document.getElementById("uploadProfileImageBtn");

    const formData = new FormData();

    formData.append("profileImage", fileInput.files[0]);

    try {

        button.disabled = true;
        button.innerHTML = "Uploading...";

        const response = await fetch("/Profile/UpdateProfile", {
            method: "POST",
            headers: {
                "RequestVerificationToken": getCsrfToken()
            },
            body: formData
        });

        const result = await response.json();

        showToast(
            result.responseMessage,
            result.responseCode === 200 ? "success" : "error"
        );

        if (result.responseCode !== 200) {
            return;
        }

        await refreshProfileSidebar();
    }
    catch (error) {

        console.error(error);

        showToast("Failed to upload image.", "error");
    }
    finally {

        button.disabled = false;

        button.innerHTML = `
            <img src="/icons/pencil.svg"
                 class="sidebar-icon"
                 alt="Change Picture" />
            Change Picture`;
    }
}

async function refreshProfileSidebar() {

    const profileSidebar = document.getElementById("profileSidebar");

    const response = await fetch("/Profile/GetProfile");

    if (!response.ok) {
        throw new Error("Failed to refresh profile.");
    }

    profileSidebar.innerHTML = await response.text();
}

function initializeProfileEditor() {

    const nameInput = document.getElementById("profileName");
    const emailInput = document.getElementById("profileEmail");

    if (!nameInput || !emailInput) {
        return;
    }

    originalProfileName = nameInput.value;
    originalProfileEmail = emailInput.value;

    nameInput.addEventListener("input", toggleSaveProfileButton);
    emailInput.addEventListener("input", toggleSaveProfileButton);

    const saveButton = document.getElementById("saveProfileBtn");

    if (saveButton) {
        saveButton.addEventListener("click", updateProfile);
    }
}

function toggleSaveProfileButton() {

    const nameInput = document.getElementById("profileName");
    const emailInput = document.getElementById("profileEmail");
    const saveButton = document.getElementById("saveProfileBtn");

    if (!nameInput || !emailInput || !saveButton) {
        return;
    }

    const hasChanges = nameInput.value !== originalProfileName || emailInput.value !== originalProfileEmail;

    saveButton.classList.toggle("d-none", !hasChanges);
}

async function updateProfile() {

    const nameInput = document.getElementById("profileName");
    const emailInput = document.getElementById("profileEmail");
    const saveButton = document.getElementById("saveProfileBtn");

    const name = nameInput.value.trim();
    const email = emailInput.value.trim();

    if (!name || !email) {
        showToast("Please fill in all fields.", "warning");
        return;
    }

    try {

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            showToast("Please enter a valid email address.", "warning");
            return;
        }

        saveButton.disabled = true;
        saveButton.innerText = "Saving...";

        const formData = new FormData();
        formData.append("name", name);
        formData.append("email", email);

        const response = await fetch("/Profile/UpdateProfile", {
            method: "POST",
            headers: {
                "RequestVerificationToken": getCsrfToken()
            },
            body: formData
        });

        const result = await response.json();

        showToast(result.responseMessage, result.responseCode === 200 ? "success" : "error");

        if (result.responseCode !== 200) {
            return;
        }

        const toastEl = document.getElementById("appToast");
        toastEl.classList.add("show");

        setTimeout(async () => {
            await fetch("/Auth/Logout", {
                method: "POST",
                headers: {
                    "RequestVerificationToken": getCsrfToken()
                }
            });
            window.location.href = "/Auth/Login";
        }, 3000);

    }
    catch (error) {

        console.error(error);

        showToast(
            "Something went wrong. Please try again.",
            "error"
        );
    }
    finally {

        saveButton.disabled = false;
        saveButton.innerText = "Save Changes";
    }
}