import React, { useEffect } from "react";
import "./Login.scss";
import { FcGoogle } from "react-icons/fc";
import GoogleLogin from "react-google-login";
import { useMutation } from "@apollo/client";
import { CREATE_ACCOUNT_IF_NOT_EXISTS } from "../../features/apolloClient";
import { AuthState, createAuth } from "../../features/authSlice";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../features/hooks";

const Login = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [authenticate] = useMutation(
    CREATE_ACCOUNT_IF_NOT_EXISTS,
    {
      onCompleted: (data) => {
        const { id, email, imageUrl, name } = data.createUserAndToken.user;
        const token = data.createUserAndToken.token;
        const auth: AuthState = { id, name, email, imageUrl, token };
        dispatch(createAuth(auth));
        navigate('/home')
      },
    }
  );

  useEffect(() => {
    let intervalId = setInterval(typeWriter, 600);
    return () => {
      clearInterval(intervalId);
    };
  }, []);

  const nameApp = "Chat App";
  let i = 0;
  let state: "grow" | "shrink" = "grow";
  const typeWriter = () => {
    if (i === nameApp.length) {
      state = "shrink";
    }
    if (i < 0) {
      state = "grow";
    }
    if (state === "grow") {
      document.querySelector(".name")!.innerHTML += nameApp.charAt(i);
      i++;
    }
    if (state === "shrink") {
      document.querySelector(".name")!.innerHTML =
        "<span>.</span>" + nameApp.substring(0, i);
      i--;
    }
  };

  const responseGoogle = (response: any) => {
    const { email, name, imageUrl } = response.profileObj;
    authenticate({
      variables: {
        userInfo: {
          email,
          name,
          imageUrl,
        },
      },
    });
  };

  return (
    <div className="login-wrapper">
      <div className="gradient"></div>
      <div className="overlay-triagle"></div>
      <div className="login-left">
        <img src="./MockUp.svg" alt="" />
        <img src="./iMessage App.svg" alt="" />
      </div>
      <div className="login-right">
        <div className="name">
          <span>.</span>
        </div>
        <GoogleLogin
          clientId={process.env.REACT_APP_GOOGLE_API_TOKEN!}
          render={(renderProps) => (
            <button
              type="button"
              className="btn-login"
              onClick={renderProps.onClick}
              disabled={renderProps.disabled}
            >
              <FcGoogle style={{ marginRight: 5 }} />
              Sign in with Google
            </button>
          )}
          onSuccess={responseGoogle}
          onFailure={responseGoogle}
          cookiePolicy="single_host_origin"
        />
      </div>
    </div>
  );
};

export default Login;
