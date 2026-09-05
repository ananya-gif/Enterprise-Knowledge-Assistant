const API_BASE_URL = 'http://localhost:5107';

export interface SourceReference {
  fileName: string;
  pageNumber: number;
  chunkIndex: number;
}

export interface ChatResponse {
  answer: string;
  sources: SourceReference[];
}

export async function sendChatMessage(
  message: string,
): Promise<ChatResponse> {
  const response = await fetch(`${API_BASE_URL}/api/chat`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      message,
    }),
  });

  if (!response.ok) {
    throw new Error(`Failed to send chat message: ${response.status}`);
  }

  return response.json();
}

export async function streamChatMessage(
  message: string,
  onChunk: (chunk: string) => void,
  onSources: (sources: SourceReference[]) => void,
): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/api/chat/stream`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ message }),
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  if (!response.body) {
    throw new Error('Response body is not available.');
  }

  const reader = response.body.getReader();
  const decoder = new TextDecoder('utf-8');

  let buffer = '';

  const processLine = (line: string) => {
    if (!line.trim()) {
      return;
    }

    const data = JSON.parse(line);

    // Support both camelCase and PascalCase
    const type = data.type ?? data.Type;
    const content = data.content ?? data.Content;
    const sources = data.sources ?? data.Sources;

  if (type === 'sources') {
  const normalizedSources: SourceReference[] = (sources ?? []).map(
    (source: any) => ({
      fileName: source.fileName ?? source.FileName ?? '',
      pageNumber: source.pageNumber ?? source.PageNumber ?? 0,
      chunkIndex: source.chunkIndex ?? source.ChunkIndex ?? 0,
    }),
  );

  onSources(normalizedSources);
}

    if (type === 'text') {
      onChunk(content ?? '');
    }
  };

  while (true) {
    const { value, done } = await reader.read();

    if (done) {
      break;
    }

    buffer += decoder.decode(value, { stream: true });

    const lines = buffer.split('\n');

    buffer = lines.pop() ?? '';

    for (const line of lines) {
      processLine(line);
    }
  }

  buffer += decoder.decode();

  if (buffer.trim()) {
    processLine(buffer);
  }
}

export async function uploadDocument(
  file: File,
): Promise<{
  fileName: string;
  chunkCount: number;
}> {
  const formData = new FormData();

  formData.append('file', file);

  const response = await fetch(
    `${API_BASE_URL}/api/document/upload`,
    {
      method: 'POST',
      body: formData,
    },
  );

  if (!response.ok) {
    throw new Error(`Failed to upload document: ${response.status}`);
  }

  return response.json();
}

export async function getHealth() {
  const response = await fetch(`${API_BASE_URL}/api/health`);

  if (!response.ok) {
    throw new Error('Failed to fetch API health status');
  }

  return response.json();
}