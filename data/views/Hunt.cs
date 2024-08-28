namespace Views;
using Spectre.Console;
using PlaceModule;
using Logging;

public class HuntingPlacesView : View {

    public HuntingPlacesView(RPG.Game game) : base(game) {}
    public override void Render() {
        AnsiConsole.Clear();
        var player = Parent.Player;
        if (player != null)
        {
            var table = new Table()
                    .AddColumn("Hunt")
                    .AddColumn("Score")
                    .AddColumn("Tier");

                table.Columns[0].Width(15);

                foreach (var (key, score) in player.Scores)
                {
                    string tier = score switch
                    {
                        > 100 => "[yellow]★★★[/]",
                        > 25 => "[lightgoldenrod1]★★[/]",
                        _ => "[khaki1]★[/]",
                    };

                    Color barColor = score switch
                    {
                        > 100 => Color.Yellow,
                        > 25 => Color.LightGoldenrod1,
                        _ => Color.Khaki1,
                    };

                    var chart = new BarChart()
                        .HideValues()
                        .WithMaxValue(100)
                        .Width(40)
                        .AddItem($"{score}/100", score, barColor);

                    table.AddRow(new Markup($"{tier.Trim()} {key}"), new Text($"{score}"), chart);
                }
            AnsiConsole.Write(table);
        }

        var selection = new SelectionPrompt<string>();
        selection.Title("Select a hunting place:");
        if (Parent.Places.Keys.Contains("Last Hunt")) {
            selection.AddChoice($"Last Hunt ({Parent.Places["Last Hunt"].Name})");
        }
        // foreach (var hunt in Parent.Places.Values)
        // {
        //     if (player != null)
        //     {
        //         if (player.Scores.Values.Any(h => h > 100))
        //         {
        //             selection.AddChoice($"{hunt.Name} (Boss)");
        //         }

        //     }
        // }
        foreach (string hunt in Parent.Places.Keys) {
            if (hunt.Contains("Last Hunt")) {
                continue;
            }
            selection.AddChoice(hunt);
        }
        selection.AddChoice("Back");
        string option = AnsiConsole.Prompt(selection);
        if (option == "Back") {
            return;
        }
        global::Logging.Debug.Write($"Hunt option: {option}");
        if (option.Contains("Last Hunt"))
        {
            option = "Last Hunt";
        }
        HuntingPlace place = (HuntingPlace)Parent.Places[option];
        RenderPlace(place);
    }

    public void RenderPlace(HuntingPlace place) {
        var player = Parent.Player;
        if (player != null && Parent.GameViews["Fight"] is FightView view)
        {
            int placeScore = player.Scores.GetValueOrDefault(place.Name, 0);
            global::Logging.Debug.Write($"Place: {place}");
            view.Setup(new EntityModule.Party(player), place.NewParty(placeScore), place);
            view.Render();
            UpdateLastHunt(place);
        }
    }

    public void UpdateLastHunt(HuntingPlace place)
    {
        if (!(Parent.Places.Keys.Contains("Last Hunt")))
        {
            Parent.Places.Add("Last Hunt", place);
        }
        else
        {
            Parent.Places["Last Hunt"] = place;
        }
    }
}