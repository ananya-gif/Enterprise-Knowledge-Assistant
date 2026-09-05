using EnterpriseKnowledgeAssistant.Api.Models;
using UglyToad.PdfPig;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class DocumentService
{
    public List<DocumentPage> ExtractPages(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        using var document = PdfDocument.Open(stream);

        var pages = new List<DocumentPage>();

        foreach (var page in document.GetPages())
        {
            pages.Add(new DocumentPage
            {
                PageNumber = page.Number,
                Text = page.Text
            });
        }

        return pages;
    }
}