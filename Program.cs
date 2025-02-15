using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static string filePath = "notes.json";
    static List<StickyNote> notes = LoadNotes();

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Sticky Notes");
            Console.WriteLine("1. Criar Nota");
            Console.WriteLine("2. Listar Notas");
            Console.WriteLine("3. Editar Nota");
            Console.WriteLine("4. Excluir Nota");
            Console.WriteLine("5. Sair");
            Console.Write("Escolha uma opção: ");

            switch (Console.ReadLine())
            {
                case "1": CriarNota(); break;
                case "2": ListarNotas(); break;
                case "3": EditarNota(); break;
                case "4": ExcluirNota(); break;
                case "5": return;
                default: Console.WriteLine("Opção inválida!"); break;
            }
        }
    }

    static void CriarNota()
    {
        Console.Write("Digite o conteúdo da nota: ");
        string conteudo = Console.ReadLine();
        notes.Add(new StickyNote { Id = Guid.NewGuid(), Conteudo = conteudo });
        SalvarNotas();
    }

    static void ListarNotas()
    {
        Console.Clear();
        if (notes.Count == 0) Console.WriteLine("Nenhuma nota encontrada.");
        else notes.ForEach(n => Console.WriteLine($"[{n.Id}] {n.Conteudo}"));
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    static void EditarNota()
    {
        Console.Write("Digite o ID da nota que deseja editar: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            var nota = notes.Find(n => n.Id == id);
            if (nota != null)
            {
                Console.Write("Novo conteúdo: ");
                nota.Conteudo = Console.ReadLine();
                SalvarNotas();
            }
        }
    }

    static void ExcluirNota()
    {
        Console.Write("Digite o ID da nota que deseja excluir: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            notes.RemoveAll(n => n.Id == id);
            SalvarNotas();
        }
    }

    static void SalvarNotas()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(notes));
    }

    static List<StickyNote> LoadNotes()
    {
        return File.Exists(filePath) ?
            JsonSerializer.Deserialize<List<StickyNote>>(File.ReadAllText(filePath)) :
            new List<StickyNote>();
    }
}

class StickyNote
{
    public Guid Id { get; set; }
    public string Conteudo { get; set; }
}
