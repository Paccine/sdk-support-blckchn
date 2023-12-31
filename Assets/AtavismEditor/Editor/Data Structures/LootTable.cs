﻿using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;

// Structure of a Loot Table
/*
/* Table structure for tables
/*
CREATE TABLE `loot_tables` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `item1` int(11) NOT NULL DEFAULT '-1',
  `item1count` int(11) DEFAULT NULL,
  `item1chance` int(11) DEFAULT NULL,
  `item2` int(11) NOT NULL DEFAULT '-1',
  `item2count` int(11) DEFAULT NULL,
  `item2chance` int(11) DEFAULT NULL,
  `item3` int(11) NOT NULL DEFAULT '-1',
  `item3count` int(11) DEFAULT NULL,
  `item3chance` int(11) DEFAULT NULL,
  `item4` int(11) NOT NULL DEFAULT '-1',
  `item4count` int(11) DEFAULT NULL,
  `item4chance` int(11) DEFAULT NULL,
  `item5` int(11) NOT NULL DEFAULT '-1',
  `item5count` int(11) DEFAULT NULL,
  `item5chance` int(11) DEFAULT NULL,
  `item6` int(11) NOT NULL DEFAULT '-1',
  `item6count` int(11) DEFAULT NULL,
  `item6chance` int(11) DEFAULT NULL,
  `item7` int(11) NOT NULL DEFAULT '-1',
  `item7count` int(11) DEFAULT NULL,
  `item7chance` int(11) DEFAULT NULL,
  `item8` int(11) NOT NULL DEFAULT '-1',
  `item8count` int(11) DEFAULT NULL,
  `item8chance` int(11) DEFAULT NULL,
  `item9` int(11) NOT NULL DEFAULT '-1',
  `item9count` int(11) DEFAULT NULL,
  `item9chance` int(11) DEFAULT NULL,
  `item10` int(11) NOT NULL DEFAULT '-1',
  `item10count` int(11) DEFAULT NULL,
  `item10chance` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`),
  KEY `item1` (`item1`)
)
*/
namespace Atavism
{
    public class LootTableEntry 
    {
        public LootTableEntry() : this( -1, 1f, 1)
        {
        }
        public LootTableEntry(int itemId, float chance, int count)
        {
            this.itemId = itemId;
            this.chance = chance;
            this.count = count;
        }
        public LootTableEntry(int id,int itemId, float chance, int count)
        {
            this.id = id;
            this.itemId = itemId;
            this.chance = chance;
            this.count = count;
        }
        public int id = -1;
        public int itemId = -1;
        public float chance=0f;
        public int count = 1;
        public int countMax = 1;
        public string itemSearch = "";
    }

    public class LootTable : DataStructure
    {
                                            // General Parameters
        public int maxEntries = 10; // Change this later to 10 when we can fit it all on the screen
        public List<LootTableEntry> entries = new List<LootTableEntry>();
        public List<int> entriesToBeDeleted = new List<int>();

        public LootTable()
        {
            // Database fields
            fields = new Dictionary<string, string>() {
        {"name", "string"},
     /*   {"item1", "int"},
        {"item1chance", "float"},
        {"item1count", "int"},
        {"item2", "int"},
        {"item2chance", "float"},
        {"item2count", "int"},
        {"item3", "int"},
        {"item3chance", "float"},
        {"item3count", "int"},
        {"item4", "int"},
        {"item4chance", "float"},
        {"item4count", "int"},
        {"item5", "int"},
        {"item5chance", "float"},
        {"item5count", "int"},
        {"item6", "int"},
        {"item6chance", "float"},
        {"item6count", "int"},
        {"item7", "int"},
        {"item7chance", "float"},
        {"item7count", "int"},
        {"item8", "int"},
        {"item8chance", "float"},
        {"item8count", "int"},
        {"item9", "int"},
        {"item9chance", "float"},
        {"item9count", "int"},
        {"item10", "int"},
        {"item10chance", "float"},
        {"item10count", "int"},*/
    };
        }

