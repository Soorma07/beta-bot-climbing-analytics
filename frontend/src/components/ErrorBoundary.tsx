import React, { ReactNode } from "react";

interface ErrorBoundaryProps {
  children: ReactNode;
}

class ErrorBoundary extends React.Component<ErrorBoundaryProps> {
  state = { hasError: false };

  static getDerivedStateFromError() {
    return { hasError: true };
  }

  render() {
    if (this.state.hasError) return <div>Error occurred.</div>;
    return this.props.children;
  }
}

export default ErrorBoundary;