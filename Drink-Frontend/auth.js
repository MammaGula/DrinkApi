const authStorageKey = "cozyCornerAuthToken";

// Backend login endpoint (JWT)
const loginApiUrl = "http://localhost:5211/api/auth/login";

function getAuthToken() {
  return localStorage.getItem(authStorageKey);
}

function parseJwt(token) {
  if (typeof token !== "string") return null;

  const splitToken = token.split(".");
  if (splitToken.length !== 3) return null;

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

function isAdmin() {
  const token = getAuthToken();
  if (!token) return false;
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

function ensureAdmin(redirectTo = "index.html") {
  // If token indicates admin, allow. Otherwise redirect away from admin page.
  if (!isAuthenticated() || !isAdmin()) {
    window.location.href = redirectTo;
  }
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

function logout() {
  clearAuthToken();
  initAuthNav();
  window.location.href = "index.html";
}

document.addEventListener("DOMContentLoaded", initAuthNav);
