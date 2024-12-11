using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;


namespace ConsoleRpgEntities.Services
{
    public class AbilityService
    {
        private readonly IOutputService _outputService;

        public AbilityService(IOutputService outputService)
        {
            _outputService = outputService;
        }

        public (String Status, int Amount, int Duration) Activate(IAbility ability, IPlayer user, ITargetable target)
        {
            Player player = (Player)user;
            if (ability is AttackAbility attackAbility)
            {
                // Shove ability logic
                target.Health -= attackAbility.Damage;
                _outputService.WriteLine($"{user.Name} attacks {target.Name}, dealing {attackAbility.Damage} damage!");
                return ("NA", 0, 0);
            }
            if (ability is DefenseAbility defenseAbility)
            {
                _outputService.WriteLine($"{user.Name} uses {defenseAbility.Name}, boosting defense by {defenseAbility.DefenseBonus}!");
                return ("Defense", defenseAbility.DefenseBonus, 1);
            }
            if (ability is MagicAbility magicAbility && player.Mana>=magicAbility.ManaCost)
            {
                var abilityStatement = $"{user.Name} uses {magicAbility.Name}";
                if (magicAbility.Damage > 0)
                {
                    target.Health -= magicAbility.Damage;
                    abilityStatement += $", dealing {magicAbility.Damage} damage to {target.Name}";
                }
                if (magicAbility.Defense > 0)
                {
                    abilityStatement += $", boosting defense by {magicAbility.Defense}";
                }
                if (magicAbility.HealsFor > 0)
                {
                    player.Health += magicAbility.HealsFor;
                    if (player.Health > player.MaxHealth)
                    {
                        player.Health = player.MaxHealth;
                    }
                    abilityStatement += $", healing {user.Name} by {magicAbility.HealsFor}";
                }
                if (magicAbility.ManaCost > 0)
                {
                    player.Mana-=magicAbility.ManaCost;
                    abilityStatement += $", using {magicAbility.ManaCost} mana.";
                }
                _outputService.WriteLine(abilityStatement+"\n");
                return ("Defense", magicAbility.Defense, 1);
            }
            return ("NA", 0, 0);
        }
    }
}
