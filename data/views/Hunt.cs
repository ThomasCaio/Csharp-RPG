namespace Views;
using Spectre.Console;
using Places;

public class HuntingPlacesView : View {

    public HuntingPlacesView(RPG.Game game) : base(game) {}
    public override void Render() {
        AnsiConsole.Clear();
        var player = Parent.Player;
        if (player != null) {
            if (player.Scores.Keys.Count() > 0 ) {
                var table = new Table();
                table.AddColumn("Hunt");
                table.AddColumn("Score");
                table.AddColumn("Tier");
                table.Columns[0].Width(15);
                foreach (string key in player.Scores.Keys) {
                    int score = player.Scores[key];
                    string tier = (score > 100) ? "[yellow]★★★[/]" : (score > 25) ? "[lightgoldenrod1]★★[/] " : "[khaki1]★[/]  ";
                    int inttier = (score > 100) ? 3 : (score > 25) ? 2 : 1;
                    Color barcolor = (score > 100) ? Color.Yellow : (score > 25) ? Color.LightGoldenrod1 : Color.Khaki1;
                    var chart = new BarChart().HideValues().WithMaxValue(100).Width(40).AddItem($"{score}/100", score, barcolor);
                    table.AddRow(new Markup($"{tier.Trim()} {key}"), new Text($"{player.Scores[key]}"), chart);
                }
                AnsiConsole.Write(table);
            }
        }
        var selection = new SelectionPrompt<string>();
        selection.Title("Select a hunting place:");
        if (Parent.Places.Keys.Contains("Last Hunt")) {
            selection.AddChoice($"Last Hunt");
        }
        foreach (string hunt in Parent.Places.Keys) {
            if (hunt == "Last Hunt") {
                continue;
            }
            selection.AddChoice(hunt);
        }
        selection.AddChoice("Back");
        string option = AnsiConsole.Prompt(selection);
        if (option == "Back") {
            return;
        }
        HuntingPlace place = (HuntingPlace)Parent.Places[option];
        RenderPlace(place);
    }

    public void RenderPlace(HuntingPlace place) {
        var player = Parent.Player;
        if (player != null) {
            var view = (FightView)Parent.GameViews["Fight"];
            int placeScore = player.Scores.GetValueOrDefault(place.Name, 0);
            view.Setup(new Entities.Party(player), place.NewParty(placeScore), place);
            view.Render();
            if (!(Parent.Places.Keys.Contains("Last Hunt"))) {
                Parent.Places.Add("Last Hunt", place);
            } else {Parent.Places["Last Hunt"] = place;}
        }
    }
}