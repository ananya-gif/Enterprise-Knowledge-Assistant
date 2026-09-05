using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class RagService
{
    private readonly SemanticSearchService _searchService;
    private readonly OpenAIService _openAIService;

    public RagService(
        SemanticSearchService searchService,
        OpenAIService openAIService)
    {
        _searchService = searchService;
        _openAIService = openAIService;
    }

    public async Task<List<SearchResult>> SearchAsync(
        string question)
    {
        return await _searchService.SearchAsync(
            question,
            topK: 3);
    }

    public string BuildContext(
    string question,
    List<SearchResult> results)
    {
        return BuildPrompt(question, results);
    }

    public async Task<string> GenerateAnswerAsync(
        string question,
        List<SearchResult> results)
    {
        var prompt = BuildPrompt(question, results);

        return await _openAIService.GetResponseAsync(prompt);
    }

    public async Task<string> AskAsync(string question)
    {
        var results = await SearchAsync(question);

        return await GenerateAnswerAsync(question, results);
    }

    public async Task<string> BuildContextAsync(string question)
    {
        var results = await SearchAsync(question);

        return BuildPrompt(question, results);
    }

    private string BuildPrompt(
        string question,
        List<SearchResult> results)
    {
        if (results.Count == 0)
        {
            return """
                You are an enterprise knowledge assistant.

                There is no relevant information in the
                provided knowledge base to answer the user's question.

                Tell the user that you don't have enough information
                from the knowledge base to answer the question.
                """;
        }

        var context = string.Join(
            "\n\n",
            results.Select(result => result.Chunk.Text));

        return $"""
            You are an enterprise knowledge assistant.

            Answer the user's question using only the
            information provided in the context below.

            If the answer cannot be found in the context,
            say that you don't have enough information.

            Context:
            {context}

            User question:
            {question}
            """;
    }
}