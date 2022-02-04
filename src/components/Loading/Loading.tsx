import React from "react";
import "./Loading.scss";

interface LoadingProps {
  style?: React.CSSProperties;
}

export const Loading: React.FC<LoadingProps> = ({ style }) => {
  return (
    <div className="loadingio-spinner-interwind-ct4t00i5zbs" style={style}>
      <div className="ldio-owqr565b33">
        <div>
          <div>
            <div>
              <div></div>
            </div>
          </div>
          <div>
            <div>
              <div></div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
