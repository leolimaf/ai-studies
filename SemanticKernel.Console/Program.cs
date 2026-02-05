using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using SemanticKernel.Console.Plugins;

var builder = Kernel.CreateBuilder()
    .AddOllamaChatCompletion(modelId: "llama3.1:latest", endpoint: new Uri("http://localhost:11434"));
    
// builder.Services.AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace));

var app = builder.Build();

var chatCompletionService = app.GetRequiredService<IChatCompletionService>();
app.Plugins.AddFromType<ProductPlugin>(nameof(ProductPlugin));

OllamaPromptExecutionSettings executionSettings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

var history = new ChatHistory();

string? input;

do
{
    Console.Write(">> ");
    input = Console.ReadLine() ?? string.Empty;
    
    history.AddUserMessage(input);

    var result = await chatCompletionService.GetChatMessageContentAsync(history, executionSettings, kernel: app);
    Console.WriteLine(result);
    
    history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (!string.IsNullOrEmpty(input));