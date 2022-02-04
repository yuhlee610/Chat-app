import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export interface Theme {
  conversationId: string;
  linearGradient: string;
}

const initialState: Theme[] = [];

export const themeSlice = createSlice({
  name: "theme",
  initialState,
  reducers: {
    updateTheme: (state, action: PayloadAction<Theme>) => {
      if (state.includes(action.payload)) {
        const index = state.indexOf(action.payload);
        state[index] = action.payload;
      } else {
        state.push(action.payload);
      }
    },
  },
});

export const { updateTheme } = themeSlice.actions;

export default themeSlice.reducer;
