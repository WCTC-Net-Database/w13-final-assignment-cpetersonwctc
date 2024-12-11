using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Equipments;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace ConsoleRpg.Helpers;

public class DevMenu
{
    private readonly OutputManager _outputManager;
    private readonly GameContext _context;
    private readonly MapManager _mapManager;
    public DevMenu(OutputManager outputManager, GameContext context, MapManager mapManager)
    {
        _outputManager = outputManager;
        _context = context;
        _mapManager = mapManager;
    }
    public Player AddPlayer()
    {
        _outputManager.ClearScreen();

        string nameOfPlayer;
        while (true)
        {
            nameOfPlayer = _outputManager.GetUserInput("What will the players name be? (anything): ");
            if (nameOfPlayer != null && nameOfPlayer != "")
            {
                nameOfPlayer = nameOfPlayer.Trim();
                break;
            }
            else
            {
                _outputManager.AddLogEntry($"Please enter a valid name!");
            }
        }
        int healthOfPlayer = promptInt("What will the players health be? (number): ");
        int experienceOfPlayer = promptInt("What will the players experience be? (number): ");
        int manaOfPlayer = promptInt("How much mana will the player have? (number): ");
        var roomsThatExist = _context.Rooms.ToList();
        Room roomOfPlayer = SelectRoom("Choose a room for the player to start in: ", true);
        //while (true)
        //{
        //    _outputManager.AddLogEntry("Choose a room for the player to start in: ");
        //    for (int r = 0; r < roomsThatExist.Count(); r++)
        //    {
        //        Thread.Sleep(500);
        //        _outputManager.AddLogEntry($"{r + 1}: {roomsThatExist.ElementAt(r).Name}");
        //    }
        //    var roomOfPlayerTemp = _outputManager.GetUserInput("What will the players starting room be? (number): ");
        //    try
        //    {
        //        int roomOfPlayerInt = Convert.ToInt32(roomOfPlayerTemp) - 1;
        //        if (roomOfPlayerInt > 0 || roomOfPlayerInt < roomsThatExist.Count())
        //        {
        //            roomOfPlayer = (Room)roomsThatExist.ElementAt(roomOfPlayerInt);
        //            break;
        //        }
        //        else
        //        {
        //            _outputManager.AddLogEntry($"Please enter a valid number!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _outputManager.AddLogEntry($"Please enter a valid number!");
        //    }
        //}

        Equipment newEquipment = new Equipment();
        Player newPlayer = new Player();
        newPlayer.Name = nameOfPlayer;
        newPlayer.Experience = experienceOfPlayer;
        newPlayer.Mana = manaOfPlayer;
        newPlayer.Health = healthOfPlayer;
        newPlayer.MaxHealth = healthOfPlayer;
        newPlayer.Room = roomOfPlayer;

        
        
        _context.Equipments.Add(newEquipment);
        _context.Players.Add(newPlayer);

        _context.SaveChanges();
        newPlayer.EquipmentId = newEquipment.Id;

        Inventory newInventory = new Inventory();
        newInventory.PlayerId = newPlayer.Id;
        _context.Inventories.Add(newInventory);
        _context.SaveChanges();
        
        return newPlayer;
    }

