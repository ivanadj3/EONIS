import { ref } from "vue";

const CART_KEY = "cart";

export const cartItems = ref(loadCart());

function loadCart() {
  const data = localStorage.getItem(CART_KEY);
  return data ? JSON.parse(data) : [];
}

function saveCart() {
  localStorage.setItem(CART_KEY, JSON.stringify(cartItems.value));
}

export function addToCart(product, quantity = 1) {
  const existing = cartItems.value.find(
    (p) => p.id === product.id
  );

  if (existing) {
    existing.quantity += quantity;
  } else {
    cartItems.value.push({
      ...product,
      quantity,
    });
  }

  saveCart();
}

export function removeFromCart(id) {
  cartItems.value = cartItems.value.filter(
    (p) => p.id !== id
  );

  saveCart();
}

export function clearCart() {
  cartItems.value = [];
  saveCart();
}

export function getCartCount() {
  return cartItems.value.reduce(
    (sum, item) => sum + item.quantity,
    0
  );
}