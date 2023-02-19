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
        Game g = new Game(TestSetup);
        System.Console.WriteLine("DEBUG Mode");
        g.Debug = true;
        g.Run();
    }

    public static void TestSetup(Game game) {
            game.Player!.Name = "Test " + game.Player!.Name;
            game.Player!.Inventory.Add(new DemonSword());
            game.Player!.Equip(game.Player.Inventory.Get(0));
            game.Player!.Scores.Add("Forest", 150);
            game.Player!.Gold = 10;
            game.Player!.Spellbook.Add(new Firebolt());
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

    public Action<Game>? gSetup;
    public bool Debug = false;

    public Game(Action<Game>? func=null) {
        gSetup = func;

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