    public void EditPlayer()
    {
        _outputManager.ClearScreen();
        
        Player player;
        while (true)
        {
            for (int p = 0; p < _context.Players.Count(); p++)
            {
                _outputManager.AddLogEntry($"{p+1}. {_context.Players.ToList().ElementAt(p).Name}");
            }

            var nameOfPlayer = _outputManager.GetUserInput("What player will you edit? (number): ");
            if (nameOfPlayer != null && nameOfPlayer != "")
            {
                try
                {
                    player = _context.Players.ToList().ElementAt(Convert.ToInt32(nameOfPlayer)-1);
                    break;
                }catch(Exception e) {
                    _outputManager.AddLogEntry($"Please enter a valid number!");
                }        
            }
            else
            {
                _outputManager.AddLogEntry($"Please enter a valid number!");
            }
        }

        while (true)
        {
            _outputManager.ClearScreen();
            _outputManager.AddLogEntry($"What will you edit for {player.Name}?: (number)");
            _outputManager.AddLogEntry($"1. Name - {player.Name}");
            _outputManager.AddLogEntry($"2. Health - {player.Health}");
            _outputManager.AddLogEntry($"3. Max Health - {player.MaxHealth}");
            _outputManager.AddLogEntry($"4. Experience - {player.Experience}");
            _outputManager.AddLogEntry($"5. Mana - {player.Mana}");
            _outputManager.AddLogEntry($"6. Room - {player.Room.Name}");
            _outputManager.AddLogEntry($"7. Stop Editing");

            var input = _outputManager.GetUserInput("");
            switch (input)
            {
                case "1":
                    player.Name = _outputManager.GetUserInput("What will the new name be?: ");
                    break;
                case "2":
                    player.Health = promptInt("What will the new health be?: ");
                    break;
                case "3":
                    player.MaxHealth = promptInt("What will the new max health be?: ");
                    break;
                case "4":
                    player.Experience = promptInt("What will the new experience be?: ");
                    break;
                case "5":
                    player.Mana = promptInt("What will the new mana be?: ");
                    break;
                case "6":
                    var roomsThatExist = _context.Rooms.ToList();
                    Room newRoom = SelectRoom("Choose a new room for the player: ", true);
                    //while (true)
                    //{
                    //    _outputManager.AddLogEntry("Choose a new room for the player: ");
                    //    for (int r = 0; r < roomsThatExist.Count(); r++)
                    //    {
                    //        //Thread.Sleep(500);
                    //        _outputManager.AddLogEntry($"{r + 1}: {roomsThatExist.ElementAt(r).Name}");
                    //    }
                    //    var roomOfPlayerInt = promptInt("Choose a new room for the player: ")-1;
                    //    if (roomOfPlayerInt > 0 || roomOfPlayerInt < roomsThatExist.Count())
                    //    {
                    //        newRoom = (Room)roomsThatExist.ElementAt(roomOfPlayerInt);
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        _outputManager.AddLogEntry($"Please enter a valid number!");
                    //    }
                    //}
                    player.Room = newRoom;
                    break;
                case "7":
                    break;
                default:
                    _outputManager.AddLogEntry("Invalid selection.");
                    break;
            }
            if (input == "7")
            {
                _outputManager.AddLogEntry($"{player.Name} now has {player.Experience} experience, {player.Mana} mana, {player.Health}/{player.MaxHealth} Health, and is in the {player.Room.Name}.");
                break;
            }
        }
        _context.SaveChanges();
    }

    public void DisplayAllCharacters()
    {
        _outputManager.ClearScreen();
        foreach (Player player in _context.Players.ToList())
        {
            _outputManager.AddLogEntry($" -     {player.Name}. has {player.Experience} experience, {player.Mana} mana, {player.Health} health, and is in the {player.Room.Name}.");
        }
        _outputManager.GetUserInput("Press enter to exit viewing.");
    }

    public void SearchForCharacter()
    {
        _outputManager.ClearScreen();
        var name = _outputManager.GetUserInput("Enter the characters name: ");

        var search = _context.Players.FromSqlRaw($"SELECT TOP 1 * FROM Players WHERE Players.Name LIKE '%{name}%';").ToList();
        if (search.ElementAt(0) != null)
        {
            var player = search.ElementAt(0);
            _outputManager.AddLogEntry($"{player.Name}. has {player.Experience} experience, {player.Mana} Mana, {player.Health} health, and is in the {player.Room.Name}.");
            var items = _context.InventoryItemBridge.Where(b => b.InventoryId == player.Inventory.Id).ToList();
            if (items.Count > 0)
            {
                _outputManager.AddLogEntry($"\nand owns:");
                foreach (InventoryItemBridge item in items)
                {
                    _outputManager.AddLogEntry($"   {item.Item.Type} - {item.Item.Name} (Damage: {item.Item.Attack}, Defense: {item.Item.Defense}, Value: {item.Item.Value})");
                }
            }
            else
            {
                _outputManager.AddLogEntry("\nand has no items.");
            }

            if (player.Abilities!=null && player.Abilities.Count > 0)
            {
                _outputManager.AddLogEntry("\nas well as abilities:");
                foreach (Ability ability in player.Abilities)
                {
                    if (ability is AttackAbility attackAbility)
                    {
                        _outputManager.AddLogEntry($"   Attack Ability - {ability.Name} (Damage: {attackAbility.Damage}, Range: {ability.Distance})");
                    }
                    if (ability is DefenseAbility defenseAbility)
                    {
                        _outputManager.AddLogEntry($"   Defense Ability - {ability.Name} (Damage: {defenseAbility.DefenseBonus}, Range: {ability.Distance})");
                    }
                    if (ability is MagicAbility magicAbility)
                    {
                        var abilityStatement = $"   Magic Ability - {ability.Name} (";
                        if (magicAbility.Damage > 0)
                        {
                            abilityStatement += $"Damage: {magicAbility.Damage} ";
                        }
                        if (magicAbility.Defense > 0)
                        {
                            abilityStatement += $"Defense: {magicAbility.Defense} ";
                        }
                        if (magicAbility.HealsFor > 0)
                        {
                            abilityStatement += $"Heals: {magicAbility.HealsFor} ";
                        }
                        if (magicAbility.ManaCost > 0)
                        {
                            abilityStatement += $"Mana Cost: {magicAbility.ManaCost}";
                        }
                        abilityStatement += ")";
                        _outputManager.AddLogEntry(abilityStatement);
                    }
                }
            }
            else
            {
                _outputManager.AddLogEntry("\nas well as no abilities.");
            }
        }
        else
        {
            _outputManager.AddLogEntry("No player found.");
        }

        _outputManager.GetUserInput("Press enter to exit viewing.");

    }

