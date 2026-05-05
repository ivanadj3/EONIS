import { ref } from "vue";
import { clearCart } from "./cart";

const USER_KEY = "user";

export const user = ref(loadUser());

function loadUser() {
  const data = localStorage.getItem(USER_KEY);
  return data ? JSON.parse(data) : {};
}

function saveUser() {
  localStorage.setItem(USER_KEY, JSON.stringify(user.value));
}

export function setUser(usr) {
    user.value = usr;
    saveUser();
}

export function deleteUser() {
  user.value = {};
  localStorage.removeItem(USER_KEY);
  clearCart()
}
