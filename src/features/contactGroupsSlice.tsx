import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ContactGroup } from "../pages/Home/Home";
import { AuthState } from "./authSlice";

const initialState: ContactGroup[] = [];

export const removeOrUpdateGroup = createAsyncThunk(
  "contactGroups/removeOrUpdateGroup",
  (updateGroup: ContactGroup, { getState, dispatch }) => {
    const { auth } = getState() as { auth: AuthState };
    const checkMemberInGroup = updateGroup.group.groupUsers.findIndex(
      (member) => member.user.id == auth.id
    );
    if (checkMemberInGroup !== -1) {
      dispatch(updateContactGroup(updateGroup));
      return "update";
    } else {
      dispatch(removeGroup(updateGroup));
      return "remove";
    }
  }
);

const contactGroupsSlice = createSlice({
  name: "contactGroups",
  initialState,
  reducers: {
    fetchContactGroups: (_, action: PayloadAction<ContactGroup[]>) => {
      return action.payload;
    },
    exitContactGroup: (state, action: PayloadAction<string>) => {
      return state.filter((ct) => ct.group.id !== action.payload);
    },
    updateContactGroup: (state, action: PayloadAction<ContactGroup>) => {
      const index = state.findIndex(
        (ct) => ct.group.id === action.payload.group.id
      );
      state[index] = action.payload;
    },
    removeGroup: (state, action: PayloadAction<ContactGroup>) => {
      return state.filter((ct) => ct.group.id !== action.payload.group.id);
    },
    addContactGroup: (state, action: PayloadAction<ContactGroup>) => {
      state.push(action.payload)
    }
  },
});

export const {
  fetchContactGroups,
  exitContactGroup,
  updateContactGroup,
  removeGroup,
  addContactGroup
} = contactGroupsSlice.actions;

export default contactGroupsSlice.reducer;
