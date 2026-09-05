using EnterpriseKnowledgeAssistant.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<OpenAIService>();
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<ChunkingService>();
builder.Services.AddScoped<EmbeddingService>();
builder.Services.AddScoped<DocumentIngestionService>();
builder.Services.AddSingleton<InMemoryVectorStore>();
builder.Services.AddScoped<SemanticSearchService>();
builder.Services.AddScoped<RagService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("ReactFrontend");

app.MapControllers();

app.Run();