import React, { useState } from "react";

const Upload = () => {
  const [file, setFile] = useState<File | null>(null);
  const [error, setError] = useState<string>("");
  const [uploading, setUploading] = useState(false);
  const [progress, setProgress] = useState<number>(0);
  const [success, setSuccess] = useState(false);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setError("");
    setSuccess(false);
    const selected = e.target.files?.[0] || null;
    if (selected && selected.type !== "text/csv") {
      setError("Please select a CSV file.");
      setFile(null);
    } else {
      setFile(selected);
    }
  };

  const handleUpload = async () => {
    if (!file) {
      setError("No file selected.");
      return;
    }
    setUploading(true);
    setProgress(0);
    setError("");
    setSuccess(false);
    try {
      // Step 1: Get pre-signed URL and jobId from API
      const res = await fetch("/api/uploads/presigned-url", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ filename: file.name })
      });
      if (!res.ok) throw new Error("Failed to get upload URL");
      const { url, jobId } = await res.json();

      // Step 2: Upload file to S3 using the URL
      const xhr = new XMLHttpRequest();
      xhr.open("PUT", url, true);
      xhr.upload.onprogress = (e) => {
        if (e.lengthComputable) {
          setProgress(Math.round((e.loaded / e.total) * 100));
        }
      };
      xhr.onload = () => {
        setUploading(false);
        if (xhr.status === 200) {
          setSuccess(true);
        } else {
          setError("Upload failed.");
        }
      };
      xhr.onerror = () => {
        setUploading(false);
        setError("Upload failed.");
      };
      xhr.send(file);
    } catch (err) {
      setError("Upload failed.");
      setUploading(false);
    }
  };

  return (
    <div>
      <h2>Upload CSV</h2>
      <input type="file" accept=".csv" onChange={handleFileChange} />
      <button onClick={handleUpload} disabled={uploading || !file}>Upload</button>
      {error && <div style={{ color: "red" }}>{error}</div>}
      {uploading && <div>Uploading... {progress}%</div>}
      {success && <div style={{ color: "green" }}>Upload successful!</div>}
    </div>
  );
};

export default Upload;