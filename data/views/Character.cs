namespace Views;
using Items;
using Spectre.Console;


public class CharacterView : View {

    public GameLog Log;
    public CharacterView(RPG.Game parent) : base(parent) {
        Log = new GameLog(parent);
    }

    public override void Render() {
        AnsiConsole.Clear();
        Log.Render();
        var player = Parent.Player;
        if (player != null) {
            var selection = new SelectionPrompt<string>();
            selection.AddChoice("Equipped items");
            selection.AddChoice("Spellbook");
            selection.AddChoice("Passive spells");
            selection.AddChoice("Back");
            var option = AnsiConsole.Prompt(selection);

            if (option == "Equipped items") {
                CharacterItems();
            }
            else if (option == "Back"){
                return;
            }
        }
        Render();
    }

    public void CharacterItems() {
        var player = Parent.Player;
        if (player != null) {
            var options = new SelectionPrompt<Items.Item>().UseConverter(item => item.Title).Title("Equipped Items");

            foreach (Items.Item? item in player.Body.Values) {
                if (item != null) {
                    options.AddChoice(item);
                }
            }
            var back = Items.Item.BlankItem("Back");
            options.AddChoice(back);
            Items.Item option = AnsiConsole.Prompt(options);
            if (option == back) {
                return;
            }
            var inventoryView = (InventoryView)Parent.GameViews["Inventory"];
            inventoryView.ItemOption(option);
        }
        CharacterItems();
    }
}