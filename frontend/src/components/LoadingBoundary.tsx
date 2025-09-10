import React, { useState, useEffect } from "react";

const LoadingBoundary = ({ children }: { children: React.ReactNode }) => {
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(false);
  }, []);

  return loading ? <div>Loading...</div> : <>{children}</>;
};

export default LoadingBoundary;