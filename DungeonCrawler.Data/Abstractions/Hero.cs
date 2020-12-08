﻿using System;

namespace DungeonCrawler.Data.Abstractions
{
    public abstract class Hero : Character
    {
        public string Name { get; set; } = "NoName Hero";
        public int Experience { get; set; } = 0;
        public int Level { get; set; } = 1;

        public void HealUp(int healingPoints)
        {
            healingPoints = (healingPoints <= HealthPoints["max"]) ?
                healingPoints :
                HealthPoints["max"];

            HealthPoints["current"] += healingPoints;
        }
    }
}