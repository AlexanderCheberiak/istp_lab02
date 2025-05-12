const uri = 'api/users';
let users = [];

async function getUsers() {
    const response = await fetch(uri);
    const data = await response.json();
    users = data;
    displayUsers(data);
}

function displayUsers(data) {
    const tBody = document.getElementById("users");
    tBody.innerHTML = "";

    data.forEach(user => {
        let row = tBody.insertRow();

        row.insertCell().innerText = user.userPhone;
        row.insertCell().innerText = user.fullName;
        row.insertCell().innerText = user.userPhoto;
        row.insertCell().innerText = new Date(user.userBirth).toLocaleDateString();
        row.insertCell().innerText = user.userAbout;
        row.insertCell().innerText = user.city;

        let actions = row.insertCell();

        let editButton = document.createElement("button");
        editButton.innerText = "Edit";
        editButton.setAttribute("onclick", `displayEditForm(${user.userId})`);
        actions.appendChild(editButton);

        let deleteButton = document.createElement("button");
        deleteButton.innerText = "Delete";
        deleteButton.setAttribute("onclick", `deleteUser(${user.userId})`);
        actions.appendChild(deleteButton);
    });

    document.getElementById("counter").innerText = `Total users: ${data.length}`;
}

async function addUser() {
    const user = {
        userPhone: document.getElementById("add-phone").value,
        fullName: document.getElementById("add-name").value,
        userPhoto: document.getElementById("add-photo").value,
        userBirth: document.getElementById("add-birth").value,
        userAbout: document.getElementById("add-about").value,
        city: document.getElementById("add-city").value
    };

    const response = await fetch(uri, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    });

    if (response.ok) {
        await getUsers();
        resetAddForm();
    } else {
        const error = await response.json();
        alert("Error: " + (error.message || response.statusText));
    }
}

function resetAddForm() {
    document.getElementById("add-phone").value = "";
    document.getElementById("add-name").value = "";
    document.getElementById("add-photo").value = "";
    document.getElementById("add-birth").value = "";
    document.getElementById("add-about").value = "";
    document.getElementById("add-city").value = "";
}

function displayEditForm(id) {
    const user = users.find(u => u.userId === id);

    document.getElementById("edit-id").value = user.userId;
    document.getElementById("edit-phone").value = user.userPhone;
    document.getElementById("edit-name").value = user.fullName;
    document.getElementById("edit-photo").value = user.userPhoto;
    document.getElementById("edit-birth").value = user.userBirth.slice(0, 10);
    document.getElementById("edit-about").value = user.userAbout;
    document.getElementById("edit-city").value = user.city;

    document.getElementById("editUser").style.display = "block";
}

async function updateUser() {
    const id = document.getElementById("edit-id").value;

    const user = {
        userId: parseInt(id),
        userPhone: document.getElementById("edit-phone").value,
        fullName: document.getElementById("edit-name").value,
        userPhoto: document.getElementById("edit-photo").value,
        userBirth: document.getElementById("edit-birth").value,
        userAbout: document.getElementById("edit-about").value,
        city: document.getElementById("edit-city").value
    };

    const response = await fetch(`${uri}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    });

    if (response.ok) {
        await getUsers();
        closeInput();
    } else {
        const error = await response.json();
        alert("Error: " + (error.message || response.statusText));
    }
}

function closeInput() {
    document.getElementById("editUser").style.display = "none";
}

async function deleteUser(id) {
    const response = await fetch(`${uri}/${id}`, { method: "DELETE" });

    if (response.ok) {
        await getUsers();
    } else {
        alert("Failed to delete user.");
    }
}