    public void CreateNewAbility()
    {
        _outputManager.ClearScreen();
        var abilityName = _outputManager.GetUserInput("What will the ability name be? (anything):");
        
        var allPlayers = _context.Players.ToList();
        Player player;
        for (int p = 0; p < allPlayers.Count; p++)
        {
            _outputManager.AddLogEntry($"{p + 1}. {allPlayers[p].Name}");
        }
        while (true)
        {
            var chosen = promptInt("What player will the ability be made for? (number): ")-1;
            if (chosen > 0 && chosen < allPlayers.Count)
            {
                player = allPlayers[chosen];
                break;
            }
            else
            {
                _outputManager.AddLogEntry("Choose a valid player....");
            }
        }
        
        var abilityDesc = _outputManager.GetUserInput("What will the description be? (anything):");
        var abilityDist = promptInt("What will the range be? (number):");

        _outputManager.AddLogEntry("1. Attack Ability");
        _outputManager.AddLogEntry("2. Defense Ability");
        _outputManager.AddLogEntry("3. Magic Ability");
        while (true)
        {
            var input = _outputManager.GetUserInput("What type of ability will it be?: (number)");
            switch (input)
            {
                case "1":
                    var attackDamage = promptInt("How much damage will it do? (number)");
                    AttackAbility attackAbility = new AttackAbility {Name = abilityName, Description = abilityDesc, Distance = abilityDist, Damage = attackDamage, AbilityType = "AttackAbility"};
                    _context.Abilities.Add(attackAbility);
                    player.Abilities.Add(attackAbility);
                    break;
                case "2":
                    var defenseDefense = promptInt("How much defense will it give? (number)");
                    DefenseAbility defenseAbility = new DefenseAbility { Name = abilityName, Description = abilityDesc, Distance = abilityDist, DefenseBonus = defenseDefense, AbilityType = "DefenseAbility" };
                    _context.Abilities.Add(defenseAbility);
                    player.Abilities.Add(defenseAbility);
                    break;
                case "3":
                    var magicDamage = promptInt("How much damage will it do? (number)");
                    var magicDefense = promptInt("How much defense will it give? (number)");
                    var magicHeal = promptInt("How much health will it heal? (number)");
                    var magicCost = promptInt("How much mana will it cost? (number)");
                    MagicAbility magicAbility = new MagicAbility { Name = abilityName, Description = abilityDesc, Distance = abilityDist, Damage = magicDamage, Defense = magicDefense, HealsFor = magicHeal, ManaCost = magicCost, AbilityType = "MagicAbility" };
                    _context.Abilities.Add(magicAbility);
                    player.Abilities.Add(magicAbility);
                    break;
                default:
                    _outputManager.AddLogEntry("Invalid selection. Please choose 1, 2, or 3.");
                    break;
            }
            if(input=="1"|| input == "2" || input == "3")
            {
                break;
            }
        }

        _outputManager.AddLogEntry($"The ability {abilityName} has been created!");
        Thread.Sleep(750);
        _context.SaveChanges();
    }

