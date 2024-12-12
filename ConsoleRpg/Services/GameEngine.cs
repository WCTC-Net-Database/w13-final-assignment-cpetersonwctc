using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly MapManager _mapManager;
    private readonly PlayerService _playerService;
    private readonly OutputManager _outputManager;
    private readonly DevMenu _devMenu;
    private Table _logTable;
    private Panel _mapPanel;

    private Player _player;
    //private IMonster _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, MapManager mapManager, PlayerService playerService, OutputManager outputManager, DevMenu devMenu)
    {
        _menuManager = menuManager;
        _mapManager = mapManager;
        _playerService = playerService;
        _outputManager = outputManager;
        _context = context;
        _devMenu = devMenu;
    }

    public void Run()
    {
        if (_menuManager.ShowMainMenu())
        {
            SetupGame();
        }
    }

    private void GameLoop()
    {
        while (true)
        {
            _mapManager.UpdateCurrentRoom(_player.RoomId);
            _mapManager.DisplayMap();
            _outputManager.ClearScreen();
            _outputManager.AddLogEntry($"{_player.Name} is at {_player.Health}/{_player.MaxHealth} health.");
            var monstersInRoom = _context.Monsters.Where(m => m.RoomId == _player.RoomId).ToList();
            _outputManager.AddLogEntry($"{_player.Room.Name} has {monstersInRoom.Where(m => m.Health > 0).Count()} monsters.");
            foreach( var monster in _context.Monsters.Where(m => m.RoomId == _player.RoomId).ToList())
            {
                if (monster.Health > 0)
                {
                    _outputManager.AddLogEntry($"   {monster.MonsterType} - {monster.Name}");
                }
            }

            _outputManager.AddLogEntry("\n1. Attack (You can't run away from battles!)");
            _outputManager.AddLogEntry("2. Move");
            _outputManager.AddLogEntry("3. Quit");
            var input = _outputManager.GetUserInput("Choose an action:");
            _outputManager.ClearScreen();

            switch (input)
            {
                case "1":
                    AttackCharacter();
                    break;
                case "2":
                    _player.RoomId = MoveTo(_player.RoomId).Id;
                    _context.SaveChanges();
                    break;
                case "3":
                    _outputManager.AddLogEntry("Exiting game...");

                    //reset monsters on exit
                    foreach (Monster monster in _context.Monsters.ToList())
                    {
                        monster.Health = monster.MaxHealth;
                    }

                    _context.SaveChanges();
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.AddLogEntry("Invalid selection. Please choose 1.");
                    break;
            }
        }
    }

    private void AttackCharacter()
    {
        
        var monstersInRoom = _context.Monsters.Where(m => m.RoomId == _player.RoomId).ToList();
        String status = "NA";
        int statusAmount = 0;

        while (true)
        {
            _outputManager.ClearScreen();
            _outputManager.AddLogEntry($"{_player.Name} - ({_player.Health}/{_player.MaxHealth} Health)");
            var livingMonsters = monstersInRoom.Where(m => m.Health > 0).ToList();
            if (livingMonsters.Count() > 0)
            {
                _outputManager.AddLogEntry("Monsters Alive: ");
                for (int m = 0; m < livingMonsters.Count(); m++)
                {
                    _outputManager.AddLogEntry($"{m + 1}. {livingMonsters[m].MonsterType} - {livingMonsters[m].Name} ({livingMonsters[m].Health}/{livingMonsters[m].MaxHealth} Health)");
                }


                var attackChoice = _outputManager.GetUserInput("\nWhat monster will you attack?: ");
                try
                {
                    var toInt = Convert.ToInt32(attackChoice) - 1;
                    if (toInt >= 0 && toInt < livingMonsters.Count())
                    {
                        if (livingMonsters[toInt] is ITargetable monster)
                        {
                            _outputManager.AddLogEntry("\n1. Use Ability\n2. Use Main Weapon");
                            var actionChoice = _outputManager.GetUserInput("What will you do?:");
                            _outputManager.ClearScreen();
                            switch (actionChoice)
                            {
                                case "1":
                                    var Data = _playerService.UseAbility(_player, monster);
                                    status = Data.Status;
                                    statusAmount = Data.Amount;
                                    break;
                                case "2":
                                    _playerService.Attack(_player, monster);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                catch { }
            }
            else
            {
                break;
            }
            livingMonsters = monstersInRoom.Where(m => m.Health > 0).ToList();
            if (livingMonsters.Count() > 0)
            {
                foreach (var monster in livingMonsters)
                {
                    if (_player.Equipment != null && _player.Equipment.Armor != null)
                    {
                        if(monster.BaseDamage - (_player.Equipment.Armor.Defense + statusAmount)<1)
                        {
                            _outputManager.AddLogEntry($"{_player.Name}'s defense to high, no damage taken...\n");
                        }
                        else
                        {
                            _player.Health -=  monster.BaseDamage - (_player.Equipment.Armor.Defense + statusAmount);
                            _outputManager.AddLogEntry($"{monster.Name} hit {_player.Name} for {monster.BaseDamage - (_player.Equipment.Armor.Defense + statusAmount)}\n");
                        }
                    }
                    else
                    {
                        _player.Health -= monster.BaseDamage - statusAmount;
                        _outputManager.AddLogEntry($"{monster.Name} hit {_player.Name} for {monster.BaseDamage - statusAmount}\n");
                    }
                }
            }
            status = "NA";
            statusAmount = 0;

            if(_player.Health >= 0)
            {
                _outputManager.AddLogEntry($"\n\n{_player.Name} - ({_player.Health}/{_player.MaxHealth} Health)");
                _outputManager.GetUserInput("Press Enter To Continue.");
            }
            else
            {
                _outputManager.AddLogEntry($"\n\n{_player.Name} - ({_player.Health}/{_player.MaxHealth} Health)");
                _outputManager.AddLogEntry($"{_player.Name} has died...");
                _context.Players.Remove(_player);
                foreach (Monster monster in _context.Monsters.ToList())
                {
                    monster.Health = monster.MaxHealth;
                }

                _context.SaveChanges();
                Thread.Sleep(1000);
                Environment.Exit(0);
                break;
            }
            
        }

    }

    private void SetupGame()
    {
        var AllPlayers = _context.Players.ToList();
        while (true)
        {
            _outputManager.ClearScreen();
            for(int p = 0; p<AllPlayers.Count(); p++)
            {
                _outputManager.AddLogEntry($"{p + 1}. {AllPlayers.ElementAt(p).Name}");
            }
            var chosenPlayer = _outputManager.GetUserInput("Who do you want to play as: ");
            try
            {
                int chosen = Convert.ToInt32(chosenPlayer)-1;
                if (chosen >= 0 && chosen < AllPlayers.Count())
                {
                    _player = AllPlayers[chosen];
                    break;
                }
                else
                {
                    _outputManager.AddLogEntry("bruh");
                }
            }
            catch (Exception e) {
                _outputManager.AddLogEntry("Did you even enter a number??");
            }
        }

        //_player = _context.Players.FirstOrDefault();
        _outputManager.AddLogEntry($"{_player.Name} has entered the game.");

        //LoadMonsters();

        // Load map
        _mapManager.LoadInitialRoom(_player.RoomId);
        _mapManager.DisplayMap();

        // Pause before starting the game loop
        Thread.Sleep(500);
        _outputManager.ClearScreen();
        GameLoop();
    }

    private Room MoveTo(int startingRoom)
    {
        var allRooms = _context.Rooms.ToList();
        var selectedRoom = allRooms[startingRoom];
        _mapManager.LoadInitialRoom(startingRoom);
        _mapManager.DisplayMap();

        while (true)
        {
            _mapManager.DisplayMap();
            _outputManager.ClearScreen();
            _outputManager.AddLogEntry("(USE NUMPAD OR ARROW KEYS)\n8. North\n6. East\n2. South\n4. West");
            var keyDown = Console.ReadKey().Key.ToString();
            switch (keyDown)
            {
                case "UpArrow":
                    if (selectedRoom.North != null)
                    {
                        return selectedRoom.North;
                    }
                    break;
                case "RightArrow":
                    if (selectedRoom.East != null)
                    {
                        return selectedRoom.East;
                    }
                    break;
                case "DownArrow":
                    if (selectedRoom.South != null)
                    {
                        return selectedRoom.South;
                    }
                    break;
                case "LeftArrow":
                    if (selectedRoom.West != null)
                    {
                        return selectedRoom.West;
                    }
                    break;
                default:
                    _outputManager.AddLogEntry("Retry Please.");
                    Thread.Sleep(250);
                    break;
            }
        }
    }
    //private void LoadMonsters()
    //{
    //    _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();
    //}

}
