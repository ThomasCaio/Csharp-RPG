namespace Views;
using Spectre.Console;
using Items;

public class InventoryView : View {
    public GameLog Log;
    public InventoryView(RPG.Game parent) : base(parent) {
        Log = new GameLog(parent);
    }
    public override void Render() {
        AnsiConsole.Clear();
        Log.Render();
        var player = Parent.Player;
        if (player != null) {
            var table = new Table().Width(60).Title("Equipments").HideHeaders();
            table.AddColumn("Slot").Width(60);
            table.AddColumn("Item").Width(60);
            foreach (Items.ItemSlot slot in player.Body.Keys) {
                var item = player.Body[slot];
                if (item != null) {
                    table.AddRow(new Text(Item.SlotName(item.Slot)), new Text(item.Title));
                }
            }
            if (table.Rows.Count > 0) {
                AnsiConsole.Write(table);
            }

            var options = new SelectionPrompt<Items.Item>().UseConverter(item => item.Title).Title("Inventory");

            foreach (Items.Item item in player.Inventory.Items) {
                options.AddChoice(item);
            }
            var back = Items.Item.BlankItem("Back");
            options.AddChoice(back);
            Items.Item option = AnsiConsole.Prompt(options);
            if (option == back) {
                return;
            }
            ItemOption(option);
        }
        Render();
    }

    public void ItemOption(Item item) {
        var player = Parent.Player;
        if (player != null) {
            AnsiConsole.Clear();
            var prompt = new SelectionPrompt<string>();
            if (item.Type == ItemType.Wearable && !(player.Body.Values.Contains(item))) {
                prompt.AddChoice("Equip");
            }
            else if (player.Body.Values.Contains(item)) {
                prompt.AddChoice("Unequip");
            }
            else if (item.Type == ItemType.Usable) {
                prompt.AddChoice("Use");
            }
            prompt.AddChoice("Look");
            prompt.AddChoice("Back");
            var option = AnsiConsole.Prompt(prompt);
            if (option == "Equip") {
                player.Equip(item);
            }
            else if (option == "Unequip") {
                player.Unequip(item);
            }
            else if (option == "Use") {
                item.Use(player);
            }
            else if (option == "Look") {
                ItemDetails(item);
            }
        }
    }

    public void ItemDetails(Item item) {
        AnsiConsole.Clear();
        Table table = new Table();
        table.AddColumns("Attribute", "Value").HideHeaders().Title($"Looking {item.Title}");
        var dict = item.Look();
        foreach (string attr in dict.Keys) {
            table.AddRow(attr, dict[attr]);
        }
        AnsiConsole.Write(table);
        AnsiConsole.Write(new Markup("[gray]Type any key to continue.[/]"));
        System.Console.ReadKey();
    }
}