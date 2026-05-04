import { http } from "./http";

export const loginApi = async (params) => {
  const { data } = await http.post("/auth/login", { ...params });
  return data;
};