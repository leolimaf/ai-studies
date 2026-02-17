using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();
    
var endpoint = config["AI:Endpoint"];
var modelId = config["AI:ModelId"];
var apiKey = config["GH_PAT"];

var azureOpenAI = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

var writer = azureOpenAI
    .GetChatClient(modelId)
    .AsAIAgent(instructions: "You are a software development expert. Write a single-paragraph article on the given topic, with a maximum of 70 words.");
    
var translator = azureOpenAI
    .GetChatClient(modelId)
    .AsAIAgent(instructions: "Translate any input to Brazilian Portuguese.");
    
var reviwer = azureOpenAI
    .GetChatClient(modelId)
    .AsAIAgent(instructions: "Revise the Portuguese text for clarity, grammar, and professional tone. Deliver only the final version.");

const string topic = "The impact of Artificial Intelligence on software development";

Console.WriteLine(topic);
Console.WriteLine();
    
var article = await writer.RunAsync(topic);
Console.WriteLine(article.ToString());
Console.WriteLine();

var translation = await translator.RunAsync(article.ToString());
Console.WriteLine(translation.ToString());
Console.WriteLine();

var review = await reviwer.RunAsync(translation.ToString());
Console.WriteLine(review.ToString());
Console.WriteLine();