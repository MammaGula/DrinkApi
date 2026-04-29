const ordersApiUrl = "http://localhost:5211/api/Orders";
const cartStorageKey = "cozyCart";

function readCart() {
  const raw = localStorage.getItem(cartStorageKey);
  return raw ? JSON.parse(raw) : [];
}

function saveCart(cart) {
  localStorage.setItem(cartStorageKey, JSON.stringify(cart));
}

function formatCurrency(n) {
  return `${n} kr`;
}

function renderCart() {
  const container = document.getElementById("cartContainer");
  const cart = readCart();
  if (!cart.length) {
    container.innerHTML = `
      <div class="card p-4 shadow-sm text-center" style="background-color: rgba(5,39,13,0.6)">
        <p class="text-light">Your basket is empty. Browse the <a href="index.html">drink list</a> to add items.</p>
      </div>
    `;
    return;
  }

  let total = 0;

  const itemsHtml = cart
    .map((it, idx) => {
      const line = it.price * it.quantity;
      total += line;
      return `
          <li class="list-group-item text-center py-1" style="height:auto; font-weight:bold;">
            <div class="fw-bold">${it.name}</div>
            <div class="small text-muted">${formatCurrency(it.price)} × ${it.quantity}</div>
            <div class="mt-1"><button class="btn btn-sm btn-outline-light" onclick="removeItem(${idx})">Remove</button></div>
          </li>
        `;
    })
    .join("\n");

  container.innerHTML = `
    <div class="card p-2 shadow-sm text-center" style="background-color: rgba(5,39,13,0.6)">
      <h3 class="mb-2 text-light">Your basket</h3>

      <div class="d-inline-block text-center mx-auto" style="width:320px; max-width:92%">
        <ul class="list-group mb-1">${itemsHtml}</ul>
        <p class="text-light medium mb-1"><strong>Total: </strong> ${formatCurrency(total)}</p>

        <div class="d-flex justify-content-center gap-3 mt-2">
          <button class="btn btn-success btn-md" id="checkoutBtn" style="min-width:150px; padding-left:1.25rem; padding-right:1.25rem">Pay / Checkout</button>
          <button class="btn btn-outline-light btn-md" id="clearBtn" style="min-width:150px; padding-left:1.25rem; padding-right:1.25rem">Clear basket</button>
          <a href="index.html" class="btn btn-outline-light btn-md" style="min-width:150px; padding-left:1.25rem; padding-right:1.25rem">Continue shopping</a>
        </div>

        <p id="cartStatus" class="mt-2 text-center text-light small"></p>
      </div>
    </div>
  `;

  document.getElementById("checkoutBtn").addEventListener("click", checkout);
  document.getElementById("clearBtn").addEventListener("click", () => {
    saveCart([]);
    renderCart();
  });
}

function removeItem(index) {
  const cart = readCart();
  cart.splice(index, 1);
  saveCart(cart);
  renderCart();
}

async function checkout() {
  const status = document.getElementById("cartStatus");
  const cart = readCart();
  if (!cart.length) {
    status.textContent = "Your basket is empty.";
    status.className = "mt-3 text-warning";
    return;
  }

  status.textContent = "Processing...";
  status.className = "mt-3 text-muted";

  try {
    // Send each cart item as an order to the backend
    for (const item of cart) {
      const order = { drinkName: item.name, quantity: Number(item.quantity) };
      const res = await fetchWithAuth(ordersApiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(order),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error(text || res.statusText);
      }
    }

    saveCart([]);
    renderCart();
    status.textContent = "Payment/checkout completed — order submitted.";
    status.className = "mt-3 text-success";
  } catch (err) {
    console.error(err);
    status.textContent = `Error during checkout: ${err.message}`;
    status.className = "mt-3 text-danger";
  }
}

document.addEventListener("DOMContentLoaded", renderCart);
