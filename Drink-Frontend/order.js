const ordersApiUrl = "http://localhost:5211/api/Orders";

// Send order to backend
async function sendOrder() {
  const name = document.getElementById("orderName").value;
  const quantity = document.getElementById("orderQuantity").value;
  const statusElement = document.getElementById("orderStatus");

  if (!name.trim()) {
    statusElement.textContent = "Please enter a drink name.";
    statusElement.className = "mt-3 text-warning";
    return;
  }

  const order = {
    drinkName: name,
    quantity: Number(quantity),
  };

  try {
    const response = await fetchWithAuth(ordersApiUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(order),
    });

    if (response.ok) {
      statusElement.textContent = "Order submitted successfully!";
      statusElement.className = "mt-3 text-success";

      // Clear form
      document.getElementById("orderName").value = "";
      document.getElementById("orderQuantity").value = "1";
    } else {
      // Get the actual error message from the backend
      const errorData = await response.text();
      statusElement.textContent = `Error: ${errorData || response.statusText}`;
      statusElement.className = "mt-3 text-danger";
      console.error("Backend error:", response.status, errorData);
    }
  } catch (error) {
    console.error("Order error:", error);
    statusElement.textContent = `Could not connect to server. Make sure backend is running.`;
    statusElement.className = "mt-3 text-danger";
  }
}
