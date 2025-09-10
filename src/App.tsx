import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Upload from "./routes/Upload";
import Chat from "./routes/Chat";
import Explorer from "./routes/Explorer";
import Settings from "./routes/Settings";
import ErrorBoundary from "./components/ErrorBoundary";
import LoadingBoundary from "./components/LoadingBoundary";
import './App.css';

function App() {
  return (
    <Router>
      <div>
        <h1>Beta Bot Climbing Analytics</h1>
        <nav className="navbar">
          <Link to="/upload" className="nav-link">Upload</Link>
          <Link to="/chat" className="nav-link">Chat</Link>
          <Link to="/explorer" className="nav-link">Explorer</Link>
          <Link to="/settings" className="nav-link">Settings</Link>
        </nav>
        <div className="main-content">
          <ErrorBoundary>
            <LoadingBoundary>
              <Routes>
                <Route path="/upload" element={<Upload />} />
                <Route path="/chat" element={<Chat />} />
                <Route path="/explorer" element={<Explorer />} />
                <Route path="/settings" element={<Settings />} />
              </Routes>
            </LoadingBoundary>
          </ErrorBoundary>
        </div>
      </div>
    </Router>
  );
}

export default App;