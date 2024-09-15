namespace ItemModule.Factory;
using ItemModule.All;
using RPG;

public class ItemFactory
    {
        private readonly Dictionary<ItemTypes, List<Item>> items = new Dictionary<ItemTypes, List<Item>>();

        public ItemFactory()
        {
            this.RegisterItems();
        }

        public List<Item> GetItems(ItemTypes itemType)
        {
            Logging.Debug.Write($"{itemType}", "factory");
            var itemList = this.items.Values
            .SelectMany(items => items)
            .Where(item => item is Item i && i.ItemType == itemType)
            .ToList();
            return itemList;
        }

        public Item CreateItem(string itemName)
        {
            var item = this.items.Values.SelectMany(x => x)
                                         .FirstOrDefault(x => x.Name == itemName);
            if (item == null)
            {
                throw new ArgumentException($"Item {itemName} not found");
            }

            return item.Clone();
        }

        private void RegisterItems()
        {
            // DEBUG
            if (RPG.Game.Debug)
            {
                this.RegisterItem(new TestSword());
            }

            // Weapon
            // Axes
            

            // Swords
            this.RegisterItem(new Knife());
            this.RegisterItem(new LongSword());
            this.RegisterItem(new ShortSword());

            // Clubs
            this.RegisterItem(new Mace());
            this.RegisterItem(new WarHammer());

            // Equipments
            this.RegisterItem(new LeatherHelmet());
            this.RegisterItem(new LeatherArmor());
            this.RegisterItem(new LeatherLegs());
            this.RegisterItem(new LeatherBoots());

            // Shields
            this.RegisterItem(new WoodenShield());

            // Consumables
            this.RegisterItem(new SmallHealthPotion());
        }

        private void RegisterItem(Item item)
        {
            Logging.Debug.Write($"{item.ItemType}", "factory");
            this.AddItem(item.ItemType, item);
        }

        private void AddItem(ItemTypes type, Item item)
        {
            if (!this.items.ContainsKey(type))
            {
                this.items[type] = new List<Item>();
            }

            this.items[type].Add(item);
        }
}