import { http } from "./http";

export const makeOrderApi = async (params) => {
  const { data } = await http.post("/orders", params, {
    headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  });
  
  return data;
};