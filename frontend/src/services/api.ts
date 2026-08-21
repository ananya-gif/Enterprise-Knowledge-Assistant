const API_BASE_URL = 'http://localhost:5107';


export interface ChatResponse {
  answer: string;
}

export async function sendChatMessage(message: string): Promise<ChatResponse> {
  const response = await fetch('http://localhost:5107/api/chat', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      message: message,
    }),
  });

  if (!response.ok) {
    throw new Error('Failed to send chat message');
  }

  return response.json();
}

export async function streamChatMessage(
  message: string,
  onChunk: (chunk: string) => void,
): Promise<void> {
  const response = await fetch('http://localhost:5107/api/chat/stream', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ message }),
  })

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`)
  }

  if (!response.body) {
    throw new Error('Response body is not available.')
  }

  const reader = response.body.getReader()
  const decoder = new TextDecoder('utf-8')

  while (true) {
    const { value, done } = await reader.read()

    if (done) {
      break
    }

    const chunk = decoder.decode(value, { stream: true })

    onChunk(chunk)
  }

  onChunk(decoder.decode())
}

export async function getHealth() {
  const response = await fetch(`${API_BASE_URL}/api/health`);

  if (!response.ok) {
    throw new Error('Failed to fetch API health status');
  }

  return response.json();
}