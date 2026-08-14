const API_BASE_URL = 'http://localhost:5107';

export async function getHealth() {
  const response = await fetch(`${API_BASE_URL}/api/health`);

  if (!response.ok) {
    throw new Error('Failed to fetch API health status');
  }

  return response.json();
}