const drinksApiUrl = "http://localhost:5211/api/Drinks";
const ordersApiUrl = "http://localhost:5211/api/Orders";

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

    let html = '<div class="table-responsive"><table class="table table-dark table-striped">';
    html += '<thead><tr><th>ID</th><th>Drink Name</th><th>Quantity</th></tr></thead><tbody>';

    orders.forEach((order) => {
      html += `<tr>
        <td>${order.id}</td>
        <td>${order.drinkName}</td>
        <td>${order.quantity}</td>
      </tr>`;
    });

    html += '</tbody></table></div>';
    ordersList.innerHTML = html;
  } catch (error) {
    console.error("Error loading orders:", error);
    ordersList.innerHTML = '<p class="text-danger">Could not load orders.</p>';
  }
}

// Add a new drink
async function addDrink() {
  const name = document.getElementById("drinkName").value;
  const price = document.getElementById("drinkPrice").value;
  const statusElement = document.getElementById("addDrinkStatus");

  if (!name.trim() || !price) {
    statusElement.textContent = "Please fill in all fields.";
    statusElement.className = "mt-3 text-warning";
    return;
  }

  const drink = {
    name: name,
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
      document.getElementById("drinkPrice").value = "";
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
