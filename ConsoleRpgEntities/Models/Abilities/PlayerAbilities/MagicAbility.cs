namespace ConsoleRpgEntities.Models.Abilities.PlayerAbilities
{
    public class MagicAbility : Ability
    {
        public int Damage { get; set; }
        public int Defense { get; set; }
        public int HealsFor {  get; set; }
        public int ManaCost { get; set; }
    }
}
