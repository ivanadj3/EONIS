import { http } from "./http";

export const createCheckoutSessionApi = async (params) => {
  const { data } = await http.post("/stripe/session", { ...params }, {
     headers: {
        Authorization: `Bearer ${user.value.jwt}`
    }
  });
  return data;
};