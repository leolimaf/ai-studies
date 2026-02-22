using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

class Program
{
    static int maxTokens = 100;
    static double temperature = 0.7;
    static double topP = 1.0;
    static int topK = 0; // 0 = desativado (GitHubModels não suporta)
    static double frequencyPenalty = 0.0;
    static double presencePenalty = 0.0;
    static string? stopSequence = null;
    
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        const string uri = "https://models.github.ai/inference";

        var gitHubPat = config["GH_PAT"];

        var client = new OpenAIClient(new ApiKeyCredential(gitHubPat!), new OpenAIClientOptions { Endpoint = new Uri(uri) })
            .GetChatClient("openai/gpt-4o-mini")
            .AsIChatClient();

        while (true)
        {
            Console.Clear();
            MostrarParametros();
            MostrarMenu();

            Console.Write("\nEscolha uma opção: ");
            var opcao = Console.ReadLine();
            
            switch (opcao)
            {
                case "1": AlterarMaxTokens(); break;
                case "2": AlterarTemperature(); break;
                case "3": AlterarTopP(); break;
                case "4": AlterarTopK(); break;
                case "5": AlterarFrequencyPenalty(); break;
                case "6": AlterarPresencePenalty(); break;
                case "7": AlterarStopSequence(); break;
                case "8": await FazerPergunta(client); break;
                case "0": return;
            }
        }
    }

    static void MostrarParametros()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n== Parâmetros Atuais do LLM ==\n");
        Console.ResetColor();
        
        Console.WriteLine($"1. MaxTokens: {maxTokens}");
        Console.WriteLine($"2. Temperature: {temperature}");
        Console.WriteLine($"3. Top P: {topP}");
        Console.WriteLine($"4. Top K: {topK}");
        Console.WriteLine($"5. Frequency Penalty: {frequencyPenalty}");
        Console.WriteLine($"6. Presence Penalty: {presencePenalty}");
        Console.WriteLine($"7. Stop Sequence: {stopSequence ?? "Nenhuma"}");
    }
    
    static void MostrarMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n== Menu de Configuração ==\n");
        Console.ResetColor();

        Console.WriteLine("1 - Alterar Max Tokens");
        Console.WriteLine("2 - Alterar Temperature");
        Console.WriteLine("3 - Alterar Top P");
        Console.WriteLine("4 - Alterar Top K");
        Console.WriteLine("5 - Alterar Frequency Penalty");
        Console.WriteLine("6 - Alterar Presence Penalty");
        Console.WriteLine("7 - Alterar Stop Sequence");
        Console.WriteLine("8 - Fazer uma pergunta ao modelo");
        Console.WriteLine("0 - Sair");
    }

    static void AlterarMaxTokens()
    {
        Console.WriteLine("Novo valor (ex: 50, 100, 200): ");
        if (int.TryParse(Console.ReadLine(), out var valor))
            maxTokens = valor;
    }
    
    static void AlterarTemperature()
    {
        Console.WriteLine("Novo valor (0.0 a 2.0): ");
        if (double.TryParse(Console.ReadLine(), out var valor))
            temperature = valor;
    }
    
    static void AlterarTopP()
    {
        Console.WriteLine("Novo valor (0.0 a 1.0): ");
        if (double.TryParse(Console.ReadLine(), out var valor))
            topP = valor;
    }
    
    static void AlterarTopK()
    {
        Console.WriteLine("Novo valor (0 a 1): ");
        if (int.TryParse(Console.ReadLine(), out var valor))
            topK = valor;
    }
    
    static void AlterarFrequencyPenalty()
    {
        Console.WriteLine("Novo valor (-2.0 a 2.0): ");
        if (double.TryParse(Console.ReadLine(), out var valor))
            frequencyPenalty = valor;
    }
    
    static void AlterarPresencePenalty()
    {
        Console.WriteLine("Novo valor (-2.0 a 2.0): ");
        if (double.TryParse(Console.ReadLine(), out var valor))
            presencePenalty = valor;
    }
    
    static void AlterarStopSequence()
    {
        Console.WriteLine("Digite uma sequência (ex: ###) ou enter para remover: ");
        var valor = Console.ReadLine();
        stopSequence = string.IsNullOrWhiteSpace(valor) ? null : valor;
    }
    
    static async Task FazerPergunta(IChatClient client)
    {
        ChatResponse response = null!;
        Console.Clear();
        MostrarParametros();
        
        Console.WriteLine("\nDigite sua pergunta para o modelo:");
        var prompt = Console.ReadLine();

        var options = new ChatOptions
        {
            MaxOutputTokens = maxTokens,
            Temperature = (float) temperature,
            TopP = (float) topP,
            TopK = topK,
            FrequencyPenalty = (float) frequencyPenalty,
            PresencePenalty = (float) presencePenalty,
            StopSequences = !string.IsNullOrWhiteSpace(stopSequence) ? new[] { stopSequence } : null
        };

        try
        {
            response = await client.GetResponseAsync(prompt!, options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine(response.Messages[0]);
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.WriteLine($"Tokens usados: prompt = {response.Usage?.InputTokenCount}, resposta = {response.Usage?.OutputTokenCount}");
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}