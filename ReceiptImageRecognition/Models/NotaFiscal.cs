namespace ReceiptImageRecognition.Models;

public class NotaFiscal
{
    public List<Item> Itens { get; set; }
    public decimal ValorTotal { get; set; }
    
    public class Item
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}