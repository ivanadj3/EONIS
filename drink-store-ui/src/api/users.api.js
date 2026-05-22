import { user } from "../store/user";
import { http } from "./http";

export const signUpApi = async (params) => {
  const { data } = await http.post("/sign-up", { ...params });
  return data;
};

export const fetchUsersApi = async () => {
  const { data } = await http.get("/users/", {
      headers: {
          Authorization: `Bearer ${user.value.jwt}`
      }
    });
  return data;
};

export const deleteUserByIdApi = async (id) => {
  const { data } = await http.delete("/users/" + id, {
      headers: {
          Authorization: `Bearer ${user.value.jwt}`
      }
    });
  return data;
};

export const changePasswordApi = async (body) => {
  const { data } = await http.post("/change-password", body, {
      headers: {
          Authorization: `Bearer ${user.value.jwt}`
      }
    });
  return data;
};