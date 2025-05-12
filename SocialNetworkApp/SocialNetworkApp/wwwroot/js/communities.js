const uri = "/api/communities";
let communities = [];

async function getCommunities() {
    const response = await fetch(uri);
    const data = await response.json();
    communities = data;
    displayCommunities(data);
}

function displayCommunities(data) {
    const tbody = document.getElementById("communities");
    tbody.innerHTML = "";

    data.forEach(community => {
        const row = tbody.insertRow();

        row.insertCell(0).innerText = community.communityName;
        row.insertCell(1).innerText = community.communityDescription || "";
        row.insertCell(2).innerText = community.communityCreatedAt?.split("T")[0] || "";

        const actions = row.insertCell(3);
        actions.innerHTML = `
            <button onclick="editCommunity(${community.communityId})">Edit</button>
            <button onclick="deleteCommunity(${community.communityId})">Delete</button>
        `;
    });

    document.getElementById("counter").innerText = `Total: ${data.length}`;
}

async function addCommunity() {
    const community = {
        communityName: document.getElementById("add-name").value,
        communityDescription: document.getElementById("add-description").value,
        communityCreatedAt: document.getElementById("add-created-at").value || new Date().toISOString()
    };

    const response = await fetch(uri, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(community)
    });

    if (response.ok) {
        getCommunities();
        document.querySelector("form").reset();
    } else {
        alert("Failed to add community.");
    }
}

function editCommunity(id) {
    const community = communities.find(c => c.communityId === id);
    document.getElementById("edit-id").value = community.communityId;
    document.getElementById("edit-name").value = community.communityName;
    document.getElementById("edit-description").value = community.communityDescription;
    document.getElementById("edit-created-at").value = community.communityCreatedAt.split("T")[0];
    document.getElementById("editForm").style.display = "block";
}

function closeEditForm() {
    document.getElementById("editForm").style.display = "none";
}

async function updateCommunity() {
    const id = document.getElementById("edit-id").value;
    const community = {
        communityId: parseInt(id),
        communityName: document.getElementById("edit-name").value,
        communityDescription: document.getElementById("edit-description").value,
        communityCreatedAt: document.getElementById("edit-created-at").value
    };

    const response = await fetch(`${uri}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(community)
    });

    if (response.ok) {
        getCommunities();
        closeEditForm();
    } else {
        alert("Failed to update community.");
    }
}

async function deleteCommunity(id) {
    if (!confirm("Are you sure you want to delete this community?")) return;

    const response = await fetch(`${uri}/${id}`, {
        method: "DELETE"
    });

    if (response.ok) {
        getCommunities();
    } else {
        alert("Failed to delete community.");
    }
}
