import { http } from "./http";

export const fetchProductsApi = async (params) => {
  const { data } = await http.get("/products", { params });
  return data;
};

export const fetchProductByIdApi = async (productId) => {
  const { data } = await http.get("/products/" + productId);
  return data;
};