import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Upload from "./routes/Upload";
import Chat from "./routes/Chat";
import Explorer from "./routes/Explorer";
import Settings from "./routes/Settings";
import ErrorBoundary from "./components/ErrorBoundary";
import LoadingBoundary from "./components/LoadingBoundary";

function App() {
  return (
    <Router>
      <nav>
        <Link to="/upload">Upload</Link>
        <Link to="/chat">Chat</Link>
        <Link to="/explorer">Explorer</Link>
        <Link to="/settings">Settings</Link>
      </nav>
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
    </Router>
  );
}

export default App;