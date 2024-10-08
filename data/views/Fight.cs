namespace Views;
using SpellModule;
using CombatModule;
using EntityModule;
using Spectre.Console;
using static System.Linq.Enumerable;

public class FightView : View {
    Party? PlayerParty;
    Party? MonsterParty;
    GameLog Log;
    public PlaceModule.HuntingPlace? FightPlace;


    public FightView(RPG.Game parent) : base(parent) {
        PlayerParty = null;
        MonsterParty = null;
        Log = new GameLog(parent);
    }

    public void Setup(Party playerParty, Party monsterParty, PlaceModule.HuntingPlace place) {
        PlayerParty = playerParty;
        MonsterParty = monsterParty;    
        FightPlace = place;
    }

    public override void Render() {
        var player = Parent.Player;
        Log.ClearLog();
        if (MonsterParty != null && player != null && PlayerParty != null && FightPlace != null)
        {
            while (MonsterParty.Count > 0) {
                Grid grid = new Grid();
                grid.AddColumn(new GridColumn());

                // DRAW TABLE
                AnsiConsole.Clear();

                Table Fight = new Table().Width(60).Title($"{FightPlace.Name} Fight").HideHeaders();
                Fight.AddColumn("Creatures");
                Fight.AddColumn("Health");
                if (Fight.Columns.Count == 2) {
                    foreach(Creature c in MonsterParty.Concat(PlayerParty)) {
                        if (c.Effects.Count() > 0) {
                            Fight.AddColumn("Effects");
                        }
                    }
                }

                foreach (int i in Range(1, PlayerParty.Count)) {
                    Creature creature = PlayerParty[i-1];
                    Text text = new Text($"#{i} {creature.Name}");
                    CreatureToTable(Fight, creature, new Text($"{creature.Name}"));
                }
                Fight.AddEmptyRow();

                foreach (int i in Range(1, MonsterParty.Count)) {
                    Monster monster = (Monster)MonsterParty[i-1];
                    Text text = new Text($"#{i} {monster.Name}");
                    CreatureToTable(Fight, monster, text);
                }

                Log.LoadEvents();

                // Draw grid
                if (Log.Events.Count > 0) {
                    grid.AddColumn(new GridColumn());
                    grid.AddRow(Fight, Log.Table);
                } else {grid.AddRow(Fight);}
                

                if (!player.IsAlive) {
                    Log.Table.AddRow("You lose!");
                    AnsiConsole.Write(grid);
                    System.Environment.Exit(1);
                }
                AnsiConsole.Write(grid);
                Log.ClearEvents();

                string option = Options(MonsterParty, PlayerParty);
                if (option == "Inventory")
                {
                    continue;
                }
                EndTurn(MonsterParty, PlayerParty);
                MonsterTurn(MonsterParty, PlayerParty);
                if (option == "Stun") continue;
                if (option == "Run") return;
                Log.ClearLog();
            }
        }
    }

    public string Options(Party MonsterParty, Party PlayerParty) {
        var player = PlayerParty[0];
        if (!player.Effects.Any(e => e.Name == "Stun"))
        {            
            string option = AnsiConsole.Prompt(
                new SelectionPrompt<string>().AddChoices<string>(new[] {
                    "Attack!",
                    "Cast",
                    "Inventory",
                    "Run",
                    })
            );

            if (option == "Attack!")
            {
                var target = SelectTarget(player, MonsterParty);
                Combat.Attack(player, target);
            }
            else if (option == "Cast")
            {
                Spell spell = SelectSpell(player);
                if (spell.Name == "Back")
                {
                    Options(MonsterParty, PlayerParty);
                }
                else
                {
                    var target = SelectTarget(player, MonsterParty);
                    Combat.Cast(player, target, spell);
                }
            }
            else if (option == "Inventory")
            {
                var view = (InventoryView)Parent.GameViews["Inventory"];
                view.Render();
            }
            return option;
        }
        AnsiConsole.WriteLine("You are stunned.");
        AnsiConsole.WriteLine("");
        AnsiConsole.MarkupLine("[gray][italic]Press any key to conitnue.[/][/]");
        Console.ReadKey();
        Log.ClearLog();
        return "Stun";

    }

    public void MonsterTurn(Party MonsterParty, Party PlayerParty) {
        List<Monster> MonstersToRemove = new List<Monster>();
        foreach (Monster monster in MonsterParty) {
            if (monster.Health <= 0) {
                MonstersToRemove.Add(monster);

                }
            else {monster.StartTurn(PlayerParty[0]);}}

        foreach(Monster monster in MonstersToRemove) {
            var player = (Character)PlayerParty[0];
            MonsterParty.Remove(monster);
            if (FightPlace != null) {
                if (player.Scores.Keys.Contains(FightPlace.Name)) {
                    player.Scores[FightPlace.Name] += 1;
                }
                else {
                    player.Scores.Add(FightPlace.Name, 1);
                }
            }
        }
    }

    public void EndTurn(Party MonsterParty, Party PlayerParty) {
        foreach (Creature creature in MonsterParty.Concat(PlayerParty)) {
            List<Effect> toRemove = new List<Effect>();
            foreach (Effect effect in creature.Effects) {
                effect.Action();
                if (effect.Turns <= 0) {
                    toRemove.Add(effect);
                }
            }
            foreach (Effect effect in toRemove) {
                creature.Effects.Remove(effect);
            }
        }
    }

    public void CreatureToTable(Table table, Creature creature, Text text) {
        BarChart chart = new BarChart().WithMaxValue(creature.MaxHealth).Width(30).AddItem($"{creature.Health}/{creature.MaxHealth}", creature.Health, Color.Red).HideValues();
        if (table.Columns.Count == 2) {
            table.AddRow(text, chart);
        }
        else if (table.Columns.Count == 3) {
            table.AddRow(text, chart, new Text(creature.EffectList));
        }
    }

    public Creature SelectTarget(Creature player, Party monsters) {
        if (monsters.Count == 1) {
            return monsters[0];
        }
        var options = new SelectionPrompt<string>();
        int counter = 1;
        foreach (Creature monster in monsters) {
            options.AddChoice($"#{counter} {monster.Name}");
            counter++;
        }
        string option = AnsiConsole.Prompt(options);
        var target = option[1] - '1';
        return monsters[target];
    }

    public Spell SelectSpell(Creature creature) {
        var player = (Character)creature;
        var selection = new SelectionPrompt<Spell>().Title("Spellbook").UseConverter(s => s.Name);
        foreach (Spell spell in player.Spellbook) {
            selection.AddChoice(spell);
        }
        selection.AddChoice(new BlankSpell("Back"));
        var option = AnsiConsole.Prompt(selection);
        return option;
    }
}