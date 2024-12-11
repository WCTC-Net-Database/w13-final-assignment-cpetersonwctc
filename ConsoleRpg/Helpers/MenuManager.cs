using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Equipments;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpg.Helpers;

public class MenuManager
{
    private readonly OutputManager _outputManager;
    private readonly GameContext _context;
    private readonly DevMenu _devMenu;
    public MenuManager(OutputManager outputManager, GameContext context, DevMenu devMenu)
    {
        _outputManager = outputManager;
        _context = context;
        _devMenu = devMenu;
    }
    public bool ShowMainMenu()
    {
        _outputManager.ClearScreen();
        _outputManager.AddLogEntry("Welcome to the RPG Game!");
        _outputManager.AddLogEntry("1. Start Game");
        _outputManager.AddLogEntry("2. Dev Tools");
        _outputManager.AddLogEntry("3. Exit");

        return HandleMainMenuInput();
    }

    private bool HandleMainMenuInput()
    {
        while (true)
        {
            var input = _outputManager.GetUserInput("Selection:");
            switch (input)
            {
                case "1":
                    _outputManager.AddLogEntry("Starting game...");
                    return true;
                case "2":
                    HandleDevToolInput();
                    return ShowMainMenu();
                case "3":
                    _outputManager.AddLogEntry("Exiting game...");
                    Environment.Exit(0);
                    return false;
                default:
                    _outputManager.AddLogEntry("Invalid selection. Please choose 1, 2, or 3.");
                    break;
            }
        }
    }

    private void HandleDevToolInput()
    {
        while (true)
        {
            _outputManager.ClearScreen();

            //foreach (Player player in _context.Players) { 
            //    _outputManager.AddLogEntry($"{player.Name}");
            //}

            _outputManager.AddLogEntry("You are now editing game data!");
            _outputManager.AddLogEntry("1. Add Player");
            _outputManager.AddLogEntry("2. Edit Player");
            _outputManager.AddLogEntry("3. Display All Players");
            _outputManager.AddLogEntry("4. Search For Player");
            _outputManager.AddLogEntry("5. Create An Ability");
            _outputManager.AddLogEntry("6. Create A Room");
            _outputManager.AddLogEntry("7. View Room Data");
            _outputManager.AddLogEntry("8. Search For Player In Room");
            _outputManager.AddLogEntry("9. Group Players By Room");
            _outputManager.AddLogEntry("10. Find Player By Item");
            _outputManager.AddLogEntry("11. Quit");

            var input = _outputManager.GetUserInput("Selection:");
            switch (input)
            {
                case "1":
                    _outputManager.AddLogEntry("Adding Player...");
                    _devMenu.AddPlayer();
                    //_outputManager.AddLogEntry($"{_context.Players.FirstOrDefault().Name}");
                    break;
                case "2":
                    _devMenu.EditPlayer();
                    break;
                case "3":
                    _outputManager.AddLogEntry("Displaying all characters: ");
                    _devMenu.DisplayAllCharacters();
                    break;
                case "4":
                    _outputManager.AddLogEntry("Search for a character: ");
                    _devMenu.SearchForCharacter();
                    break;
                case "5":
                    _devMenu.CreateNewAbility();
                    break;
                case "6":
                    _devMenu.CreateNewRoom();
                    break;
                case "7":
                    _devMenu.SearchForRoom();
                    break;
                case "8":
                    _devMenu.SearchForCharacterInRoom();
                    break;
                case "9":
                    _devMenu.GroupPlayersByRoom();
                    break;
                case "10":
                    _devMenu.GetPlayerFromItem();
                    break;
                default:
                    _outputManager.AddLogEntry("Invalid selection. Please choose review your input.");
                    break;
            }
            if (input == "11")
            {
                break;
            }
        }
    }
}
