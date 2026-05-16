import { http } from "./http";

export const fetchProductsApi = async (params) => {
  const { data } = await http.get("/products", { params });
  return data;
};

export const fetchProductByIdApi = async (productId) => {
  const { data } = await http.get("/products/" + productId);
  return data;
};

export const postProductApi = async (body) => {
  const { data } = await http.post("/products", body);
  return data;
};

export const deleteProductApi = async (productId) => {
  const { data } = await http.delete("/products/" + productId);
  return data;
};

export const updateProductApi = async (productId, body) => {
  const { data } = await http.put("/products/" + productId, body);
  return data;
};