namespace Views;
using ItemModule;
using Spectre.Console;

public class ShopView : View {

    public GameLog Log;

    public ShopView(RPG.Game parent) : base(parent) {
        Parent = parent;
        Log = new GameLog(parent);
    }

    public override void Render(){
        AnsiConsole.Clear();
        Log.Render();

        // Simplify selection prompt creation by chaining methods
        var selection = new SelectionPrompt<string>()
            .Title("What would you like to buy?")
            .PageSize(10)
            .AddChoices(new[] {"Equipments", "Weapons", "Shields", "Consumables", "Back"});

        var option = AnsiConsole.Prompt(selection);
        if (option != "Back") {
            ShopItemType(option);
            Render();
        }
    }

    public void ShopItemType(string itemType) {
        Enum.TryParse(itemType, out ItemTypes it);
        var itemList = Parent.itemFactory.GetItems(it);
        AnsiConsole.Clear();

        // Simplify selection prompt creation by chaining methods
        var selection = new SelectionPrompt<Item>()
            .Title($"Select a {itemType} to buy:")
            .PageSize(10)
            .UseConverter(item => $"{item.Name}\t${item.Price}")
            .AddChoices(itemList);

        Item option = AnsiConsole.Prompt(selection);
        if (option != null) {
            Buy(option);
        }
    }

    public void Buy(Item item) {
        var player = Parent.Player;
        if (player != null) {
            if (player.Gold >= item.Price) {
                player.Inventory.Add(item);
                player.Gold -= item.Price;
                Log.AddEvent($"You bought {item.Name} for ${item.Price}.");
            }
            else {
                Log.AddEvent($"You don't have enough gold to buy {item.Name}. It costs ${item.Price}.");
            }
        }
    }

    public void Sell(Item item) {
        // TODO: Implement selling items
    }
}
