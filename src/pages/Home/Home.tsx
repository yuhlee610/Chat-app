import React, { useEffect, useState } from "react";
import MenuItem from "../../components/MenuItem/MenuItem";
import "./Home.scss";
import { menuIcons, themeList } from "../../data/data";
import { SubmitHandler, useForm } from "react-hook-form";
import { AiOutlineUsergroupAdd } from "react-icons/ai";
import RoundedAvatar from "../../components/RoundedAvatar/RoundedAvatar";
import ConversationItem from "../../components/ConversationItem/ConversationItem";
import { useAppDispatch, useAppSelector } from "../../features/hooks";
import { useMutation, useQuery, useSubscription } from "@apollo/client";
import {
  ADD_MEMBERS,
  CREATE_GROUP,
  CREATE_MESSAGE,
  EXIT_GROUP,
  GET_CONTACT_GROUPS,
  GET_CONTACT_USERS,
  GET_MEMBERS,
  GROUP_CREATED_SUBSCRIPTION,
  GROUP_EXITED_SUBSCRIPTION,
  MEMBERS_ADDED_SUBSCRIPTION,
  MEMBERS_REMOVED_SUBSCRIPTION,
  MESSAGE_CREATED_SUBSCRIPTION,
  REMOVE_MEMBERS,
} from "../../features/apolloClient";
import { Loading } from "../../components/Loading/Loading";
import NotFound from "../../components/NotFound/NotFound";
import ChatHeader from "../../components/ChatHeader/ChatHeader";
import { BsFillRecordCircleFill } from "react-icons/bs";
import Modal from "../../components/Modal/Modal";
import { animated, useTransition } from "react-spring";
import Conversation from "../../components/Conversation/Conversation";
import { updateTheme } from "../../features/themeSlice";
import {
  addMessageUserToUser,
  addMessageUserToGroup,
} from "../../features/conversationsSlice";
import { CgDetailsMore } from "react-icons/cg";
import { HiOutlineKey } from "react-icons/hi";
import { BiExit } from "react-icons/bi";
import {
  addContactGroup,
  exitContactGroup,
  fetchContactGroups,
  removeOrUpdateGroup,
  updateContactGroup,
} from "../../features/contactGroupsSlice";
import { TiUserDeleteOutline } from "react-icons/ti";
import { AiOutlineUserAdd } from "react-icons/ai";
import { logout } from "../../features/authSlice";

enum Type {
  TO_USER,
  TO_GROUP,
}

interface SearchInput {
  searchValue: string;
}

interface GroupInput {
  groupName: string;
}

interface MessageInput {
  message: string;
}

export interface User {
  id: string;
  name: string;
  imageUrl: string;
  email: string;
}

interface LatestMessage {
  content: string;
}

export interface Group {
  id: string;
  name: string;
  host: User;
  groupUsers?: MemberInGroup[];
}

interface MemberInGroup {
  user: User;
}

export interface ContactUser {
  user: User;
  latestMessage: LatestMessage;
}

export interface ContactGroup {
  group: Group;
  latestMessage: LatestMessage;
  numOfMembers?: number;
}

export const isContactGroup = (
  item: ContactGroup | ContactUser | User
): item is ContactGroup => {
  return (item as ContactGroup).group !== undefined;
};

export const isContactUser = (
  item: ContactGroup | ContactUser | User
): item is ContactUser => {
  return (item as ContactUser).user !== undefined;
};

export const isUser = (
  item: ContactGroup | ContactUser | User
): item is User => {
  return (item as User).email !== undefined;
};

const defaultAnimation = {
  from: { opacity: 0 },
  enter: { opacity: 1, zIndex: 2 },
  leave: { opacity: 0, zIndex: -2 },
};

