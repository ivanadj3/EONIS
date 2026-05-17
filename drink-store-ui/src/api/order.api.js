import { user } from "../store/user";
import { http } from "./http";

export const makeOrderApi = async (params) => {
  const { data } = await http.post("/orders", params, {
    headers: {
        Authorization: `Bearer ${user.value.jwt}`
    }
  });

  return data;
};

export const getOrdersApi = async (params) => {
   const { data } = await http.get("/orders", {
    headers: {
        Authorization: `Bearer ${user.value.jwt}`
    }
  });

  return data;
};

export const getOrdersAdminApi = async (params) => {
   const { data } = await http.get("/orders/admin", {
    headers: {
        Authorization: `Bearer ${user.value.jwt}`
    }
  });

  return data;
};

export const getOrderByIdApi = async (id) => {
   const { data } = await http.get("/orders/" + id, {
    headers: {
        Authorization: `Bearer ${user.value.jwt}`
    }
  });

  return data;
};

export const deleteOrderByIdApi = async (id) => {
   const { data } = await http.delete("/orders/" + id, {
    headers: {
        Authorization: `Bearer ${user.value.jwt}`
    }
  });

  return data;
};