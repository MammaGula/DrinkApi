const drinksApiUrl = "http://localhost:5211/api/Drinks";
const ordersApiUrl = "http://localhost:5211/api/Orders";

function setMenuStatus(message, className) {
  const statusElement = document.getElementById("menuStatus");
  if (!statusElement) return;
  statusElement.textContent = message || "";
  statusElement.className = className || "mt-2";
}

function setRowStatus(id, message, className) {
  const rowStatus = document.getElementById(`menu-status-${id}`);
  if (!rowStatus) return;
  rowStatus.textContent = message || "";
  rowStatus.className = className || "";
}

// Load all drinks for menu management
async function loadMenu() {
  const menuList = document.getElementById("menuList");
  if (!menuList) return;

  menuList.innerHTML = '<p class="text-light">Loading menu...</p>';
  setMenuStatus("", "mt-2");

  try {
    const response = await fetch(drinksApiUrl);

    if (!response.ok) {
      menuList.innerHTML = '<p class="text-danger">Error loading menu.</p>';
      return;
    }

    const drinks = await response.json();

    if (!Array.isArray(drinks) || drinks.length === 0) {
      menuList.innerHTML = '<p class="text-light">No drinks found.</p>';
      return;
    }

    let html =
      '<div class="table-responsive"><table class="table table-dark table-striped align-middle">';
    html +=
      "<thead><tr><th>ID</th><th>Name</th><th>Type</th><th>Sweetness</th><th>Price (kr)</th><th>Actions</th><th>Status</th></tr></thead><tbody>";

    drinks.forEach((drink) => {
      const drinkType = drink.type ?? "";
      const drinkSweetness = drink.sweetness ?? 0;
      html += `<tr>
        <td>${drink.id}</td>
        <td>
          <input
            type="text"
            class="form-control"
            id="menu-name-${drink.id}"
            value="${drink.name}"
          />
        </td>
        <td>
          <input
            type="text"
            class="form-control"
            id="menu-type-${drink.id}"
            value="${drinkType}"
          />
        </td>
        <td>
          <input
            type="number"
            class="form-control"
            id="menu-sweetness-${drink.id}"
            value="${drinkSweetness}"
            min="0"
            max="10"
            step="1"
          />
        </td>
        <td>
          <input
            type="number"
            class="form-control"
            id="menu-price-${drink.id}"
            value="${drink.price}"
            min="0"
            step="0.01"
          />
        </td>
        <td>
          <button class="btn btn-sm btn-success me-2" onclick="updateDrink(${drink.id})">
            Update
          </button>
          <button class="btn btn-sm btn-danger" onclick="deleteDrink(${drink.id})">
            Delete
          </button>
        </td>
        <td><span id="menu-status-${drink.id}"></span></td>
      </tr>`;
    });

    html += "</tbody></table></div>";
    menuList.innerHTML = html;
  } catch (error) {
    console.error("Error loading menu:", error);
    menuList.innerHTML = '<p class="text-danger">Could not load menu.</p>';
  }
}

// Load all orders
async function loadOrders() {
  const ordersList = document.getElementById("ordersList");
  ordersList.innerHTML = '<p class="text-light">Loading orders...</p>';

  try {
    const response = await fetch(ordersApiUrl);

    if (!response.ok) {
      ordersList.innerHTML = '<p class="text-danger">Error loading orders.</p>';
      return;
    }

    const orders = await response.json();

    if (orders.length === 0) {
      ordersList.innerHTML = '<p class="text-light">No orders found.</p>';
      return;
    }

    let html =
      '<div class="table-responsive"><table class="table table-dark table-striped">';
    html +=
      "<thead><tr><th>ID</th><th>Drink Name</th><th>Quantity</th><th>Unit Price</th><th>Total Price</th><th>Created At</th></tr></thead><tbody>";

    orders.forEach((order) => {
      const unitPrice =
        typeof order.unitPrice === "number"
          ? order.unitPrice.toFixed(2) + " kr"
          : "-";
      const totalPrice =
        typeof order.totalPrice === "number"
          ? order.totalPrice.toFixed(2) + " kr"
          : "-";
      const createdAt = order.createdAt
        ? new Date(order.createdAt).toLocaleString()
        : "";

      html += `<tr>
        <td>${order.id}</td>
        <td>${order.drinkName}</td>
        <td>${order.quantity}</td>
        <td>${unitPrice}</td>
        <td>${totalPrice}</td>
        <td>${createdAt}</td>
      </tr>`;
    });

    html += "</tbody></table></div>";
    ordersList.innerHTML = html;
  } catch (error) {
    console.error("Error loading orders:", error);
    ordersList.innerHTML = '<p class="text-danger">Could not load orders.</p>';
  }
}

