import { useState } from 'react';
import {
  streamChatMessage,
  uploadDocument,
  type SourceReference,
} from '../services/api';

function ChatPage() {
  const [message, setMessage] = useState('');
  const [response, setResponse] = useState('');
  const [sources, setSources] = useState<SourceReference[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isUploading, setIsUploading] = useState(false);
  const [uploadMessage, setUploadMessage] = useState('');
  const [error, setError] = useState('');

  const handleSend = async () => {
  if (!message.trim() || isLoading) {
    return;
  }

  try {
    setIsLoading(true);
    setError('');
    setResponse('');
    setSources([]);

    await streamChatMessage(
      message,
      (chunk) => {
        setResponse((currentResponse) => currentResponse + chunk);
      },
      (newSources) => {
        setSources(newSources);
      },
    );
  } catch (error) {
    console.error(error);
    setError('Unable to get a response. Please try again.');
  } finally {
    setIsLoading(false);
  }
};

  const handleUpload = async (
    event: React.ChangeEvent<HTMLInputElement>,
  ) => {
    const file = event.target.files?.[0];

    if (!file) {
      return;
    }

    if (!file.name.toLowerCase().endsWith('.pdf')) {
      setError('Only PDF files are supported.');
      return;
    }

    try {
      setIsUploading(true);
      setError('');
      setUploadMessage('');

      const result = await uploadDocument(file);

      setUploadMessage(
        `${result.fileName} uploaded successfully. ${result.chunkCount} chunk(s) created.`,
      );
    } catch (error) {
      console.error(error);
      setError('Unable to upload the document. Please try again.');
    } finally {
      setIsUploading(false);
      event.target.value = '';
    }
  };

  return (
    <div className="app-shell">
      <header className="app-header">
        <div>
          <div className="brand">
            <span className="brand-icon">✦</span>
            Enterprise Knowledge Assistant
          </div>

          <p className="subtitle">
            Search your enterprise documents using AI-powered semantic retrieval.
          </p>
        </div>
      </header>

      <main className="main-content">
        <section className="upload-card">
          <div className="section-header">
            <div>
              <h2>Knowledge Base</h2>
              <p>Upload a PDF to add it to the knowledge base.</p>
            </div>

            <span className="status-badge">PDF</span>
          </div>

          <label className="upload-area">
            <span className="upload-icon">↑</span>

            <span className="upload-title">
              {isUploading ? 'Processing document...' : 'Upload a PDF'}
            </span>

            <span className="upload-description">
              {isUploading
                ? 'Extracting text and generating embeddings.'
                : 'Choose an enterprise document to get started.'}
            </span>

            <input
              type="file"
              accept=".pdf"
              onChange={handleUpload}
              disabled={isUploading}
            />
          </label>

          {uploadMessage && (
            <div className="success-message">
              ✓ {uploadMessage}
            </div>
          )}
        </section>

        <section className="chat-card">
          <div className="chat-header">
            <div>
              <h2>Ask the Assistant</h2>
              <p>
                Ask questions about the documents in your knowledge base.
              </p>
            </div>

            <span className="online-status">
              <span className="status-dot" />
              Ready
            </span>
          </div>

          {!response && !isLoading && !error && (
            <div className="empty-state">
              <div className="empty-icon">✦</div>
              <h3>How can I help?</h3>
              <p>
                Ask about policies, procedures, benefits, security, travel,
                or any other uploaded enterprise content.
              </p>
            </div>
          )}

          {(response || isLoading || error) && (
            <div className="response-area">
              {isLoading && !response && (
                <div className="assistant-message">
                  <div className="message-avatar">AI</div>

                  <div className="message-content">
                    <span className="message-label">Assistant</span>

                    <div className="typing-indicator">
                      <span />
                      <span />
                      <span />
                    </div>
                  </div>
                </div>
              )}

              {response && (
                <div className="assistant-message">
                  <div className="message-avatar">AI</div>

                  <div className="message-content">
                    <span className="message-label">Assistant</span>

                    <p className="answer">{response}</p>

                    {sources.length > 0 && (
                      <div className="sources">
                        <div className="sources-title">
                          Sources
                        </div>

                        {sources.map((source, index) => (
                          <div
                            className="source-item"
                            key={`${source.fileName}-${source.chunkIndex}-${index}`}
                          >
                            <span className="source-icon">📄</span>

                            <div>
                              <div className="source-file">
                                {source.fileName}
                              </div>

                              <div className="source-meta">
                                Page {source.pageNumber} · Chunk{' '}
                                {source.chunkIndex}
                              </div>
                            </div>
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                </div>
              )}

              {error && (
                <div className="error-message">
                  {error}
                </div>
              )}
            </div>
          )}

          <div className="input-area">
            <input
              type="text"
              placeholder="Ask a question about your documents..."
              value={message}
              disabled={isLoading}
              onChange={(event) => setMessage(event.target.value)}
              onKeyDown={(event) => {
                if (event.key === 'Enter') {
                  handleSend();
                }
              }}
            />

            <button
              type="button"
              onClick={handleSend}
              disabled={isLoading || !message.trim()}
              className="send-button"
            >
              {isLoading ? '...' : 'Send'}
            </button>
          </div>
        </section>
      </main>

      <footer className="app-footer">
        Enterprise Knowledge Assistant · RAG-powered document search
      </footer>
    </div>
  );
}

export default ChatPage;