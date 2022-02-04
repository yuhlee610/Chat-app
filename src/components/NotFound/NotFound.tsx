import React from "react";
import { VscSearchStop } from "react-icons/vsc";
import './NotFound.scss'

const NotFound = () => {
  return (
    <div className="not-found">
      {/* <VscSearchStop /> */}
      <img src="https://chat.zalo.me/assets/search-empty.a19dba60677c95d6539d26d2dc363e4e.png" alt="" />
      <div>No result</div>
      <div>Try others keyword!</div>
    </div>
  );
};

export default NotFound;
