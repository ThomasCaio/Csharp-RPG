namespace RPG;
using EntityModule;
using ItemModule.All;
using ItemModule.Factory;
using Views;
using PlaceModule;
using FileModule;
using Logging;
using SpellModule;
using ItemModule;

class Program {
    public static void Main(string[] args) {
        if (args.Contains("--test")) {
            Test.Run();
        }
        else if (args.Contains("--debug"))
        {
            Debug.Run();
        }
        else {
            Build.Run();
        }
    }
}

public class Test
{
    public static void Run()
    {
        FileModule.Test.Run();
    }
}

public class Debug
{
    public static void Run() {
        Game g = new Game(TestSetup, true);
        string now = DateTime.Now.ToString("---- (dd-MM-yyyy HH:mm:ss) ----" + Environment.NewLine);
        Logging.Debug.Write($"{now}");
        g.Run();
    }

    public static void TestSetup(Game game)
    {
        game.Player!.Inventory.Add(new TestSword());
        game.Player!.Equip((Wearable)game.Player.Inventory.Get(0));
        game.Player!.Experience += 100000;
        game.Player!.Scores.Add("Forest", 150);
        game.Player!.Scores.Add("Cavern", 150);
        game.Player!.Gold = 10000;
        game.Player!.Spellbook.Add(new Firebolt());
    }
}

public static class Build
{
    public static void Run()
    {
        Game g = new Game(DefaultSetup);
        g.Run();
    }

    public static void DefaultSetup(Game game)
    {
        game.Player!.Inventory.Add(new Knife());
        game.Player!.Equip((Wearable)game.Player!.Inventory.Get(0));
    }
}

public class Game {
    public string GameName = "[yellow]C# RPG[/]";
    public Character? Player;
    public static List<string> Log = new List<string>();
    public static Random RNG = new Random();

    public Dictionary<string, View> GameViews = new Dictionary<string, View>();
    public Dictionary<string, Place> Places = new Dictionary<string, Place>();
    public ItemFactory itemFactory;

    public Action<Game>? gSetup;
    public static bool Debug;

    public Game(Action<Game>? func=null, bool debug=false) {
        gSetup = func;
        Debug = debug;

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
        Places["Carvern"] = new Cavern(this);
        Places["Carvern2"] = new Cavern2(this);

        // Item Manager
        itemFactory = new ItemFactory();
    }

    public void Run() {
        GameViews["Menu"].Render();

    }
    public void Setup() {
    }
}
