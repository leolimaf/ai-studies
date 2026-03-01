using Microsoft.Extensions.AI;
using OllamaSharp;
using ReceiptImageRecognition.Models;

IChatClient chatClient = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2-vision:latest");

var systemMessage = new ChatMessage(ChatRole.System, 
    """
    Você é um assistente de extração de dados de notas fiscais. 
    Sua tarefa é analisar a imagem da nota fiscal fornecida e tentar extrair as seguintes informações:
    - Itens comprados (código, descrição, quantidade, valor unitário e valor total)
    - Valor total da compra
    """);

var message = new ChatMessage(ChatRole.User, 
    """
    Extraia os dados da nota fiscal.
    Informe o conteúdo contido no formato em JSON, conforme o seguinte exemplo:
    {
        "itens": [
            {
                "codigo": 123,
                "nome": "Arroz Integral 1kg",
                "quantidade": 1,
                "valor_unitario": 32.90,
                "valor_total": 32.90
            }
        ],
        "valor_total": 32.90
    }
    """);
message.Contents.Add(new DataContent(File.ReadAllBytes("nota-fiscal-supermercado.jpg"), "image/jpeg"));

var response = await chatClient.GetResponseAsync<NotaFiscal>([systemMessage, message],  new ChatOptions{Temperature = 0.3f});

if (response.Result is { } notaFiscal)
{
    Console.WriteLine($"\nForam extraídos {notaFiscal.Itens.Count} itens da nota fiscal:");

    foreach (var item in notaFiscal.Itens)
        Console.WriteLine($"{item.Descricao} => R$ {item.ValorUnitario} x {item.Quantidade} = R$ {item.ValorTotal}");

    Console.WriteLine("\nValor total da compra: R$ " + notaFiscal.ValorTotal);
}

Console.WriteLine(response.Text);