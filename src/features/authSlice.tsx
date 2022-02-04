import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { User } from "../pages/Home/Home";
import type { RootState } from "./store";

export interface AuthState extends User {
  token: string;
}

const initialState: AuthState = {
  id: "",
  name: "",
  email: "",
  imageUrl: "",
  token: "",
};

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    createAuth: (_, action: PayloadAction<AuthState>) => {
      return action.payload;
    },
    logout: (state) => {
      return {
        id: "",
        name: "",
        email: "",
        imageUrl: "",
        token: "",
      };
    },
  },
});

export const { createAuth, logout } = authSlice.actions;

export default authSlice.reducer;
