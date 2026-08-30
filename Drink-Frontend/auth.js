// Auth.js — Handles authentication (JWT) for the frontend, helper library
// - Manages JWT authentication for the frontend, including login, logout, and role-based access control

// - jwt token is gotten from the backend upon login and stored in localStorage
const authStorageKey = "cozyCornerAuthToken";

// Backend login endpoint (JWT)
const loginApiUrl = "http://localhost:5211/api/auth/login";

// Retrieves the JWT token from localStorage
function getAuthToken() {
  return localStorage.getItem(authStorageKey);
}

// Parses a JWT token and returns its payload as a JavaScript object
function parseJwt(token) {
  if (typeof token !== "string") return null;

  const splitToken = token.split(".");
  if (splitToken.length !== 3) return null;

  // Decode the payload part of the JWT (second segment) from base64url to JSON
  const payloadBase64Url = splitToken[1];
  const base64 = payloadBase64Url.replace(/-/g, "+").replace(/_/g, "/");

  try {
    // `atob` decodes base64 to bytes in a string.
    const bytes = atob(base64);

    // Convert bytes string to UTF-8 text safely via TextDecoder.
    const utf8 = new TextDecoder("utf-8").decode(
      Uint8Array.from(bytes, (c) => c.charCodeAt(0)),
    );

    return JSON.parse(utf8);
  } catch (error) {
    return null;
  }
}

// Checks if the current user has the Admin role
function isAdmin() {
  const token = getAuthToken();
  if (!token) return false;

  // If no token, user is not admin
  const payload = parseJwt(token);
  if (!payload) return false;

  // role claim can be 'role' (string), an array, or 'roles'
  const roleClaim =
    payload.role ??
    payload.roles ??
    payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  if (!roleClaim) return false;
  if (Array.isArray(roleClaim)) return roleClaim.includes("Admin");
  return roleClaim === "Admin";
}

// Ensures the current user is an Admin; redirects if not
function ensureAdmin(redirectTo = "index.html") {
  // If token indicates admin, allow. Otherwise redirect away from admin page.
  // Useful for protecting admin-only pages
  if (!isAuthenticated() || !isAdmin()) {
    window.location.href = redirectTo;
  }
}

// Sets the JWT token in localStorage (or removes it if null)
function setAuthToken(token) {
  if (token) {
    localStorage.setItem(authStorageKey, token);
  } else {
    localStorage.removeItem(authStorageKey);
  }
}

// Clears the JWT token from localStorage
function clearAuthToken() {
  localStorage.removeItem(authStorageKey);
}

// Checks if the user is currently authenticated (has a valid JWT token)
function isAuthenticated() {
  return Boolean(getAuthToken());
}

// Ensures the user is authenticated; redirects to login if not
function ensureAuthenticated(redirectTo = "login.html") {
  if (!isAuthenticated()) {
    window.location.href = redirectTo;
  }
}

// Wrapper around fetch that includes the Authorization header if the user is authenticated
function fetchWithAuth(input, init = {}) {
  const token = getAuthToken();
  const headers = new Headers(init.headers || {});
  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }
  return fetch(input, { ...init, headers });
}

// Initializes the authentication-related navigation links (Login, Logout, Admin)
function initAuthNav() {
  const authLink = document.getElementById("authLink");
  if (!authLink) return;
  // Avoid adding a duplicate Admin link if one already exists in the static nav
  const adminExists = Boolean(document.querySelector('a[href="admin.html"]'));

  if (isAuthenticated()) {
    if (isAdmin()) {
      if (adminExists) {
        // Static Admin link already present; only add Logout button here
        authLink.innerHTML = `<button class="btn btn-outline-light" onclick="logout()">Logout</button>`;
      } else {
        // No static Admin link; inject Admin + Logout
        authLink.innerHTML = `<a href="admin.html" class="btn btn-outline-light me-2">Admin</a><button class="btn btn-outline-light" onclick="logout()">Logout</button>`;
      }
    } else {
      authLink.innerHTML = `<button class="btn btn-outline-light" onclick="logout()">Logout</button>`;
    }
  } else {
    authLink.innerHTML = `<a href="login.html" class="btn btn-outline-light">Login</a>`;
  }
}

// Logs out the current user by clearing the JWT token and updating the navigation
function logout() {
  clearAuthToken();
  initAuthNav();
  window.location.href = "index.html";
}

// Initialize the authentication-related navigation links when the DOM is fully loaded
document.addEventListener("DOMContentLoaded", initAuthNav);
