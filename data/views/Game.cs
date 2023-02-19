namespace Views;
using Spectre.Console;

public class GameView : View {
    GameLog Log;

    public GameView(RPG.Game parent) : base(parent) {
        Log = new GameLog(parent);
    }

    public override void Render() {
        var player = Parent.Player;
        if (player != null) {
            AnsiConsole.Clear();

            var grid = new Grid();
            grid.AddColumn(new GridColumn());

            var table = RenderCharacterStats(player);

            if (Log.Events.Count() > 0) {
                grid.AddColumn(new GridColumn());
                grid.AddRow(table, Log.Table);

            }
            else {
                grid.AddRow(table);
            }
            
            Log.LoadEvents();
            Log.ClearEvents();
            AnsiConsole.Write(grid);
            Log.ClearLog();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices("Hunt", "Character", "Inventory", "Shop", "Save and exit game"));

            if (option == "Hunt") {
                Parent.GameViews["Hunt"].Render();
            } else if (option == "Character") {
                Parent.GameViews["Character"].Render();
            } else if (option == "Inventory") {
                Parent.GameViews["Inventory"].Render();
            } else if (option == "Shop") {
                Parent.GameViews["Shop"].Render();
            } else if (option == "Save and exit game"){
                FileModule.Serializers.Character(Parent.Player!);
                return;
            };

        }
        Render();
    }

    public Table RenderCharacterStats(Entities.Character character) {
        Table t = new Table().Border(TableBorder.Simple).HideHeaders().Width(60).Title("Character");
        t.AddColumn(new TableColumn("Attributes"));
        t.AddColumn(new TableColumn("Values"));
        t.AddRow(new Text("Name"), new Text($"{character.Name}"));
        t.AddRow(new Text("Experience"), new Text($"{character.Experience}/{character.NextLevel}"));
        t.AddRow(new Text("Level"), new Text($"{character.Level}"));
        t.AddRow(new Text("Health"), new BarChart().WithMaxValue(character.MaxHealth).AddItem($"{character.Health}/{character.MaxHealth}", character.Health, Color.Red).HideValues());
        t.AddRow(new Text("Gold"), new Text($"{character.Gold}"));
        if (character.Effects.Count > 0) {
            t.AddRow(new Text("Effects"), new Text($"{character.EffectList()}"));
        }
        return t;
    }
}