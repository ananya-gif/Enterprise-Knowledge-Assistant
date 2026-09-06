# Enterprise Knowledge Assistant

An AI-powered enterprise knowledge assistant that allows users to upload PDF documents and ask natural-language questions about their content.

The application uses Retrieval-Augmented Generation (RAG) to retrieve relevant document chunks using semantic search and provide grounded answers using OpenAI.

## Features

- PDF document upload
- Page-aware PDF text extraction
- Configurable text chunking with overlap
- OpenAI embeddings using `text-embedding-3-small`
- In-memory vector storage for local development
- Semantic search using cosine similarity
- Retrieval-Augmented Generation (RAG)
- Streaming AI responses
- Source citations with document name, page number, and chunk
- React-based chat interface
- API validation and error handling
- Automated unit tests for core ingestion and retrieval logic

## Architecture

```text
React Client
    |
    v
ASP.NET Core API
    |
    +--> Document Ingestion
    |       |
    |       +--> PDF Extraction
    |       +--> Chunking
    |       +--> OpenAI Embeddings
    |       +--> In-Memory Vector Store
    |
    +--> Chat / RAG Pipeline
            |
            +--> Query Embedding
            +--> Semantic Search
            +--> Context Construction
            +--> OpenAI Responses API
            +--> Streaming Response + Sources
```

## RAG Flow

### Document ingestion

1. Extract PDF text page by page.
2. Split text into overlapping chunks.
3. Generate an embedding for each chunk.
4. Store chunks, metadata, and embeddings in the vector store.

### Question answering

1. Generate an embedding for the user's question.
2. Calculate cosine similarity against stored document embeddings.
3. Select the most relevant chunks.
4. Build a grounded RAG prompt using the retrieved context.
5. Generate the answer using OpenAI.
6. Stream the answer back to React.
7. Return source metadata including document name, page, and chunk.

## Technology Stack

### Backend

- C#
- ASP.NET Core
- .NET 10
- OpenAI .NET SDK
- PdfPig
- xUnit

### Frontend

- React
- TypeScript
- Vite
- CSS

### AI / Search

- OpenAI `gpt-5`
- OpenAI `text-embedding-3-small`
- Semantic search
- Cosine similarity
- Retrieval-Augmented Generation

## Project Structure

```text
EnterpriseKnowledgeAssistant/
|
+-- backend/
|   +-- EnterpriseKnowledgeAssistant.Api/
|   |   +-- Controllers/
|   |   +-- Models/
|   |   +-- Services/
|   |   +-- Program.cs
|   |
|   +-- EnterpriseKnowledgeAssistant.Tests/
|       +-- ChunkingServiceTests.cs
|       +-- SemanticSearchServiceTests.cs
|
+-- frontend/
|   +-- src/
|       +-- pages/
|       +-- services/
|       +-- ...
|
+-- README.md
```

## Running the Application

### Prerequisites

- .NET 10 SDK
- Node.js
- OpenAI API key

Configure the OpenAI API key using local configuration or environment variables.

**Never commit API keys or other secrets to source control.**

### Backend

Navigate to:

```text
backend/EnterpriseKnowledgeAssistant.Api
```

Run:

```bash
dotnet run
```

### Frontend

Navigate to:

```text
frontend
```

Install dependencies:

```bash
npm install
```

Start the development server:

```bash
npm run dev
```

Open the URL shown by Vite.

## Running Tests

From the `backend` directory:

```bash
dotnet test EnterpriseKnowledgeAssistant.Api/EnterpriseKnowledgeAssistant.Api.slnx
```

The test suite covers:

- Chunk creation
- Chunk overlap
- Page metadata preservation
- Cosine similarity
- Zero-vector handling
- Vector dimension validation

## Current Storage Design

The current implementation uses an in-memory vector store.

This was intentionally chosen for the local prototype to validate the complete RAG pipeline without introducing unnecessary infrastructure during development.

The in-memory store is not persistent and its contents are lost when the application restarts.

## Production Architecture

For production, the in-memory vector store can be replaced with persistent infrastructure.

A possible AWS architecture:

```text
React
  |
  v
Application Load Balancer
  |
  v
ASP.NET Core API
  |
  +--> Amazon S3
  |     Document storage
  |
  +--> Amazon RDS PostgreSQL + pgvector
  |     Persistent vector storage/search
  |
  +--> OpenAI API
  |     Embeddings + generation
  |
  +--> AWS Secrets Manager
  |
  +--> Amazon CloudWatch
        Logging and monitoring
```

### Production improvements

- PostgreSQL with `pgvector` for persistent vector storage
- Amazon S3 for document storage
- AWS Secrets Manager for API credentials
- IAM-based access control
- Authentication and authorization
- Metadata filtering
- Vector indexes for larger datasets
- Request cancellation and timeouts
- Structured logging and monitoring
- Rate limiting
- Document lifecycle management
- Retrieval evaluation and ranking improvements

## Design Considerations

### Why chunk documents?

Large documents cannot efficiently be passed to the model as a single context. Chunking allows the system to retrieve only the most relevant portions of a document.

### Why use overlap?

Overlap helps preserve context across chunk boundaries so information split between two chunks is less likely to be lost during retrieval.

### Why embeddings?

Embeddings represent text as vectors, allowing semantically similar questions and document sections to be compared even when they do not use the exact same words.

### Why cosine similarity?

Cosine similarity measures the directional similarity between embedding vectors and is commonly used for semantic retrieval.

### Why RAG?

RAG allows the model to generate answers grounded in the application's enterprise knowledge base instead of relying only on the model's pre-trained knowledge.

## Limitations

The current implementation is a portfolio-focused prototype rather than a production deployment.

Known limitations include:

- In-memory vector storage
- No persistent document database
- No authentication/authorization
- PDF-only ingestion
- Basic retrieval ranking
- No advanced metadata filtering
- No document deletion API
- No multi-user isolation

These areas are addressed in the proposed production architecture.

## Future Enhancements

- PostgreSQL + pgvector
- S3 document storage
- Authentication and authorization
- Multiple document formats
- Document management UI
- Hybrid keyword + vector search
- Reranking
- Conversation history
- Enterprise access control
- Retrieval evaluation metrics
- Production observability

## License

This project is intended as a portfolio and learning project.
