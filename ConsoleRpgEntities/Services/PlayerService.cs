using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Equipments;
using ConsoleRpgEntities.Services;

public class PlayerService 
{
    private readonly IOutputService _outputService;
    private readonly AbilityService _abilityService;

    public PlayerService(IOutputService outputService, AbilityService abilityService)
    {
        _outputService = outputService;
        _abilityService = abilityService;
    }

    public void Attack(IPlayer player, ITargetable target)
    {
        if (player.Equipment?.Weapon == null)
        {
            _outputService.WriteLine($"{player.Name} has no weapon equipped!");
            return;
        }

        _outputService.WriteLine($"{player.Name} attacks {target.Name} with a {player.Equipment.Weapon.Name} dealing {player.Equipment.Weapon.Attack} damage!\n");
        target.Health -= player.Equipment.Weapon.Attack;
        _outputService.WriteLine($"{target.Name} has {target.Health} health remaining.\n");
    }

    public (String Status, int Amount, int Duration) UseAbility(IPlayer player, ITargetable target)
    {

        var playerAbilities = player.Abilities.ToList();
        _outputService.WriteLine("Abilities: ");
        if (playerAbilities.Any())
        {
            for (int a = 0; a < playerAbilities.Count; a++)
            {
                _outputService.WriteLine($"{a + 1}. {playerAbilities[a].Name} - {playerAbilities[a].Description}");
            }
            _outputService.WriteLine("What ability will you use?: \n");
            var chosenAbility = Console.ReadLine();
            try
            {
                var toInt = Convert.ToInt32(chosenAbility) - 1;
                if (toInt >= 0 && toInt < playerAbilities.Count())
                {
                    return _abilityService.Activate(playerAbilities[toInt], player, target);
                }
            }
            catch { }
        }
        else
        {
            _outputService.WriteLine("You have no abiliies...");
        }
        return ("NA",0,0);
    }

    public void resetPlayer(Player player)
    {
        if (player.Equipment != null)
        {
            player.Equipment.Weapon = null;
            player.Equipment.Armor = null;
        }
        if (player.Inventory.BridgeItems != null)
        {
            player.Inventory.BridgeItems.Clear();
        }
        
        player.Mana = 0;
        player.MaxHealth = 100;
        player.Health = 100;
        player.Experience = 0;
        player.Abilities.Clear();
        player.Room = null;
        player.RoomId = 0;
    }
    //public void EquipItemFromInventory(IPlayer player, Item item)
    //{
    //    if (player.Inventory?.Items.Contains(item) == true)
    //    {
    //        player.Equipment?.EquipItem(item);
    //    }
    //    else
    //    {
    //        _outputService.WriteLine($"{player.Name} does not have the item {item.Name} in their inventory!");
    //    }
    //}
}
