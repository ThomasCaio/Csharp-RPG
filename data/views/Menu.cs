namespace Views;
using RPG;
using Spectre.Console;
using System.Text.RegularExpressions;
using FileModule;


public class MenuView : View {
    string? CharacterName;
    public MenuView(Game parent) : base(parent) {
    }

    public override void Render() {
        AnsiConsole.Clear();
        Grid g = new Grid();

        g.AddColumn(new GridColumn());
        g.AddRow(new Markup("[red]RPG[/] Game TEST"));
        g.AddEmptyRow();

        g.AddRow("1: Create a new character.");
        g.AddRow("2: Load character.");
        g.AddRow("3: Exit.");

        AnsiConsole.Write(g);

        char option = Console.ReadKey().KeyChar;
        if (option == '1')
        {
            NewCharacter();
        }
        else if (option == '2')
        {
            Load();
            Render();
        }
        else if (option == '3')
        {
            AnsiConsole.Clear();
            if (!AnsiConsole.Confirm("Do you really want to leave?"))
            {
                Render();
            }
            Environment.Exit(0);
        }
        else {
            Render();
        }
    }

    public void NewCharacter() {
        Table t = new Table().HideHeaders();
        t.AddColumn(new TableColumn("Text"));
        t.AddRow(new Markup("Type your [underline]character name[/]!"));
        t.AddEmptyRow();
        t.AddEmptyRow();
        t.AddEmptyRow();
        t.AddEmptyRow();


        while (true) {
            AnsiConsole.Clear();
            AnsiConsole.Write(t);
            AnsiConsole.Cursor.SetPosition(3, 4);
            CharacterName = System.Console.ReadLine();
            if (CharacterName != null) {
                if (CharacterName.Count() < 3) {
                    t.UpdateCell(4, 0, new Text("Error: Your name should have at least 3 characters."));
                    continue;
                }
                if (!Regex.IsMatch(CharacterName, @"^[a-zA-Z]+$")) {
                    t.UpdateCell(4, 0, new Text("Error: Only letters allowed!"));
                    continue;
                }
                if (CharacterName != null) {
                    CharacterName.Trim();
                    break;
                }
            }
        }
        Parent.Player = new Entities.Character(CharacterName);
        if (Parent.gSetup != null) {
            Parent.gSetup(Parent);
        }
        Parent.GameViews["Game"].Render();
    }

    public void Load()
    {
        AnsiConsole.Clear();
        var characters = FileHandler.ListCharacters();
        if (characters.Count() <= 0)
        {
            AnsiConsole.MarkupLine("No characters to load.");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[italic][grey]Press any key to continue.[/][/]");
            Console.ReadKey();
            return;
        }
        
        List<string> marked_characters = new List<string>();
        foreach (var c in characters)
        {
            var marked = c;
            if (c.Contains("Test "))
            {
                var test = c.Substring(0, 4);
                var name = c.Substring(5);

                marked = $"[yellow]{test}[/] {name}";

            }
            marked_characters.Add(marked);
        }
        var prompt = new SelectionPrompt<string>()
            .Title("Characters")
            .MoreChoicesText("")
            .AddChoices(marked_characters);
        var character = AnsiConsole.Prompt(prompt);
        int idx = marked_characters.IndexOf(character);
        string selected_characater = characters[idx];
        FileHandler.LoadCharacter(Parent, selected_characater);
        Parent.GameViews["Game"].Render();
    }
}