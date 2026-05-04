import { http } from "./http";

export const signUpApi = async (params) => {
  const { data } = await http.post("/sign-up", { ...params });
  return data;
};