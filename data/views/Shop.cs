namespace Views;
using Items;
using Spectre.Console;

public class ShopView : View{

    public GameLog Log;

    public ShopView(RPG.Game parent) : base(parent) {
        Log = new GameLog(parent);
    }

    public override void Render(){
        AnsiConsole.Clear();
        Log.Render();
        var selection = new SelectionPrompt<string>()
        .AddChoiceGroup("Equipments", new[] {"Helmets", "Armors", "Legs", "Boots"})
        .AddChoiceGroup("Weapons", new[] {"Swords", "Axes", "Clubs", "Staffs"})
        .AddChoices(new[] {"Shields", "Consumables", "Back"});
        var option = AnsiConsole.Prompt(selection);
        if (option != "Back") {
            ShopItemType(option);
            Render();
        }
    }

    public void ShopItemType(string itemType) {
        var itemList = Parent.itemFactory.Filter(itemType);
        AnsiConsole.Clear();
        var selection = new SelectionPrompt<Item>();
        selection.UseConverter(item => {
            if (item.Price > 0)  {
                return $"{item.Title}\t\t${item.Price}";
            }
            return item.Title;
            });
        foreach(Item i in itemList) {
            selection.AddChoice(i);
        }
        selection.AddChoice(Item.BlankItem("Back"));
        Item option = AnsiConsole.Prompt(selection);
        if (option.Title != "Back") {
            Buy(option);
        }
    }

    public void Buy(Item item) {
        var player = Parent.Player;
        if (player != null) {
            if (player.Gold >= item.Price) {
                player.Inventory.Add(item);
                player.Gold -= item.Price;
            }
            else {
                Log.AddEvent($"You have no gold to buy {item.Title}. It costs {item.Price} gold.");
            }
        }
    }

    public void Sell(Item item) {

    }
}