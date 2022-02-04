import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { User, Group } from "../pages/Home/Home";
import { AuthState } from "./authSlice";

export interface Message {
  id: string;
  content: string;
  date: string;
  toUserOrGroup: User | Group;
  sendByUser: User;
}

export interface Conversation {
  id: string;
  messages: Message[];
}

const initialState: Conversation[] = [];

export const conversationsSlice = createSlice({
  name: "conversations",
  initialState,
  reducers: {
    addConversations: (state, action: PayloadAction<Conversation>) => {
      state.push(action.payload);
    },
    addMessageUserToUser: (state, action: PayloadAction<Message>) => {
      const index = state.findIndex(
        (conv) =>
          conv.id === action.payload.sendByUser.id ||
          conv.id === action.payload.toUserOrGroup.id
      );
      if (index !== -1) {
        state[index] = {
          id: state[index].id,
          messages: [action.payload, ...state[index].messages],
        };
      }
    },
    addMessageUserToGroup: (state, action: PayloadAction<Message>) => {
      const index = state.findIndex(
        (conv) => conv.id === action.payload.toUserOrGroup.id
      );
      if (index !== -1) {
        state[index] = {
          id: state[index].id,
          messages: [action.payload, ...state[index].messages],
        };
      }
    },
  },
});

export const { addConversations, addMessageUserToUser, addMessageUserToGroup } =
  conversationsSlice.actions;

export default conversationsSlice.reducer;
