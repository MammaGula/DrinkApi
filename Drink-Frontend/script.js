const drinksApiUrl = "http://localhost:5211/api/Drinks";

// Load drinks from backend
async function loadDrinks() {
  const list = document.getElementById("drinkList");

  // Toggle: if list has drinks, hide them
  if (list.innerHTML.trim() !== "") {
    list.innerHTML = "";
    return;
  }

  try {
    const response = await fetch(drinksApiUrl);

    if (!response.ok) {
      console.error("API error:", response.status, response.statusText);
      return;
    }

    const drinks = await response.json();

    drinks.forEach((drink) => {
      const item = document.createElement("li");
      item.className =
        "list-group-item d-flex justify-content-between align-items-center";

      item.innerHTML = `
                <div>
                    <a href="drink.html?id=${drink.id}" class="text-dark text-decoration-none"><strong>${drink.name}</strong></a>
                </div>
                <span class="badge bg-success rounded-pill">${drink.price} kr</span>
            `;

      list.appendChild(item);
    });
  } catch (error) {
    console.error("Error loading drinks:", error);
  }
}
