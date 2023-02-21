namespace Entities;
using Items;
using CombatModule;
using Spells;
using static System.Linq.Enumerable;
using RPG;

public enum CreatureType {
    Character,
    Monster
}

public abstract class Creature {
    private double maxHealth = 100;
    private double health = 100;

    public string Name { get; set; }
    public CreatureType Type { get; set; }
    public double BaseDamage { get; set; } = 1;
    public double BaseDefense { get; set; } = 0;
    public List<Passive> Passives { get; } = new List<Passive>();
    public List<Effect> Effects { get; } = new List<Effect>();

    public double Health {
        get { return health; }
        set { health = Math.Max(0, Math.Min(value, maxHealth)); }
    }

    public double MaxHealth {
        get { return maxHealth; }
        set {
            maxHealth = value;
            health = Math.Min(health, maxHealth);
        }
    }

    public virtual double Damage { get { return BaseDamage + DamageModifier; } }
    public virtual double DamageModifier { get { return 0; } }

    public virtual double Defense { get { return BaseDefense + DefenseModifier; } }
    public virtual double DefenseModifier { get { return 0; } }

    public List<Passive> OnHitPassives { get { return Passives.Where(p => p.Type == PassiveType.OnHit).ToList(); } }

    public string EffectList {
        get { return Effects.Count == 0 ? "" : string.Join(", ", Effects); }
    }

    public Creature(string name, CreatureType type) {
        Name = name;
        Type = type;
    }
}


public class Inventory {
    public List<Item> Items = new List<Item>();
    public int InventorySize = 20;
    public Inventory() {}
    public void Add(Item item) {
        if (Items.Count > InventorySize){
            RPG.Game.Log.Add("Your inventory is full");
            return;
        }
        Items.Add(item);
    }
    public void Remove(Item item) {
        if (!Items.Contains(item)) {
            RPG.Game.Log.Add($"Your inventory don't contains {item.Name}.");
            return;
        }
        Items.Remove(item);
    }

    public Item Get(int index) {
        return Items[index];
    }
}

public class Character : Creature
{
    public double NextLevel = 100;
    public double _experience = 0;

    public List<Spell> Spellbook = new List<Spell>();
    public double Experience {
        get{return Convert.ToInt32(_experience);}
        set{
            while (value >= NextLevel) {
                value -= NextLevel;
                LevelUP();
                Game.Log.Add($"{this.Name} level up!");
                NextLevel = (int)Math.Round(NextLevel * 1.25, 0);
            }
            _experience = value;
        }}
    public int Level = 1;
    public int Gold = 0;
    public Dictionary<ItemSlot, Item?> Body = new Dictionary<ItemSlot, Item?>
    {
        [ItemSlot.Head] = null,
        [ItemSlot.Chest] = null,
        [ItemSlot.Legs] = null,
        [ItemSlot.Boots] = null,
        [ItemSlot.MainHand] = null,
        [ItemSlot.OffHand] = null,
    };
    public Inventory Inventory = new Inventory();

    public Dictionary<string, int> Scores = new Dictionary<string, int>();
    public Character(string name) : base(name, CreatureType.Character) {
        
    }

    public void LevelUP() {
        Level++;
        MaxHealth += 5;
        Health += 5;
    }

    public override double DamageModifier
    { 
        get
        {
            if (Body[ItemSlot.MainHand] is Weapon weapon)
            {
                return weapon.BaseDamage;
            }
            return 0;
        }
        
    }

    public void Equip(Item item) {
        Item? equipped = Body.GetValueOrDefault(item.Slot, null);
        if (equipped == null) {
            Body[item.Slot] = item;
            Inventory.Remove(item);
            Game.Log.Add($"{item.Name} equipped.");
        }
        else {
            Unequip(equipped);
            Equip(item);
        }
    }

    public void Unequip(Item item) {
        Item? equipped = Body[item.Slot];
        if (equipped != null) {
            Body[item.Slot] = null;
            Inventory.Add(item);
            Game.Log.Add($"{item.Name} unequipped.");
            return;
        }
        RPG.Game.Log.Add("You have no item on this slot.");
    }

    public bool IsAlive => Health > 0;
}

public class Monster : Creature {

    public Tuple<int, int> DropGold;
    public int DropExp;
    public Monster(string name) : base(name, CreatureType.Monster) {
        DropGold = new Tuple<int, int>(0, 0);
        DropExp = 0;
    }

    public int GetGoldDrop() {
        int value = RPG.Game.RNG.Next(DropGold.Item1, DropGold.Item2);
        return value;
    }

    public virtual void StartTurn(Creature target) {
        Combat.Attack(this, target);
    }
}

public class Party : List<Creature> {
    public Party() {}
    public Party(Creature creature) {
        this.Add(creature);
    }
}