    public void CreateNewRoom()
    {
        _outputManager.ClearScreen();
        var roomName = _outputManager.GetUserInput("What will the rooms name be? (anything):");
        var roomDesc = _outputManager.GetUserInput("What will the rooms description be? (anything):");

        _outputManager.AddLogEntry("Position of room will be made by creating connections to other rooms.");
        
        Room north = null;
        Room east = null;
        Room south = null;
        Room west = null;

        while (true)
        {
            var chosenRoom = SelectRoom("What room will be North?: ", false);
            if (chosenRoom == null)
            {
                break;
            }
            else
            {
                if (chosenRoom.South == null)
                {
                    north = chosenRoom;
                    break;
                }
                else
                {
                    _outputManager.AddLogEntry("Thats not possible to do!");
                    Thread.Sleep(250);
                }
            }
        }
        while (true)
        {
            var chosenRoom = SelectRoom("What room will be East?: ", false);
            if (chosenRoom == null)
            {
                break;
            }
            else
            {
                if (chosenRoom.West == null)
                {
                    east = chosenRoom;
                    break;
                }
                else
                {
                    _outputManager.AddLogEntry("Thats not possible to do!");
                    Thread.Sleep(250);
                }
            }
        }
        while (true)
        {
            var chosenRoom = SelectRoom("What room will be South?: ", false);
            if (chosenRoom == null)
            {
                break;
            }
            else
            {
                if (chosenRoom.North == null)
                {
                    south = chosenRoom;
                    break;
                }
                else
                {
                    _outputManager.AddLogEntry("Thats not possible to do!");
                    Thread.Sleep(250);
                }
            }
        }
        while (true)
        {
            var chosenRoom = SelectRoom("What room will be West?: ", false);
            if (chosenRoom == null)
            {
                break;
            }
            else
            {
                if (chosenRoom.East == null)
                {
                    west = chosenRoom;
                    break;
                }
                else
                {
                    _outputManager.AddLogEntry("Thats not possible to do!");
                    Thread.Sleep(250);
                }
            }
        }

        Room newRoom = new Room() { Name = roomName, Description = roomDesc, North = north, East = east, South = south, West = west};

        if(north != null)
            north.South = newRoom;
        if(east != null)
            east.West = newRoom;
        if (south != null)
            south.North = newRoom;
        if (west != null)
            west.East = newRoom;

        _context.Rooms.Add(newRoom);
        _context.SaveChanges();
    }

    public void SearchForRoom()
    {
        _outputManager.ClearScreen();
        var name = _outputManager.GetUserInput("Enter the room name: ");

        var search = _context.Rooms.FromSqlRaw($"SELECT TOP 1 * FROM Rooms WHERE Rooms.Name LIKE '%{name}%';").ToList();
        if (search.ElementAt(0) != null)
        {
            Room foundRoom = search.ElementAt(0);
            _mapManager.LoadInitialRoom(foundRoom.Id);
            _mapManager.DisplayMap();
            _outputManager.AddLogEntry($"{foundRoom.Name} - {foundRoom.Description}");
            var playersInRoom = _context.Players.Where(p => p.RoomId == foundRoom.Id).ToList();
            if (playersInRoom.Count > 0)
            {
                _outputManager.AddLogEntry("\nRoom contains players: ");
                foreach (var player in playersInRoom)
                {
                    _outputManager.AddLogEntry($"   - {player.Name}");
                }
            }
            else
            {
                _outputManager.AddLogEntry("\nRoom contains no players.");
            }
            var monstersInRoom = _context.Monsters.Where(m => m.RoomId == foundRoom.Id).ToList();
            if (monstersInRoom.Count > 0)
            {
                _outputManager.AddLogEntry("\nand contains monsters: ");
                foreach (var monster in monstersInRoom)
                {
                    _outputManager.AddLogEntry($" {monster.MonsterType} - {monster.Name}");
                }
            }
            else
            {
                _outputManager.AddLogEntry("\nand contains no monsters.");
            }
        }
        _outputManager.GetUserInput("\nPress enter to exit viewing.");
    }

    public void SearchForCharacterInRoom()
    {
        _outputManager.ClearScreen();
        var room = SelectRoom("What room are you searching in?: ", true);
        _outputManager.ClearScreen();
        _outputManager.AddLogEntry("1. Name");
        _outputManager.AddLogEntry("2. Health");
        _outputManager.AddLogEntry("3. Max Health");
        _outputManager.AddLogEntry("4. Experience");
        _outputManager.AddLogEntry("5. Mana");
        var choice = _outputManager.GetUserInput("What stat are you searching with?: ");
        List<Player> results;
        switch (choice)
        {
            case "1":
                var search1 = _outputManager.GetUserInput("What is the name?: ");
                results = _context.Players.FromSqlRaw($"SELECT * FROM Players WHERE Players.Name LIKE '%{search1}%';").ToList();
                break;
            case "2":
                var search2 = promptInt("What is the health?: ");
                results = _context.Players.Where(p => p.Health == search2).ToList();
                break;
            case "3":
                var search3 = promptInt("What is the max health?: ");
                results = _context.Players.Where(p => p.MaxHealth == search3).ToList();
                break;
            case "4":
                var search4 = promptInt("What is the experience?: ");
                results = _context.Players.Where(p => p.Experience == search4).ToList();
                break;
            case "5":
                var search5 = promptInt("What is the mana?: ");
                results = _context.Players.Where(p => p.Mana == search5).ToList();
                break;
            default:
                results = _context.Players.ToList();
                break;
        }
        _outputManager.AddLogEntry($"\nI found player(s): ");
        foreach (Player player in results)
        {
            _outputManager.AddLogEntry($"   - {player.Name}");
        }
        _outputManager.GetUserInput("Press Enter To Continue");
    }

