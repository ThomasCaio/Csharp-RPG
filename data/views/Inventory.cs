namespace Views;
using Spectre.Console;
using ItemModule;

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
            foreach (ItemModule.ItemSlot slot in player.Body.Keys) {
                var item = player.Body[slot];
                if (item != null) {
                    table.AddRow(new Text(item.Slot.ToString()), new Text(item.Name));
                }
            }
            if (table.Rows.Count > 0) {
                AnsiConsole.Write(table);
            }

            var options = new SelectionPrompt<ItemModule.Item>().UseConverter(item => item.Name).Title("Inventory");

            foreach (ItemModule.Item item in player.Inventory.Items) {
                options.AddChoice(item);
            }
            var back = ItemModule.Item.BlankItem("Back");
            options.AddChoice(back);
            ItemModule.Item option = AnsiConsole.Prompt(options);
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
            if (item.ObjectType == ObjectTypes.Wearable && !(player.Body.Values.Contains(item))) {
                prompt.AddChoice("Equip");
            }
            else if (player.Body.Values.Contains(item)) {
                prompt.AddChoice("Unequip");
            }
            else if (item.ObjectType == ObjectTypes.Usable) {
                prompt.AddChoice("Use");
            }
            prompt.AddChoice("Look");
            prompt.AddChoice("Back");
            var option = AnsiConsole.Prompt(prompt);
            if (option == "Equip") {
                player.Equip((Wearable)item);
            }
            else if (option == "Unequip") {
                player.Unequip((Wearable)item);
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
        table.AddColumns("Attribute", "Value").HideHeaders().Title($"Looking {item.Name}");
        var dict = item.Look();
        foreach (string attr in dict.Keys) {
            table.AddRow(attr, dict[attr]);
        }
        AnsiConsole.Write(table);
        AnsiConsole.Write(new Markup("[gray]Type any key to continue.[/]"));
        System.Console.ReadKey();
    }
}