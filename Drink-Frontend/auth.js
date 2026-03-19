const authStorageKey = "cozyCornerAuthToken";

// TODO: Update this URL to match your backend login endpoint.
// If your backend uses ASP.NET Identity with a JWT endpoint, point it here.
const loginApiUrl = "http://localhost:5211/api/Auth/login";

function getAuthToken() {
  return localStorage.getItem(authStorageKey);
}

function setAuthToken(token) {
  if (token) {
    localStorage.setItem(authStorageKey, token);
  } else {
    localStorage.removeItem(authStorageKey);
  }
}

function clearAuthToken() {
  localStorage.removeItem(authStorageKey);
}

function isAuthenticated() {
  return Boolean(getAuthToken());
}

function ensureAuthenticated(redirectTo = "login.html") {
  if (!isAuthenticated()) {
    window.location.href = redirectTo;
  }
}

function fetchWithAuth(input, init = {}) {
  const token = getAuthToken();
  const headers = new Headers(init.headers || {});
  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }
  return fetch(input, { ...init, headers });
}

function initAuthNav() {
  const authLink = document.getElementById("authLink");
  if (!authLink) return;

  if (isAuthenticated()) {
    authLink.innerHTML = `<button class="btn btn-outline-light" onclick="logout()">Logout</button>`;
  } else {
    authLink.innerHTML = `<a href="login.html" class="btn btn-outline-light">Login</a>`;
  }
}

function logout() {
  clearAuthToken();
  initAuthNav();
  window.location.href = "index.html";
}

document.addEventListener("DOMContentLoaded", initAuthNav);
