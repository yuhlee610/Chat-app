import React, { useEffect } from "react";
import { setInterval } from "timers";
import "./Login.scss";

const Login = () => {
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

  return (
    <div className="login-wrapper">
      <div className="login-left">
        <img src="./MockUp.svg" alt="" />
        <img src="./iMessage App.svg" alt="" />
      </div>
      <div className="login-right">
        <div className="name">
          <span>.</span>
        </div>
      </div>
    </div>
  );
};

export default Login;
