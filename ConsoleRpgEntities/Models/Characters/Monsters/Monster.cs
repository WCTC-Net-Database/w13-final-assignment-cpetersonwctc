﻿using ConsoleRpgEntities.Models.Attributes;

namespace ConsoleRpgEntities.Models.Characters.Monsters
{
    public abstract class Monster : IMonster, ITargetable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomId {  get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int AggressionLevel { get; set; }
        public int BaseDamage {  get; set; }
        public string MonsterType { get; set; }
    }
}
