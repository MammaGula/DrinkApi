const drinksApiUrl = "http://localhost:5211/api/Drinks";

function getQueryParam(name) {
  const params = new URLSearchParams(window.location.search);
  return params.get(name);
}

async function loadDrinkDetail() {
  const id = getQueryParam("id");
  const container = document.getElementById("drinkContainer");
  if (!id) {
    container.innerHTML = `<div class="alert alert-warning">No drink specified.</div>`;
    return;
  }

  try {
    const res = await fetch(`${drinksApiUrl}/${encodeURIComponent(id)}`);
    if (!res.ok) {
      container.innerHTML = `<div class="alert alert-danger">Could not load drink details.</div>`;
      return;
    }

    const drink = await res.json();

    const description = drink.description ?? "No description available.";
    const image = drink.imageUrl
      ? `<img src="${drink.imageUrl}" alt="${drink.name}" class="img-fluid mb-3 mx-auto d-block" style="max-height:240px; object-fit:cover">`
      : "";

    container.innerHTML = `
      <div class="card p-4 shadow-sm text-center" style="background-color: rgba(5,39,13,0.6)">
        <div class="d-flex flex-column align-items-center text-center">
          ${image ? `<div>${image}</div>` : ""}
          <div>
            <h2 style="color:#c8f7dc">${drink.name}</h2>
            <p class="text-light">${description}</p>
            <p class="text-light"><strong>Price:</strong> ${drink.price} kr</p>

            <div class="mb-3 d-flex justify-content-center align-items-center">
              <label class="form-label text-light me-2 mb-0">Quantity</label>
              <input type="number" id="detailQuantity" class="form-control" value="1" min="1" style="width:100px" />
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-center align-items-center gap-2 flex-wrap mt-3">
          <button class="btn btn-success" id="addToCartBtn">Add to basket</button>
          <a href="cart.html" class="btn btn-outline-light">View basket</a>
          <a href="index.html" class="btn btn-outline-light">Back to Drink List</a>
        </div>

        <p id="detailStatus" class="mt-3 text-center"></p>
      </div>
    `;

    document.getElementById("addToCartBtn").addEventListener("click", () => {
      addToCart(drink);
    });
  } catch (err) {
    console.error(err);
    container.innerHTML = `<div class="alert alert-danger">Error loading drink.</div>`;
  }
}

function addToCart(drink) {
  const qty = Number(document.getElementById("detailQuantity").value) || 1;
  const storageKey = "cozyCart";
  const raw = localStorage.getItem(storageKey);
  const cart = raw ? JSON.parse(raw) : [];

  const existing = cart.find((i) => i.id === drink.id);
  if (existing) {
    existing.quantity += qty;
  } else {
    cart.push({
      id: drink.id,
      name: drink.name,
      price: drink.price,
      quantity: qty,
    });
  }

  localStorage.setItem(storageKey, JSON.stringify(cart));
  const status = document.getElementById("detailStatus");
  status.textContent = `Added ${qty} × ${drink.name} to basket`;
  status.className = "mt-3 text-success";
}

document.addEventListener("DOMContentLoaded", loadDrinkDetail);
