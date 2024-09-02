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

        var selection = new SelectionPrompt<string>()
            .Title("What would you like to buy?")
            .PageSize(10)
            .AddChoices(new[] {"Equipments", "Weapons", "Shields", "Consumables", "Back"});

        var option = AnsiConsole.Prompt(selection);

        if (option == "Back")
        {
            return;
        }

        string subOption = string.Empty;

        if (option == "Equipments")
        {
            var s = new SelectionPrompt<string>()
            .Title("What would you like to buy?")
            .PageSize(10)
            .AddChoices(new[] {"Helmet", "Armor", "Legs", "Boots", "Back"});
            subOption = AnsiConsole.Prompt(s);
            if (subOption == "Back") {
                Render();
                return;
            }
        }
        else if (option == "Weapons")
        {
            var s = new SelectionPrompt<string>()
            .Title("What would you like to buy?")
            .PageSize(10)
            .AddChoices(new[] {"Sword", "Axe", "Club", "Back"});
            subOption = AnsiConsole.Prompt(s);
            if (subOption == "Back") {
                Render();
                return;
            }
        }
        else if (option == "Consumables")
        {
            option = "Consumable";
            ShopItemType(option);
        }

        else if (option == "Shields")
        {
            ShopItemType("Shield");    
        }
        
        if (!string.IsNullOrEmpty(subOption) && subOption != "Back")
        {
            ShopItemType(subOption);
        }
        Logging.Debug.Write($"{option} {subOption}", "option");
        Render();
    }

    public void ShopItemType(string itemType) {
        Enum.TryParse(itemType, out ItemTypes it);
        Logging.Debug.Write($"{itemType} - {it}", "shop");
        var itemList = new List<Item>(Parent.itemFactory.GetItems(it)) {Item.BlankItem("Back")};

        AnsiConsole.Clear();

        var selection = new SelectionPrompt<Item>()
            .Title($"Select a {itemType} to buy:")
            .PageSize(10)
            .UseConverter(item => item.Name == "Back" ? "Back" : $"{item.Name}\t\t\t${item.BuyPrice}")
            .AddChoices(itemList);

        Item option = AnsiConsole.Prompt(selection);

        if (option.Name == "Back") {
            return;
        } else if (option != null) {
            Buy(option);
        }
    }

    public void Buy(Item item) {
        var player = Parent.Player;
        if (player != null) {
            if (player.Gold >= item.BuyPrice) {
                player.Inventory.Add(item);
                player.Gold -= item.BuyPrice;
                Log.AddEvent($"You bought {item.Name} for ${item.BuyPrice}.");
            }
            else {
                Log.AddEvent($"You don't have enough gold to buy {item.Name}. It costs ${item.BuyPrice}.");
            }
        }
    }

    public void Sell(Item item) {
        // TODO: Implement selling items
    }
}
