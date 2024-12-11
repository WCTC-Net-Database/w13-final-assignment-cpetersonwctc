using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Services;

namespace ConsoleRpgEntities.Models.Equipments
{
    public class InventoryItemBridge
    {
        public int Id { get; set; }

        // Foreign keys
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        // Navigational properties
        public virtual Item Item { get; set; }
        public virtual Inventory Inventory { get; set; }
    }



}
