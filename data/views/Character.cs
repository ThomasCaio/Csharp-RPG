namespace Views;
using ItemModule;
using Spectre.Console;


public class CharacterView : View {

    public GameLog Log;
    public CharacterView(RPG.Game parent) : base(parent) {
        Log = new GameLog(parent);
    }

    public override void Render() {
        var player = Parent.Player!;
        AnsiConsole.Clear();
        Log.Render();
        Grid grid = new Grid();
        Table table = new Table().Width(60).Title($"{player.Name}").HideHeaders();
        table.AddColumn("Attributes");
        table.AddColumn("Values");

        table.AddRow(new Text("Max Health"), new Text($"{player.MaxHealth}"));
        // table.AddRow(new Text("Max Mana"), new Text($"{player.MaxMana}"));
        table.AddRow(new Text("Damage"), new Text($"{player.TotalDamage}"));
        table.AddRow(new Text("Defense"), new Text($"{player.Defense}"));
        table.AddRow(new Text("Armor"), new Text($"{player.Armor}"));
        table.AddRow(new Text("Physical Resistance"), new Text($"{player.PhysicalResistance}"));
        table.AddRow(new Text("Fire Resistance"), new Text($"{player.FireResistance}"));
        table.AddRow(new Text("Water Resistance"), new Text($"{player.WaterResistance}"));
        table.AddRow(new Text("Air Resistance"), new Text($"{player.AirResistance}"));
        table.AddRow(new Text("Earth Resistance"), new Text($"{player.EarthResistance}"));

        AnsiConsole.Write(table);

        if (player != null) {
            var selection = new SelectionPrompt<string>();
            selection.AddChoice("Equipped items");
            selection.AddChoice("Spellbook");
            selection.AddChoice("Passive spells");
            selection.AddChoice("Back");
            var option = AnsiConsole.Prompt(selection);

            if (option == "Equipped items")
            {
                CharacterItems();
            }
            else if (option == "Spellbook")
            {
                Spellbook();
            }
            else if (option == "Back")
            {
                return;
            }
        }
        Render();
    }

    public void CharacterItems() {
        AnsiConsole.Clear();
        var player = Parent.Player;
        if (player != null) {
            var options = new SelectionPrompt<ItemModule.Item>().UseConverter(item => item.Name).Title("Equipped Items");

            foreach (ItemModule.Item? item in player.Body.Values) {
                if (item != null) {
                    options.AddChoice(item);
                }
            }
            var back = ItemModule.Item.BlankItem("Back");
            options.AddChoice(back);
            ItemModule.Item option = AnsiConsole.Prompt(options);
            if (option == back) {
                return;
            }
            var inventoryView = (InventoryView)Parent.GameViews["Inventory"];
            inventoryView.ItemOption(option);
        }
        CharacterItems();
    }

    public void Spellbook()
    {
        var spellbook = Parent.Player!.Spellbook;
        if (spellbook.Count() <= 0)
        {
            AnsiConsole.MarkupLine("You have no spells yet.");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[gray][italic]Press any key to continue.[/][/]");
            Console.ReadKey();
            return;
        }
        var spells = new SelectionPrompt<SpellModule.Spell>().UseConverter(s => s.Name).Title("Spellbook").AddChoices(spellbook);
        var option = AnsiConsole.Prompt(spells);
        AnsiConsole.Clear();
        var g = new Grid();
        g.AddColumn(new GridColumn());
        g.AddColumn(new GridColumn());
        // TODO: Magical Damage Type
        g.AddRow(new[] {new Markup($"[red]{option.Name}[/]"), new Markup("[blue]Magic[/]")});
        g.AddRow(new[] {new Markup("Damage"), new Markup($"{option.Damage()}")});
        g.AddRow(new[] {new Markup("Description"), new Markup($"[italic]{option.SpellDescription()}[/]")});
        AnsiConsole.Write(g);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[gray][italic]Press any key to continue.[/][/]");
        Console.ReadKey();
    }
}