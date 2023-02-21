namespace Items.Factory;
using Items.All;

public class ItemFactory
    {
        private readonly Dictionary<ItemTypes, List<Item>> items = new Dictionary<ItemTypes, List<Item>>();

        public ItemFactory()
        {
            this.RegisterItems();
        }

        public IEnumerable<Item> GetItems(ItemTypes itemType)
        {
            return this.items[itemType];
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
            this.RegisterItem(new ShortSword());
            this.RegisterItem(new DemonSword());

            this.RegisterItem(new LeatherHelmet());
        }

        private void RegisterItem(Wearable item)
        {
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