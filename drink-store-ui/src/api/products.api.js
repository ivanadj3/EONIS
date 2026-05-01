import { http } from "./http";

export const fetchProductsApi = async (params) => {
  const { data } = await http.get("/products", { params });
  return data;
};