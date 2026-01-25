using System.ClientModel;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
    
var modelId = "gpt-4.1";

var uri = "https://models.inference.ai.azure.com";

var gitHubPat = config["GH_PAT"];

var client = new OpenAIClient(new ApiKeyCredential(gitHubPat!), new OpenAIClientOptions { Endpoint = new Uri(uri) });

var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(modelId, client);

var kernel = builder.Build();
var chat = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();

history.AddSystemMessage("Você é um assistente especializado que responde perguntas sobre desenvolvimento de softwares." +
                         "Você deve limitar suas respostas a informações relacionadas ao desenvolvimento de softwares." +
                         "Se uma pergunta estiver fora do contexto, você deve informar que não pode ajudar." +
                         "Se você não souber a resposta, responda educadamente que não sabe.");

Console.WriteLine("Tecle ENTER para encerrar");
Console.WriteLine();

while (true)
{
    Console.Write(">> ");
    var entrada = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(entrada))
        break;
    
    history.AddUserMessage(entrada);

    var sb = new StringBuilder();
    var result = chat.GetStreamingChatMessageContentsAsync(history);
    Console.WriteLine();

    await foreach (var item in result)
    {
        sb.Append(item);
        Console.Write(item.Content);
    }
    Console.WriteLine("\n");
    
    history.AddAssistantMessage(sb.ToString());
}