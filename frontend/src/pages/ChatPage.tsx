import { useState } from 'react';
import { streamChatMessage } from '../services/api';

function ChatPage() {
  const [message, setMessage] = useState('');
  const [response, setResponse] = useState('');

  const handleSend = async () => {
    try {
      setResponse('');

      await streamChatMessage(message, (chunk) => {
        setResponse((currentResponse) => currentResponse + chunk);
      });
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <h1>Enterprise Knowledge Assistant</h1>

      <p>Ask questions about your documents.</p>

      <div>
        <input
          type="text"
          placeholder="Ask a question..."
          value={message}
          onChange={(event) => setMessage(event.target.value)}
        />

        <button type="button" onClick={handleSend}>
          Send
        </button>

        <p>You typed: {message}</p>

        <p>Assistant: {response}</p>
      </div>
    </div>
  );
}

export default ChatPage;