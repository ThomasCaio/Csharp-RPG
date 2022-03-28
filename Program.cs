namespace RPG;
using Entities;
using Items.All;
using Items.Factory;
using Views;
using Places;


class Program {
    public static void Main(string[] args) {
        if (args.Contains("--test")) {
            Test.Run();
        }
        else {
            Build.Run();
        }
    }
}

public class Test
{
    public static void Run() {
        Game g = new Game();
        System.Console.WriteLine("DEBUG Mode");
        g.Debug = true;
        g.Run();
    }

    public static void PlayerTest(Character player) {
            player.Inventory.Add(new DemonSword());
            player.Equip(player.Inventory.Get(1));
            player.Scores.Add("Forest", 150);
            player.Gold = 10;
    }
}

public static class Build {
    public static void Run() {
        Game g = new Game();
        g.Run();
    }
}

public class Game {
    public Character? Player;
    public static List<string> Log = new List<string>();
    public static Random RNG = new Random();

    public Dictionary<string, View> GameViews = new Dictionary<string, View>();
    public Dictionary<string, Place> Places = new Dictionary<string, Place>();
    public ItemFactory itemFactory;
    public bool Debug = false;

    public Game() {
        Player = null;

        // Register Views
        GameViews["Menu"] = new MenuView(this);
        GameViews["Game"] = new GameView(this);
        GameViews["Hunt"] = new HuntingPlacesView(this);
        GameViews["Character"] = new CharacterView(this);
        GameViews["Fight"] = new FightView(this);
        GameViews["Inventory"] = new InventoryView(this);
        GameViews["Shop"] = new ShopView(this);

        // Register Hunting Areas
        Places["Forest"] = new Forest(this);
        Places["Forest"] = new Forest(this);
        Places["Forest"] = new Forest(this);

        // Item Manager
        itemFactory = new ItemFactory(this);
    }

    public void PlayerSetUp(Action<Character> func) {
        if (Player != null){
            func(Player);
        }
    }

    public void Run() {
        GameViews["Menu"].Render();

    }

    public void Setup() {
    }
}
