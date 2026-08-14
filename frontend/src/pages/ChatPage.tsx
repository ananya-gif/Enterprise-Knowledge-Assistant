import { useState } from 'react';
import { getHealth } from '../services/api';

function ChatPage() {
  const [message, setMessage] = useState('');

  const handleSend = async () => {
    try {
      const health = await getHealth();

      console.log(health);
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

        <p>You typed: {message}</p>

        <button type="button" onClick={handleSend}>
          Send
        </button>
      </div>
    </div>
  );
}

export default ChatPage;