        public LootTable Clone()
        {
            return (LootTable)this.MemberwiseClone();
        }

        public override string GetValue(string fieldKey)
        {
            switch (fieldKey)
            {
                case "name":
                    return Name;
           /*     case "item1":
                    if (entries.Count > 0)
                        return entries[0].itemId.ToString();
                    else
                        return "-1";
                case "item1chance":
                    if (entries.Count > 0)
                        return entries[0].chance.ToString();
                    else
                        return "0";
                case "item1count":
                    if (entries.Count > 0)
                        return entries[0].count.ToString();
                    else
                        return "0";
                case "item2":
                    if (entries.Count > 1)
                        return entries[1].itemId.ToString();
                    else
                        return "-1";
                case "item2chance":
                    if (entries.Count > 1)
                        return entries[1].chance.ToString();
                    else
                        return "0";
                case "item2count":
                    if (entries.Count > 1)
                        return entries[1].count.ToString();
                    else
                        return "0";
                case "item3":
                    if (entries.Count > 2)
                        return entries[2].itemId.ToString();
                    else
                        return "-1";
                case "item3chance":
                    if (entries.Count > 2)
                        return entries[2].chance.ToString();
                    else
                        return "0";
                case "item3count":
                    if (entries.Count > 2)
                        return entries[2].count.ToString();
                    else
                        return "0";
                case "item4":
                    if (entries.Count > 3)
                        return entries[3].itemId.ToString();
                    else
                        return "-1";
                case "item4chance":
                    if (entries.Count > 3)
                        return entries[3].chance.ToString();
                    else
                        return "0";
                case "item4count":
                    if (entries.Count > 3)
                        return entries[3].count.ToString();
                    else
                        return "0";
                case "item5":
                    if (entries.Count > 4)
                        return entries[4].itemId.ToString();
                    else
                        return "-1";
                case "item5chance":
                    if (entries.Count > 4)
                        return entries[4].chance.ToString();
                    else
                        return "0";
                case "item5count":
                    if (entries.Count > 4)
                        return entries[4].count.ToString();
                    else
                        return "0";
                case "item6":
                    if (entries.Count > 5)
                        return entries[5].itemId.ToString();
                    else
                        return "-1";
                case "item6chance":
                    if (entries.Count > 5)
                        return entries[5].chance.ToString();
                    else
                        return "0";
                case "item6count":
                    if (entries.Count > 5)
                        return entries[5].count.ToString();
                    else
                        return "0";
                case "item7":
                    if (entries.Count > 6)
                        return entries[6].itemId.ToString();
                    else
                        return "-1";
                case "item7chance":
                    if (entries.Count > 6)
                        return entries[6].chance.ToString();
                    else
                        return "0";
                case "item7count":
                    if (entries.Count > 6)
                        return entries[6].count.ToString();
                    else
                        return "0";
                case "item8":
                    if (entries.Count > 7)
                        return entries[7].itemId.ToString();
                    else
                        return "-1";
                case "item8chance":
                    if (entries.Count > 7)
                        return entries[7].chance.ToString();
                    else
                        return "0";
                case "item8count":
                    if (entries.Count > 7)
                        return entries[7].count.ToString();
                    else
                        return "0";
                case "item9":
                    if (entries.Count > 8)
                        return entries[8].itemId.ToString();
                    else
                        return "-1";
                case "item9chance":
                    if (entries.Count > 8)
                        return entries[8].chance.ToString();
                    else
                        return "0";
                case "item9count":
                    if (entries.Count > 8)
                        return entries[8].count.ToString();
                    else
                        return "0";
                case "item10":
                    if (entries.Count > 9)
                        return entries[9].itemId.ToString();
                    else
                        return "-1";
                case "item10chance":
                    if (entries.Count > 9)
                        return entries[9].chance.ToString();
                    else
                        return "0";
                case "item10count":
                    if (entries.Count > 9)
                        return entries[9].count.ToString();
                    else
                        return "0";*/
            }
            return "";
        }

    }
}