    public void GroupPlayersByRoom()
    {
        var allPlayers = _context.Players.ToList();
        var allRooms = _context.Rooms.ToList();
        var playerGroups = allPlayers.GroupBy(p => p.RoomId, (RoomId) => new {Key = RoomId});
        foreach (var room in playerGroups)
        {
            _outputManager.ClearScreen();
            _outputManager.AddLogEntry($"{allRooms.Where(r => r.Id == room.Key).FirstOrDefault().Name}");
            var playersInRoom = allPlayers.Where(p => p.RoomId == room.Key);
            foreach (var player in playersInRoom)
            {
                _outputManager.AddLogEntry($"   - {player.Name}");
            }
        }
        _outputManager.GetUserInput("Press Enter To Exit");
    }

    public void GetPlayerFromItem()
    {
        var search = _outputManager.GetUserInput("What item will you search for?: ");
        var result = _context.Items.FromSqlRaw($"SELECT * FROM Items WHERE Items.Name LIKE '%{search}%';").ToList().FirstOrDefault();
        if (result != null)
        {
            var bridges = _context.InventoryItemBridge.Where(b => b.ItemId == result.Id).ToList();
            if(bridges.Count > 0)
            {
                foreach (var bridge in bridges)
                {
                    var inventories = _context.Inventories.Where(p => p.Id == bridge.InventoryId).ToList();
                    foreach (var inventory in inventories)
                    {
                        var players = _context.Players.Where(p => p.Id == inventory.PlayerId).ToList();
                        _outputManager.AddLogEntry($"{result.Name} is owned by: ");
                        foreach (var player in players)
                        {
                            _outputManager.AddLogEntry($"   - {player.Name}");
                        }
                    }
                }
            }
            else
            {
                _outputManager.AddLogEntry($"No one has {result.Name}.");
            }
        }
        else
        {
            _outputManager.AddLogEntry($"{search} does not exist.");
        }
        _outputManager.GetUserInput("Press Enter To Exit");
    }
    private Room SelectRoom(String prompt, bool required)
    {
        var allRooms = _context.Rooms.ToList();
        var selectedRoom = allRooms[0];
        _mapManager.LoadInitialRoom(0);
        _mapManager.DisplayMap();

        while (true)
        {
            _mapManager.DisplayMap();
            _outputManager.ClearScreen();
            _outputManager.AddLogEntry(prompt);
            _outputManager.AddLogEntry("(USE NUMPAD)\n8. North\n6. East\n2. South\n4. West\n5. Select");
            if (!required)
            {
                _outputManager.AddLogEntry("0.Skip");
            }
            var keyDown = Console.ReadKey().Key.ToString();
           // _outputManager.AddLogEntry(keyDown);
            switch (keyDown)
            {
                case "UpArrow":
                    if (selectedRoom.North != null)
                    {
                        selectedRoom = selectedRoom.North;
                        _mapManager.UpdateCurrentRoom(selectedRoom.Id);
                    }
                    break;
                case "RightArrow":
                    if (selectedRoom.East != null)
                    {
                        selectedRoom = selectedRoom.East;
                        _mapManager.UpdateCurrentRoom(selectedRoom.Id);
                    }
                    break;
                case "DownArrow":
                    if (selectedRoom.South != null)
                    {
                        selectedRoom = selectedRoom.South;
                        _mapManager.UpdateCurrentRoom(selectedRoom.Id);
                    }
                    break;
                case "LeftArrow":
                    if (selectedRoom.West != null)
                    {
                        selectedRoom = selectedRoom.West;
                        _mapManager.UpdateCurrentRoom(selectedRoom.Id);
                    }
                    break;
                case "Clear":
                    return selectedRoom;
                    break;
                case "Insert":
                    if (!required)
                    {
                        selectedRoom = null;
                        return selectedRoom;
                    }
                    break;
                default:
                    _outputManager.AddLogEntry("Retry Please.");
                    Thread.Sleep(250);
                    break;
            }
        }
    }
    private int promptInt(String prompt)
    {
        while (true)
        {
            var response = _outputManager.GetUserInput(prompt);
            if (response != null)
            {
                try
                {
                    return Convert.ToInt32(response);
                }
                catch (Exception ex)
                {
                    _outputManager.AddLogEntry($"Please enter a valid number!");
                }
            }
            else
            {
                _outputManager.AddLogEntry($"Please enter a valid number!");
            }
        }
    }
}