const Home = () => {
  const dispatch = useAppDispatch();

  const userInfo = useAppSelector((state) => state.auth);

  const contactGroupsData: ContactGroup[] = useAppSelector(
    (state) => state.contactGroups
  );

  // toggle
  const [toggleMenu, setToggleMenu] = useState<string>("contactedUsers");
  const [toggleConversation, setToggleConversation] = useState<string>();

  const [selectedConversation, setSelectedConversation] = useState<
    ContactUser | ContactGroup | User
  >();

  // fetch data
  const { loading: loadingMembers, data: membersData } = useQuery(GET_MEMBERS);
  const {
    loading: loadingContactUsers,
    data: contactUsersData,
    refetch: refetchContactUser,
  } = useQuery(GET_CONTACT_USERS, {
    variables: {
      id: userInfo.id,
    },
  });
  const { loading: loadingContactGroups, refetch: refetchContactGroup } =
    useQuery(GET_CONTACT_GROUPS, {
      variables: {
        id: userInfo.id,
      },
      onCompleted: (data) => {
        dispatch(fetchContactGroups(data.contactGroups));
      },
      fetchPolicy: "no-cache",
    });

  // update data
  const [createGroup, { loading: groupCreateLoading }] =
    useMutation(CREATE_GROUP);

  const [createMessage] = useMutation(CREATE_MESSAGE);

  const [exitGroup] = useMutation(EXIT_GROUP, {
    refetchQueries: [
      {
        query: GET_CONTACT_GROUPS,
        variables: {
          id: userInfo.id,
        },
      },
    ],
  });

  const [removeMembers, { loading: removeMembersLoading }] =
    useMutation(REMOVE_MEMBERS);

  const [addMembers, { loading: addMembersLoading }] = useMutation(ADD_MEMBERS);

  // subscription
  useSubscription(GROUP_CREATED_SUBSCRIPTION, {
    variables: {
      id: userInfo.id,
    },
    onSubscriptionData: () => {
      refetchContactGroup();
    },
  });

  useSubscription(MESSAGE_CREATED_SUBSCRIPTION, {
    variables: {
      id: userInfo.id,
    },
    onSubscriptionData: (data) => {
      const tempObj = data.subscriptionData.data.messageCreated;
      if (data.subscriptionData.data.messageCreated.toUser) {
        tempObj.toUserOrGroup = tempObj.toUser;
        delete tempObj["toGroup"];
        delete tempObj["toUser"];
        dispatch(addMessageUserToUser(tempObj));
        refetchContactUser();
      } else {
        tempObj.toUserOrGroup = tempObj.toGroup;
        delete tempObj["toGroup"];
        delete tempObj["toUser"];
        dispatch(addMessageUserToGroup(tempObj));
        refetchContactGroup();
      }
    },
  });

  useSubscription(GROUP_EXITED_SUBSCRIPTION, {
    variables: {
      userId: userInfo.id,
    },
    onSubscriptionData: (data) => {
      const newContactGroup: ContactGroup =
        data.subscriptionData.data.groupExited;
      dispatch(updateContactGroup(newContactGroup));
      if (
        isContactGroup(selectedConversation) &&
        selectedConversation.group.id === newContactGroup.group.id
      ) {
        setSelectedConversation(newContactGroup);
      }
    },
  });

  useSubscription(MEMBERS_REMOVED_SUBSCRIPTION, {
    variables: {
      userId: userInfo.id,
    },
    onSubscriptionData: (data) => {
      const newContactGroup: ContactGroup =
        data.subscriptionData.data.groupRemoveMembers;
      dispatch(removeOrUpdateGroup(newContactGroup))
        .unwrap()
        .then((result) => {
          if (
            isContactGroup(selectedConversation) &&
            selectedConversation.group.id === newContactGroup.group.id
          ) {
            if (result === "remove") {
              setSelectedConversation(undefined);
            } else {
              setSelectedConversation(newContactGroup);
            }
          }
        });
    },
  });

  useSubscription(MEMBERS_ADDED_SUBSCRIPTION, {
    variables: {
      userId: userInfo.id,
    },
    onSubscriptionData: (data) => {
      const newContactGroup: ContactGroup =
        data.subscriptionData.data.groupAddMembers;
      if (
        contactGroupsData.findIndex(
          (ct) => ct.group.id === newContactGroup.group.id
        ) === -1
      ) {
        console.log("add");
        dispatch(addContactGroup(newContactGroup));
      } else {
        console.log("update");
        dispatch(updateContactGroup(newContactGroup));
        if (
          isContactGroup(selectedConversation) &&
          selectedConversation.group.id === newContactGroup.group.id
        ) {
          setSelectedConversation(newContactGroup);
        }
      }
    },
  });

  const { register: registerSearch, watch: watchSearch } =
    useForm<SearchInput>();
  const { register: registerGroup, getValues: getValueGroup } =
    useForm<GroupInput>();
  const {
    register: registerMessage,
    handleSubmit,
    setValue: setValueMessage,
  } = useForm<MessageInput>();

  // state searchValue
  const watchSeacrchValue = watchSearch("searchValue", "");

  // theme modal
  const [openThemeModal, setOpenThemeModal] = useState<boolean>(false);
  const themeTransition = useTransition(openThemeModal, defaultAnimation);

  // remove member modal
  const [openRemoveMemberModal, setOpenRemoveMemberModal] =
    useState<boolean>(false);
  const removeMemberTransition = useTransition(
    openRemoveMemberModal,
    defaultAnimation
  );

  // create group modal
  const [openCreateGroupModal, setOpenCreateGroupModal] =
    useState<boolean>(false);
  const createGroupTransition = useTransition(
    openCreateGroupModal,
    defaultAnimation
  );

  // add members group modal
  const [openAddMembersModal, setOpenAddMembersModal] =
    useState<boolean>(false);
  const addMembersTransition = useTransition(
    openAddMembersModal,
    defaultAnimation
  );

  // members in group when create
  // members who chosen to remove from group
  const [membersSelected, setMembersSelected] = useState<string[]>([]);

  useEffect(() => {
    if (
      !openCreateGroupModal ||
      !openRemoveMemberModal ||
      !openAddMembersModal
    ) {
      setMembersSelected([]);
    }
  }, [openCreateGroupModal, openRemoveMemberModal, openAddMembersModal]);

  const onSubmit: SubmitHandler<MessageInput> = (data) => {
    createMessage({
      variables: {
        content: data.message,
        sendByUserId: userInfo.id,
        groupOrUserId: isUser(selectedConversation)
          ? selectedConversation.id
          : isContactUser(selectedConversation)
          ? selectedConversation.user.id
          : selectedConversation.group.id,
        type: isContactGroup(selectedConversation)
          ? Type[Type.TO_GROUP]
          : Type[Type.TO_USER],
      },
    });
    setValueMessage("message", "");
  };

  const exitGroupHandler = () => {
    const groupId =
      isContactGroup(selectedConversation) && selectedConversation.group.id;
    exitGroup({
      variables: {
        userId: userInfo.id,
        groupId: groupId,
      },
      onCompleted: () => {
        dispatch(exitContactGroup(groupId));
        setSelectedConversation(undefined);
      },
    });
  };

  const clickMemberHandler = (id: string): void => {
    if (membersSelected.includes(id)) {
      setMembersSelected(membersSelected.filter((member) => member !== id));
    } else {
      setMembersSelected((prev) => [...prev, id]);
    }
  };

  const clickMenuItemHandler = (id: string): void => {
    setToggleMenu(id);
    setToggleConversation(undefined);
  };

  const createGroupHandler = () => {
    createGroup({
      variables: {
        name: getValueGroup("groupName"),
        hostId: userInfo.id,
        groupUserIds: [...membersSelected, userInfo.id],
      },
    });
    setOpenCreateGroupModal(false);
  };

  const removeMembersHandler = () => {
    if (membersSelected.length < 1) return;
    if (isContactGroup(selectedConversation)) {
      removeMembers({
        variables: {
          userIds: membersSelected,
          groupId: selectedConversation.group.id,
        },
      });
      setOpenRemoveMemberModal(false);
    }
  };

  const addMembersHandler = () => {
    if (membersSelected.length < 1) return;
    if (isContactGroup(selectedConversation)) {
      addMembers({
        variables: {
          userIds: membersSelected,
          groupId: selectedConversation.group.id,
        },
      });
      setOpenAddMembersModal(false);
    }
  };

  return (
    <div className="home-wrapper">
      <div className="menu">
        <MenuItem title={userInfo.name}>
          <RoundedAvatar src={userInfo.imageUrl} />
        </MenuItem>
        {menuIcons.map((ele) => (
          <MenuItem
            key={ele.id}
            onToggle={() => clickMenuItemHandler(ele.id)}
            isToggle={ele.id === toggleMenu}
            title={ele.title}
          >
            {ele.component}
          </MenuItem>
        ))}
        <MenuItem onToggle={() => dispatch(logout)} title="Logout">
          <BiExit />
        </MenuItem>
      </div>
      <div className="contact">
        <div className="contact-search">
          <input
            autoComplete="off"
            placeholder="Search here..."
            defaultValue={""}
            {...registerSearch("searchValue", {
              required: true,
            })}
          />
          <div
            onClick={() => {
              setOpenCreateGroupModal((prev) => !prev);
            }}
          >
            <AiOutlineUsergroupAdd />
          </div>
        </div>
        <div className="conversation-list">
          {toggleMenu === "contactedUsers" &&
            contactUsersData?.contactUsers
              .filter(({ user }: ContactUser) =>
                user.name.includes(watchSeacrchValue)
              )
              .map(({ user, latestMessage }: ContactUser) => (
                <ConversationItem
                  key={user.id}
                  src={user.imageUrl}
                  name={user.name}
                  latestMessage={latestMessage.content}
                  onToggle={() => {
                    setToggleConversation(user.id);
                    setSelectedConversation({ user, latestMessage });
                  }}
                  isToggle={user.id === toggleConversation}
                />
              ))}

          {toggleMenu === "contactedGroups" &&
            contactGroupsData
              .filter(({ group }: ContactGroup) =>
                group.name.includes(watchSeacrchValue)
              )
              .map(({ group, latestMessage, numOfMembers }: ContactGroup) => (
                <ConversationItem
                  key={group.id}
                  src={`https://ui-avatars.com/api/?length=1&&name=${group.name}&&background=f44711&&color=ffffff`}
                  name={group.name}
                  latestMessage={latestMessage.content}
                  onToggle={() => {
                    setToggleConversation(group.id);
                    setSelectedConversation({
                      group,
                      latestMessage,
                      numOfMembers,
                    });
                  }}
                  isToggle={group.id === toggleConversation}
                />
              ))}

          {toggleMenu === "members" &&
            membersData?.users
              .filter((user: User) => user.name.includes(watchSeacrchValue))
              .map((user: User) => (
                <ConversationItem
                  key={user.id}
                  src={user.imageUrl}
                  name={user.name}
                  onToggle={() => {
                    setToggleConversation(user.id);
                    setSelectedConversation(user);
                  }}
                  isToggle={user.id === toggleConversation}
                />
              ))}

          {toggleMenu === "members" &&
            membersData?.users.filter((user: User) =>
              user.name.includes(watchSeacrchValue)
            ).length === 0 && <NotFound />}
          {toggleMenu === "contactedUsers" &&
            contactUsersData?.contactUsers.filter(({ user }: ContactUser) =>
              user.name.includes(watchSeacrchValue)
            ).length === 0 && <NotFound />}
          {toggleMenu === "contactedGroups" &&
            contactGroupsData.filter(({ group }: ContactGroup) =>
              group.name.includes(watchSeacrchValue)
            ).length === 0 && <NotFound />}

          {(loadingContactGroups || loadingContactUsers || loadingMembers) && (
            <Loading />
          )}
        </div>
      </div>
      <div className="chat">
        {selectedConversation && isContactUser(selectedConversation) && (
          <ChatHeader
            src={selectedConversation.user.imageUrl}
            name={selectedConversation.user.name}
            email={selectedConversation.user.email}
          />
        )}
        {selectedConversation && isContactGroup(selectedConversation) && (
          <ChatHeader
            src={`https://ui-avatars.com/api/?length=1&&name=${selectedConversation.group.name}&&background=f44711&&color=ffffff`}
            name={selectedConversation.group.name}
            numOfMembers={selectedConversation.numOfMembers}
          />
        )}
        {selectedConversation && isUser(selectedConversation) && (
          <ChatHeader
            src={selectedConversation.imageUrl}
            name={selectedConversation.name}
            email={selectedConversation.email}
          />
        )}
        <div className="messages-container">
          {selectedConversation ? (
            <Conversation
              userOrGroup={
                isUser(selectedConversation)
                  ? selectedConversation
                  : isContactUser(selectedConversation)
                  ? selectedConversation.user
                  : selectedConversation.group
              }
            />
          ) : (
            <div className="welcome">
              <div className="greeting">
                Welcome to <span>Chat App</span>
              </div>
              <div className="image-wrapper">
                <img src="welcome.jpg" />
              </div>
            </div>
          )}
        </div>
        {selectedConversation && (
          <div className="message-input">
            <form onSubmit={handleSubmit(onSubmit)}>
              <input
                autoComplete="off"
                placeholder={"Aa"}
                {...registerMessage("message", {
                  required: true,
                  maxLength: 1000,
                })}
              />
            </form>
          </div>
        )}
      </div>
      {selectedConversation && (
        <div className="chat-info">
          {isContactUser(selectedConversation) && (
            <ChatHeader
              src={selectedConversation.user.imageUrl}
              name={selectedConversation.user.name}
              email={selectedConversation.user.email}
              direction="vertical"
            />
          )}
          {isContactGroup(selectedConversation) && (
            <ChatHeader
              src={`https://ui-avatars.com/api/?length=1&&name=${selectedConversation.group.name}&&background=f44711&&color=ffffff`}
              name={selectedConversation.group.name}
              numOfMembers={selectedConversation.numOfMembers}
              direction="vertical"
            />
          )}
          {isUser(selectedConversation) && (
            <ChatHeader
              src={selectedConversation.imageUrl}
              name={selectedConversation.name}
              email={selectedConversation.email}
              direction="vertical"
            />
          )}
          {
            <ul className="options">
              <li
                onClick={() => {
                  setOpenThemeModal((prev) => !prev);
                }}
              >
                <BsFillRecordCircleFill /> Change theme
              </li>
              {isContactGroup(selectedConversation) && (
                <>
                  <li>
                    <CgDetailsMore /> Members
                  </li>
                  <li style={{ flexDirection: "column" }}>
                    {isContactGroup(selectedConversation) &&
                      selectedConversation.group.groupUsers.map((member) => (
                        <ConversationItem
                          key={member.user.id}
                          src={member.user.imageUrl}
                          name={member.user.name}
                        />
                      ))}
                  </li>
                  <li>
                    <HiOutlineKey /> Host
                  </li>
                  <li>
                    {
                      <ConversationItem
                        src={selectedConversation.group.host.imageUrl}
                        name={selectedConversation.group.host.name}
                      />
                    }
                  </li>
                  {userInfo.id !== selectedConversation.group.host.id && (
                    <li onClick={exitGroupHandler}>
                      <BiExit /> Exit
                    </li>
                  )}
                  {userInfo.id === selectedConversation.group.host.id && (
                    <>
                      <li onClick={() => setOpenRemoveMemberModal(true)}>
                        <TiUserDeleteOutline /> Remove
                      </li>
                      <li onClick={() => setOpenAddMembersModal(true)}>
                        <AiOutlineUserAdd /> Add more
                      </li>
                    </>
                  )}
                </>
              )}
            </ul>
          }
        </div>
      )}

      {selectedConversation &&
        themeTransition(
          (style, modal) =>
            modal && (
              <animated.div style={style} className="modal-wrapper">
                <Modal
                  title="Colors"
                  openModal={openThemeModal}
                  onToggleModal={() => {
                    setOpenThemeModal((prev) => !prev);
                  }}
                >
                  <ul className="theme-list">
                    {themeList.map((theme, index) => (
                      <li
                        className="theme-item-wrapper"
                        key={index}
                        onClick={() => {
                          dispatch(
                            updateTheme({
                              conversationId: isUser(selectedConversation)
                                ? selectedConversation.id
                                : isContactUser(selectedConversation)
                                ? selectedConversation.user.id
                                : selectedConversation.group.id,
                              linearGradient: theme
                                .replace("circle at center 75%, ", "")
                                .replace("radial", "linear"),
                            })
                          );
                          setOpenThemeModal((prev) => !prev);
                        }}
                      >
                        <div
                          className="theme-item"
                          style={{ backgroundImage: theme }}
                        ></div>
                      </li>
                    ))}
                  </ul>
                </Modal>
              </animated.div>
            )
        )}

      {createGroupTransition(
        (style, modal) =>
          modal && (
            <animated.div style={style}>
              <Modal
                title="Create group"
                openModal={openCreateGroupModal}
                onToggleModal={() => {
                  setOpenCreateGroupModal((prev) => !prev);
                }}
              >
                <div className="create-group-wrapper">
                  <input
                    autoComplete="off"
                    className="input-group-name"
                    placeholder="Name here..."
                    defaultValue=""
                    {...registerGroup("groupName", {
                      required: true,
                    })}
                  />
                  <div className="body-title">Members</div>
                  <div className="list-members">
                    {membersData?.users
                      .filter((user: User) => user.id !== userInfo.id)
                      .map((user: User) => (
                        <ConversationItem
                          key={user.id}
                          src={user.imageUrl}
                          name={user.name}
                          onToggle={() => {
                            clickMemberHandler(user.id);
                          }}
                          isToggle={membersSelected.includes(user.id)}
                        />
                      ))}
                  </div>
                  <button
                    onClick={createGroupHandler}
                    disabled={
                      (getValueGroup("groupName") &&
                        getValueGroup("groupName").length === 0) ||
                      membersSelected.length < 2
                    }
                  >
                    {groupCreateLoading ? "Creating..." : "Create"}
                  </button>
                </div>
              </Modal>
            </animated.div>
          )
      )}

      {removeMemberTransition(
        (style, modal) =>
          modal && (
            <animated.div style={style}>
              <Modal
                title="Remove members"
                openModal={openRemoveMemberModal}
                onToggleModal={() => {
                  setOpenRemoveMemberModal((prev) => !prev);
                }}
              >
                <div className="remove-members-wrapper">
                  <div className="body-title">Members</div>
                  <div className="list-members">
                    {isContactGroup(selectedConversation) &&
                      selectedConversation.group.groupUsers
                        .filter((member) => member.user.id !== userInfo.id)
                        .map((member) => (
                          <ConversationItem
                            key={member.user.id}
                            src={member.user.imageUrl}
                            name={member.user.name}
                            onToggle={() => {
                              clickMemberHandler(member.user.id);
                            }}
                            isToggle={membersSelected.includes(member.user.id)}
                          />
                        ))}
                    {isContactGroup(selectedConversation) &&
                      selectedConversation.group.groupUsers.filter(
                        (member) => member.user.id !== userInfo.id
                      ).length === 0 &&
                      "No member is available to remove."}
                  </div>
                  <button
                    onClick={removeMembersHandler}
                    disabled={membersSelected.length < 1}
                  >
                    {removeMembersLoading ? "Removing..." : "Remove"}
                  </button>
                </div>
              </Modal>
            </animated.div>
          )
      )}

      {addMembersTransition(
        (style, modal) =>
          modal && (
            <animated.div style={style}>
              <Modal
                title="Remove members"
                openModal={openAddMembersModal}
                onToggleModal={() => {
                  setOpenAddMembersModal((prev) => !prev);
                }}
              >
                <div className="remove-members-wrapper">
                  <div className="body-title">Members</div>
                  <div className="list-members">
                    {membersData?.users
                      .filter(
                        (user) =>
                          isContactGroup(selectedConversation) &&
                          selectedConversation.group?.groupUsers.findIndex(
                            (member) => member.user.id === user.id
                          ) === -1
                      )
                      .map((user) => (
                        <ConversationItem
                          key={user.id}
                          src={user.imageUrl}
                          name={user.name}
                          onToggle={() => {
                            clickMemberHandler(user.id);
                          }}
                          isToggle={membersSelected.includes(user.id)}
                        />
                      ))}
                    {membersData?.users.filter(
                      (user) =>
                        isContactGroup(selectedConversation) &&
                        selectedConversation.group?.groupUsers.findIndex(
                          (member) => member.user.id === user.id
                        ) === -1
                    ).length === 0 && "No member is available to add."}
                  </div>
                  <button
                    onClick={addMembersHandler}
                    disabled={membersSelected.length < 1}
                  >
                    {addMembersLoading ? "Adding..." : "Add"}
                  </button>
                </div>
              </Modal>
            </animated.div>
          )
      )}
    </div>
  );
};

export default Home;
