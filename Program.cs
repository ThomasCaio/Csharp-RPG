namespace RPG;
using Entities;
using Items.All;
using Items.Factory;
using Views;
using Places;

public class Test
{
    public static void Run() {
        Build.Run();
    }
}

public static class Build {
    public static void Run() {
        Game g = new Game();
    }
}

public class Game {
    public Character? Player;
    public static List<string> Log = new List<string>();
    public static Random RNG = new Random();

    public Dictionary<string, View> GameViews = new Dictionary<string, View>();
    public Dictionary<string, Place> Places = new Dictionary<string, Place>();
    public ItemFactory itemFactory;

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


        Setup();
    }

    public void PlayerSetUp() {
        if (Player != null){
            itemFactory.New(Player.Inventory, "Demon Sword");
            Player.Equip(Player.Inventory.Get(0));
            Player.Inventory.Add(new ShortSword());
            Player.Scores.Add("Forest", 150);
            Player.Gold = 10;
        }
    }

    public void Setup() {
        GameViews["Menu"].Render();
    }
}
