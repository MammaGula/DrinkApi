// Use the loginApiUrl from auth.js (should match your backend login endpoint).
// The backend should return a JSON payload with a token (e.g. { token: "..." }).

async function login() {
  const email = document.getElementById("loginEmail").value.trim();
  const password = document.getElementById("loginPassword").value;
  const statusElement = document.getElementById("loginStatus");

  if (!email || !password) {
    statusElement.textContent = "Please enter your email and password.";
    statusElement.className = "mt-3 text-warning";
    return;
  }

  statusElement.textContent = "Logging in...";
  statusElement.className = "mt-3 text-light";

  try {
    const response = await fetch(loginApiUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    });

    const text = await response.text();
    let data = null;
    try {
      data = JSON.parse(text);
    } catch (e) {
      // response is not JSON
    }

    if (!response.ok) {
      statusElement.textContent = `Login failed: ${text || response.statusText}`;
      statusElement.className = "mt-3 text-danger";
      return;
    }

    const token =
      data?.token ??
      data?.accessToken ??
      data?.data?.token ??
      data?.data?.accessToken;

    if (!token) {
      statusElement.textContent =
        "Login succeeded but no token was returned from the server (check API response).";
      statusElement.className = "mt-3 text-warning";
      return;
    }

    setAuthToken(token);
    statusElement.textContent = "Login successful! Redirecting to Admin...";
    statusElement.className = "mt-3 text-success";

    setTimeout(() => {
      window.location.href = "admin.html";
    }, 700);
  } catch (error) {
    console.error("Login error:", error);
    statusElement.textContent =
      "Could not connect to the server. Make sure the backend is running.";
    statusElement.className = "mt-3 text-danger";
  }
}

// If user is already logged in, show a quick message
document.addEventListener("DOMContentLoaded", () => {
  if (typeof isAuthenticated === "function" && isAuthenticated()) {
    const statusElement = document.getElementById("loginStatus");
    if (statusElement) {
      statusElement.textContent = "You are already logged in.";
      statusElement.className = "mt-3 text-success";
    }
  }
});
