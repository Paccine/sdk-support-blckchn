﻿using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;

// Structure of a Atavism Level Xp Requirements
/*
/* Table structure for tables
/*
 CREATE TABLE `stat` (
  `name` varchar(45) NOT NULL,
  `type` int(11) DEFAULT '0',
  `stat_function` varchar(45) DEFAULT NULL,
  
  Stat table:

name - String - The name of the stat.
type - Integer - Use a drop down with the following: 
(0: Base stat - this is for like strength, agility etc; and 1: Resistance stat - for armour etc)
stat_function - String - What function the stat serves, only used for base stats. 
It only wants the following options: 
health_mod, mana_mod (these two effect the character's health/mana). physical_power, magical_power (these two effect the damage done by the characters abilities). physical_accuracy, magical_accuracy (these two effect the chance of hitting with an ability). See my attached sql file to see examples.  
*/
namespace Atavism
{
    public class TaskData : DataStructure
    {
     
        public TaskData()
        {
            // Database fields
            fields = new Dictionary<string, string>() {
        {"name", "string"},
    };
        }

        public TaskData Clone()
        {
            return (TaskData)this.MemberwiseClone();
        }

        public override string GetValue(string fieldKey)
        {
            switch (fieldKey)
            {
                case "id":
                    return id.ToString();
                case "name":
                    return Name;
            }
            return "";
        }
    }
}