// Update an existing drink
async function updateDrink(id) {
  const nameInput = document.getElementById(`menu-name-${id}`);
  const typeInput = document.getElementById(`menu-type-${id}`);
  const sweetnessInput = document.getElementById(`menu-sweetness-${id}`);
  const priceInput = document.getElementById(`menu-price-${id}`);

  if (!nameInput || !typeInput || !sweetnessInput || !priceInput) return;

  const name = nameInput.value.trim();
  const type = typeInput.value.trim();
  const sweetness = Number(sweetnessInput.value);
  const price = priceInput.value;

  if (!name || !type || price === "" || Number.isNaN(sweetness)) {
    setRowStatus(id, "Fill in all fields.", "text-warning");
    return;
  }

  if (sweetness < 0 || sweetness > 10) {
    setRowStatus(id, "Sweetness must be 0-10.", "text-warning");
    return;
  }

  const drink = {
    name: name,
    type: type,
    sweetness: sweetness,
    price: parseFloat(price),
  };

  setRowStatus(id, "Saving...", "text-light");

  try {
    const response = await fetch(`${drinksApiUrl}/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(drink),
    });

    if (response.ok) {
      setRowStatus(id, "Updated.", "text-success");
    } else {
      setRowStatus(id, "Update failed.", "text-danger");
    }
  } catch (error) {
    console.error("Error updating drink:", error);
    setRowStatus(id, "Update error.", "text-danger");
  }
}

// Delete an existing drink
async function deleteDrink(id) {
  const confirmed = window.confirm("Delete this drink?");
  if (!confirmed) return;

  setRowStatus(id, "Deleting...", "text-light");

  try {
    const response = await fetch(`${drinksApiUrl}/${id}`, {
      method: "DELETE",
    });

    if (response.ok) {
      setRowStatus(id, "Deleted.", "text-success");
      await loadMenu();
    } else {
      setRowStatus(id, "Delete failed.", "text-danger");
    }
  } catch (error) {
    console.error("Error deleting drink:", error);
    setRowStatus(id, "Delete error.", "text-danger");
  }
}

// Add a new drink
async function addDrink() {
  const name = document.getElementById("drinkName").value;
  const type = document.getElementById("drinkType").value;
  const sweetnessValue = document.getElementById("drinkSweetness").value;
  const price = document.getElementById("drinkPrice").value;
  const statusElement = document.getElementById("addDrinkStatus");

  const sweetness = Number(sweetnessValue);

  if (!name.trim() || !type.trim() || price === "" || Number.isNaN(sweetness)) {
    statusElement.textContent = "Please fill in all fields.";
    statusElement.className = "mt-3 text-warning";
    return;
  }

  if (sweetness < 0 || sweetness > 10) {
    statusElement.textContent = "Sweetness must be 0-10.";
    statusElement.className = "mt-3 text-warning";
    return;
  }

  const drink = {
    name: name.trim(),
    type: type.trim(),
    sweetness: sweetness,
    price: parseFloat(price),
  };

  try {
    const response = await fetch(drinksApiUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(drink),
    });

    if (response.ok) {
      statusElement.textContent = "Drink added successfully!";
      statusElement.className = "mt-3 text-success";

      // Clear form
      document.getElementById("drinkName").value = "";
      document.getElementById("drinkType").value = "";
      document.getElementById("drinkSweetness").value = "";
      document.getElementById("drinkPrice").value = "";

      await loadMenu();
    } else {
      statusElement.textContent = "Error adding drink.";
      statusElement.className = "mt-3 text-danger";
    }
  } catch (error) {
    console.error("Error adding drink:", error);
    statusElement.textContent = "Could not add drink.";
    statusElement.className = "mt-3 text-danger";
  }
}

// Orders polling control
let ordersPollingId = null;
function startOrdersPolling(intervalMs = 5000) {
  if (ordersPollingId) return;
  ordersPollingId = setInterval(loadOrders, intervalMs);
}

function stopOrdersPolling() {
  if (!ordersPollingId) return;
  clearInterval(ordersPollingId);
  ordersPollingId = null;
}

// Start loading data and polling when DOM is ready
document.addEventListener("DOMContentLoaded", () => {
  loadMenu();
  loadOrders();
  startOrdersPolling(5000);
});
