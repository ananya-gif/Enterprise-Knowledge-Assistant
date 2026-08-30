using UglyToad.PdfPig;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class DocumentService
{
    public string ExtractText(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        using var document = PdfDocument.Open(stream);

        var pages = new List<string>();

        foreach (var page in document.GetPages())
        {
            pages.Add(page.Text);
        }

        return string.Join("\n", pages);
    }
}