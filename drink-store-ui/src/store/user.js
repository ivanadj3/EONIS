import { ref } from "vue";
import { clearCart } from "./cart";
import { jwtDecode } from "jwt-decode";

const USER_KEY = "user";

export const user = ref(loadUser());

function loadUser() {
  const data = localStorage.getItem(USER_KEY);
  return data ? JSON.parse(data) : {};
}

function saveUser() {
  localStorage.setItem(USER_KEY, JSON.stringify(user.value));
}

export function setUser(token) {
  const claims = jwtDecode(token)
  user.value = {
    jwt: token,
    role: claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
  };
  saveUser();
}

export function deleteUser() {
  user.value = {};
  localStorage.removeItem(USER_KEY);
  clearCart()
}

export function isLoggedIn() {
  return Object.keys(user.value).length !== 0;
}
