namespace Views;
using RPG;
using Spectre.Console;
using System.Text.RegularExpressions;


public class MenuView : View {
    string? CharacterName;
    public MenuView(Game parent) : base(parent) {
    }

    public override void Render() {
        Grid g = new Grid();

        g.AddColumn(new GridColumn());
        g.AddRow(new Markup("[red]RPG[/] Game TEST"));
        g.AddEmptyRow();

        g.AddRow("1: Create a new character.");
        g.AddRow("2: Load character.");
        g.AddRow("3: Exit.");

        AnsiConsole.Write(g);

        char option = Console.ReadKey().KeyChar;
        if (option == '1') {
            NewCharacter();
        }
        else if (option == '2') {
        }
        else if (option == '3') {
        }
        else {
        }
    }

    public void NewCharacter() {
        Grid grid = new Grid();
        grid.AddColumn(new GridColumn());
        grid.AddRow(new Markup("Type your [underline]character name[/]!"));
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
        if (Parent.Debug) {
            Parent.PlayerSetUp(Test.PlayerTest);
        }
        else {
            Parent.PlayerSetUp(p => {
                p.Inventory.Add(new Items.All.ShortSword());
            });
        }
        Parent.GameViews["Game"].Render();
    }
}