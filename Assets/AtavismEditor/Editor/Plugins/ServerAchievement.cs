﻿using UnityEngine;
using UnityEditor;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
namespace Atavism
{
    // Handles the Item Configuration
    public class ServerAchievement : AtavismDatabaseFunction
    {

        enum VipView
        {
            Home,
            Level,
            Settings
        }

        public new Dictionary<int, AchievementData> dataRegister;
        public new AchievementData editingDisplay;
        public new AchievementData originalDisplay;
        public List<BonusOptionData> optionslist = new List<BonusOptionData>();

        public int[] bonusIds = new int[] { -1 };
        public string[] bonusOptions = new string[] { "~ none ~" };
        public string[] bonusOptionsParam = new string[] { "" };
        public string[] bonusOptionsCode = new string[] { "" };

        public string[] damageOptions = new string[] { "~ none ~" };

        public string[] itemEffectTypeOptions = new string[] { "~ none ~" };
        public string[] vipOptions = new string[] { "~ none ~" };
        public string[] accountEffectTypeOptions = new string[] { "~ none ~" };
        VipView view = VipView.Level;
        public string[] itemQualityOptions = new string[] { "~ none ~" };
        string searchString = "";

        public int[] mobIds = new int[] { -1 };
        public string[] mobList = new string[] { "~ none ~" };

        public int[] itemIds = new int[] { -1 };
        public string[] itemsList = new string[] { "~ none ~" };

        public int[] abilityIds = new int[] { -1 };
        public string[] abilityList = new string[] { "~ none ~" };
        public int[] skillsIds = new int[] { -1 };
        public string[] skillsList = new string[] { "~ none ~" };
        // Handles the prefab creation, editing and save
        // private ItemPrefab item = null;
        public string[] statOptions = new string[] { "~ none ~" };

        // Filter/Search inputs
        private string currencySearchInput = "";
        private List<string> effectSearchInput = new List<string>();
        private List<string> statsSearchInput = new List<string>();

        // Use this for initialization
        public ServerAchievement()
        {
            functionName = "Achievements";
            // Database tables name
            tableName = "achievement_settings";
            functionTitle = "Achievement Configuration";
            loadButtonLabel = "Load Achievements";
            notLoadedText = "No Achievements loaded.";
            // Init
            dataRegister = new Dictionary<int, AchievementData>();

            editingDisplay = new AchievementData();
            originalDisplay = new AchievementData();
            showRecovery = true;
            searchString = "";
        }

        void resetSearch(bool fokus)
        {
            currencySearchInput = "";
            effectSearchInput.Clear();
           if(fokus) GUI.FocusControl(null);
        }

        public override void Activate()
        {
            linkedTablesLoaded = false;
            resetSearch(true);
        }
      

        private void LoadItemList()
        {
            string query = "SELECT id, name FROM item_templates where isactive = 1";

            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            // Read data
            int optionsId = 0;
            if ((rows != null) && (rows.Count > 0))
            {
                itemsList = new string[rows.Count + 1];
                itemsList[optionsId] = "~ none ~";
                itemIds = new int[rows.Count + 1];
                itemIds[optionsId] = -1;
                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    itemsList[optionsId] = data["id"] + ":" + data["name"];
                    itemIds[optionsId] = int.Parse(data["id"]);
                }
            }
        }

        public void LoadAbilityOptions()
        {
            string query = "SELECT id, name FROM abilities where isactive = 1";

            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            // Read data
            int optionsId = 0;
            if ((rows != null) && (rows.Count > 0))
            {
                abilityList = new string[rows.Count + 1];
                abilityList[optionsId] = "~ none ~";
                abilityIds = new int[rows.Count + 1];
                abilityIds[optionsId] = -1;
                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    abilityList[optionsId] = data["id"] + ":" + data["name"];
                    abilityIds[optionsId] = int.Parse(data["id"]);
                }
            }
        }

        public void LoadSkillsOptions()
        {
            string query = "SELECT id, name FROM skills where isactive = 1";

            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            // Read data
            int optionsId = 0;
            if ((rows != null) && (rows.Count > 0))
            {
                skillsList = new string[rows.Count + 1];
                skillsList[optionsId] = "~ none ~";
                skillsIds = new int[rows.Count + 1];
                skillsIds[optionsId] = -1;
                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    skillsList[optionsId] = data["id"] + ":" + data["name"];
                    skillsIds[optionsId] = int.Parse(data["id"]);
                }
            }
        }

        public void LoadBonusesOptions()
        {
                // Read all entries from the table
                string query = "SELECT name FROM bonuses_settings where isactive = 1";
                // If there is a row, clear it.
                if (rows != null)
                    rows.Clear();
                // Load data
                rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
                //Debug.Log("#Rows:"+rows.Count);
                // Read all the data
                int optionsId = 0;
                if ((rows != null) && (rows.Count > 0))
                {
                    vipOptions = new string[rows.Count + 1];
                vipOptions[optionsId] = "~ none ~";
                    foreach (Dictionary<string, string> data in rows)
                    {
                        optionsId++;
                    vipOptions[optionsId] = data["name"];
                    }
                }
        }
      

        public void LoadStatOptions()
        {

            // Read all entries from the table
            string query = "SELECT name FROM stat where isactive = 1";

            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            //Debug.Log("#Rows:"+rows.Count);
            // Read all the data
            int optionsId = 0;
            if ((rows != null) && (rows.Count > 0))
            {
                statOptions = new string[rows.Count + 1];
                statOptions[optionsId] = "~ none ~";
                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    statOptions[optionsId] = data["name"];
                }
            }

        }



        // Load Database Data
        public override void Load()
        {
            if (!dataLoaded)
            {
                // Clean old data
                // dataRegister.Clear();
                displayKeys.Clear();

                // Read all entries from the table
                string query = "SELECT * FROM " + tableName + " where isactive = 1 ";

                // If there is a row, clear it.
                if (rows != null)
                    rows.Clear();

                // Load data
                rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
                //Debug.Log("#Rows:"+rows.Count);
                // Read all the data
                displayList.Clear();
                if ((rows != null) && (rows.Count > 0))
                {
                    foreach (Dictionary<string, string> data in rows)
                    {
                        displayList.Add(data["id"] + ". " + data["name"]);
                        displayKeys.Add(int.Parse(data["id"]));

                    }
                }
                dataLoaded = true;
            }
        }

        AchievementData LoadEntity(int id)
        {
            // Read all entries from the table
            string query = "SELECT * FROM " + tableName + " where isactive = 1 AND id=" + id;
            //  Debug.LogError(query);
            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            //Debug.Log("#Rows:"+rows.Count);
            // Read all the data
            AchievementData display = new AchievementData();
            Char delimiter = ';';
            if ((rows != null) && (rows.Count > 0))
            {
                foreach (Dictionary<string, string> data in rows)
                {   //foreach(string key in data.Keys)
                    //	Debug.Log("Name[" + key + "]:" + data[key]);
                    //return;
                    display.id = int.Parse(data["id"]);
                    display.Name = data["name"];
                    display.value = int.Parse(data["value"]);
                    display.description = data["description"];
                 //   display.objects = data["objects"];
                    if (data["objects"].Length > 0)
                    {
                        string[] splitted = data["objects"].Split(delimiter);
                        for (int i = 0; i < splitted.Length; i++)
                        {
                            display.objects.Add(int.Parse(splitted[i]));
                            display.objectsSearch.Add("");
                        }
                    }
                    display.type = int.Parse(data["type"]);
                    display.isLoaded = true;
                    //   ad.enchantLevels.Add(display);
                }
            }
            query = "SELECT * FROM achievement_bonuses where achievement_id=" + id;

            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            if ((rows != null) && (rows.Count > 0))
            {
                foreach (Dictionary<string, string> data in rows)
                {   //foreach(string key in data.Keys)
                    //	Debug.Log("Name[" + key + "]:" + data[key]);

                    display.bonuses.Add(new BonusEntry(int.Parse(data["achievement_id"]), int.Parse(data["bonus_settings_id"]), int.Parse(data["value"]), float.Parse(data["valuep"])));
                }
            }

            query = "SELECT * FROM achievement_stats where achievement_id=" + id;

            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            if ((rows != null) && (rows.Count > 0))
            {
                foreach (Dictionary<string, string> data in rows)
                {   //foreach(string key in data.Keys)
                    //	Debug.Log("Name[" + key + "]:" + data[key]);

                    display.stats.Add(new StatEntry( data["stat"], int.Parse(data["value"]), float.Parse(data["valuep"])));
                }
            }

            return display;
        }

        public override void LoadRestore()
        {
            if (!dataRestoreLoaded)
            {
                // Clean old data
                dataRegister.Clear();
                displayKeys.Clear();

                // Read all entries from the table
                string query = "SELECT distinct id,name FROM " + tableName + " where isactive = 0";

                // If there is a row, clear it.
                if (rows != null)
                    rows.Clear();

                // Load data
                rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
                //Debug.Log("#Rows:"+rows.Count);
                // Read all the data
                displayList.Clear();
                if ((rows != null) && (rows.Count > 0))
                {
                    foreach (Dictionary<string, string> data in rows)
                    {
                        displayList.Add(data["id"] + ". " + data["name"]);
                        displayKeys.Add(int.Parse(data["id"]));

                    }
                }
                dataRestoreLoaded = true;
            }
        }

        private void LoadMobList()
        {
            string query = "SELECT id, name FROM mob_templates where isactive = 1";

            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();

            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            // Read data
            int optionsId = 0;
            if ((rows != null) && (rows.Count > 0))
            {
                mobList = new string[rows.Count + 1];
                mobList[optionsId] = "~ none ~";
                mobIds = new int[rows.Count + 1];
                mobIds[optionsId] = -1;
                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    mobList[optionsId] = data["id"] + ":" + data["name"];
                    mobIds[optionsId] = int.Parse(data["id"]);
                }
            }
        }
     /*   public void LoadBonusSettings()
        {
            // Clean old data
            // Read all entries from the table
            string query = "SELECT * FROM bonuses_settings where isactive = 1 order by id ";
            // Debug.LogError(query);
            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();
            optionslist.Clear();
            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            //Debug.Log("#Rows:"+rows.Count);
            // Read all the data

            if ((rows != null) && (rows.Count > 0))
            {
                int optionsId = 0;
                bonusOptions = new string[rows.Count + 1];
                bonusOptions[optionsId] = "~ none ~"; 
                bonusIds = new int[rows.Count + 1];
                bonusIds[optionsId] = -1;
              
                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    bonusOptions[optionsId] = data["name"];
                    bonusIds[optionsId] = int.Parse(data["id"]);
                    //    Debug.LogError(int.Parse(data["id"])+", "+data["name"]+", "+float.Parse(data["cost"])+", "+float.Parse(data["chance"]));
                    optionslist.Add(new BonusOptionData(int.Parse(data["id"]), data["name"], data["code"], data["param"]));
                }
            }
            //     dataLoaded = true;

            //  }
        }*/

        public void LoadBonusSettings()
        {
            // Read all entries from the table
            string query = "SELECT * FROM bonuses_settings where isactive = 1 order by id ";
            //  Debug.LogError(query);
            // If there is a row, clear it.
            if (rows != null)
                rows.Clear();
            //     optionslist.Clear();
            // Load data
            rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            //Debug.Log("#Rows:"+rows.Count);
            // Read all the data

            if ((rows != null) && (rows.Count > 0))
            {
                int optionsId = 0;
                bonusOptions = new string[rows.Count + 1];
                bonusOptionsParam = new string[rows.Count + 1];
                bonusOptionsCode = new string[rows.Count + 1];
                bonusOptions[optionsId] = "~ none ~";
                bonusOptionsParam[optionsId] = "";
                bonusOptionsCode[optionsId] = "";
                bonusIds = new int[rows.Count + 1];
                bonusIds[optionsId] = -1;

                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    bonusOptions[optionsId] = data["name"];
                    bonusIds[optionsId] = int.Parse(data["id"]);
                    bonusOptionsParam[optionsId] = data["param"];
                    bonusOptionsCode[optionsId] = data["code"];
                    //    Debug.LogError(int.Parse(data["id"])+", "+data["name"]+", "+float.Parse(data["cost"])+", "+float.Parse(data["chance"]));
                    //     optionslist.Add(new BonusOptionData(int.Parse(data["id"]), data["name"], data["code"], data["param"]));
                }
            }
        }



        public void LoadBonusSettings2()
        {
         /*   if (!dataLoaded)
            {*/
                // Clean old data
                dataRegister.Clear();
                displayKeys.Clear();
               // EnchantLevelData eld = new EnchantLevelData();
                // Read all entries from the table
                string query = "SELECT * FROM achievement_settings where isactive = 1 order by id ";
                //  Debug.LogError(query);
                // If there is a row, clear it.
                if (rows != null)
                    rows.Clear();
                optionslist .Clear();
                // Load data
                rows = DatabasePack.LoadData(DatabasePack.contentDatabasePrefix, query);
            //Debug.Log("#Rows:"+rows.Count);
            // Read all the data

            if ((rows != null) && (rows.Count > 0))
            {
                int optionsId = 0;
                bonusOptions = new string[rows.Count + 1];
                bonusOptions[optionsId] = "~ none ~";
                bonusIds = new int[rows.Count + 1];
                bonusIds[optionsId] = -1;
                foreach (Dictionary<string, string> data in rows)
                {

                }
                foreach (Dictionary<string, string> data in rows)
                {
                    optionsId++;
                    bonusOptions[optionsId] = data["name"];
                    bonusIds[optionsId] = int.Parse(data["id"]);
                    //    Debug.LogError(int.Parse(data["id"])+", "+data["name"]+", "+float.Parse(data["cost"])+", "+float.Parse(data["chance"]));
                    optionslist.Add(new BonusOptionData(int.Parse(data["id"]), data["name"], data["code"], data["params"]));
                }
            }
           //     dataLoaded = true;

          //  }
        }

        public override void Draw(Rect box)
        {

            // Setup the layout
            Rect pos = box;
                base.Draw(pos);

        }
        
        // Draw the loaded list
        public override void DrawLoaded(Rect box)
        {
            // Setup the layout
            Rect pos = box;
            pos.x += ImagePack.innerMargin;
            pos.y += ImagePack.innerMargin;
            pos.width -= ImagePack.innerMargin;
            pos.height = ImagePack.fieldHeight;
            pos.width /= 2;
            pos.x += pos.width;
            pos.x -= pos.width;
            pos.width *= 2;

            if (view.Equals(VipView.Level))
            {
                if (displayList.Count <= 0)
                {
                    pos.y += ImagePack.fieldHeight;
                    ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("You must create an Achievement before edit it."));
                    return;
                }
                // Draw the content database info
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Edit Achievement"));
                

                if (newItemCreated)
                {
                    newItemCreated = false;
                    newSelectedDisplay = displayKeys.Count - 1;
                }
                // Draw data Editor
                if (newSelectedDisplay != selectedDisplay)
                {
                    selectedDisplay = newSelectedDisplay;
                    int displayKey = -1;
                    if (selectedDisplay >= 0)
                        displayKey = displayKeys[selectedDisplay];
                    editingDisplay = LoadEntity(displayKey);
                    resetSearch(false);
                }
                pos.y += ImagePack.fieldHeight * 1.5f;
                pos.x -= ImagePack.innerMargin;
                pos.y -= ImagePack.innerMargin;
                pos.width += ImagePack.innerMargin;
            }

            if (state != State.Loaded && view.Equals(VipView.Level))
            {
                pos.x += ImagePack.innerMargin;
                pos.width /= 2;
                //Draw super magical compound object.
                newSelectedDisplay = ImagePack.DrawDynamicPartialListSelector(pos, Lang.GetTranslate("Search Filter")+": ", ref entryFilterInput, selectedDisplay, displayList);

                pos.y += ImagePack.fieldHeight;
                //Build prefabs button
                pos.x += pos.width;
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Duplicate")))
                {
                    Duplicate();
                }
                pos.x -= pos.width;

                pos.width *= 2;
                pos.y += ImagePack.fieldHeight * 1.5f;
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Achievement Properties") +":");
                pos.y += ImagePack.fieldHeight * 0.75f;
            }

            DrawEditor(pos, false);

            pos.y -= ImagePack.fieldHeight;
            pos.y += ImagePack.innerMargin;
            pos.width -= ImagePack.innerMargin;
        }

        public override void DrawRestore(Rect box)
        {
            // Setup the layout
            Rect pos = box;
            pos.x += ImagePack.innerMargin;
            pos.y += ImagePack.innerMargin;
            pos.width -= ImagePack.innerMargin;
            pos.height = ImagePack.fieldHeight;

            if (displayList.Count <= 0)
            {
                pos.y += ImagePack.fieldHeight;
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("You dont have deleted")+" " + Lang.GetTranslate(functionName) + ".");
                return;
            }

            // Draw the content database info
            ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Restore")+" " + Lang.GetTranslate(functionName) + " "+ Lang.GetTranslate("Configuration"));
            pos.y += ImagePack.fieldHeight;

            pos.width -= 140 + 155;
            for (int i = 0; i < displayList.Count; i++)
            {
                ImagePack.DrawText(pos, displayList[i]);
                pos.x += pos.width;
                if (ImagePack.DrawButton(pos, Lang.GetTranslate("Delete Permanently")))
                {
                    DeleteForever(displayKeys[i]);
                }
                pos.x += 155;
                if (ImagePack.DrawButton(pos, Lang.GetTranslate("Restore")))
                {
                    RestoreEntry(displayKeys[i]);
                }

                pos.x -= pos.width + 155;
                pos.y += ImagePack.fieldHeight;
            }
            pos.width += 140+ 155;
            showCancel = false;
            showDelete = false;
            showSave = false;
            if (resultTimeout != -1 && resultTimeout < Time.realtimeSinceStartup && result != "")
            {
                result = "";
            }
            EnableScrollBar(pos.y - box.y + ImagePack.fieldHeight);
        }

        public override void CreateNewData()
        {
        
                editingDisplay = new AchievementData();
                originalDisplay = new AchievementData();
          
            selectedDisplay = -1;
        }

        // Edit or Create
        public override void DrawEditor(Rect box, bool newItem)
        {
            newEntity = newItem;
            // Setup the layout
            Rect pos = box;
            pos.y += ImagePack.innerMargin;
            pos.width -= ImagePack.innerMargin;
            pos.height = ImagePack.fieldHeight;
            if (state.Equals(State.New))
            {
                pos.width /= 2;
                pos.x += pos.width;

                pos.x -= pos.width;
                pos.width *= 2;
                pos.y += 2f * ImagePack.innerMargin;
            }

            if (!linkedTablesLoaded)
            {
                LoadBonusSettings();
                LoadMobList();
                LoadItemList();
                LoadAbilityOptions();
                LoadSkillsOptions();
                LoadStatOptions();
                LoadBonusesOptions();
                linkedTablesLoaded = true;
            }

            // Draw the content database info
            if (newEntity)
            {
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Create a new Achievement"));
                pos.y += ImagePack.fieldHeight;
            }

            editingDisplay.Name = ImagePack.DrawField(pos, Lang.GetTranslate("Name") + ":", editingDisplay.Name, 0.8f);
            pos.y += ImagePack.fieldHeight;
            pos.width /= 2;
            editingDisplay.type = ImagePack.DrawDynamicFilteredListSelector(pos, Lang.GetTranslate("Type") + ": ", ref searchString, editingDisplay.type, editingDisplay.types);
            pos.y += ImagePack.fieldHeight;
            editingDisplay.value = ImagePack.DrawField(pos, Lang.GetTranslate("Value") + ":", editingDisplay.value);
            pos.width *= 2;
            pos.y += ImagePack.fieldHeight;
            editingDisplay.description = ImagePack.DrawField(pos, Lang.GetTranslate("Description") + ":", editingDisplay.description, 0.75f, 60);
            pos.y += ImagePack.fieldHeight * 3;
            pos.width /= 2;

         
            if (editingDisplay.type == 1)
            {
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Objective Mob"));
             


                for (int j = 0; j < editingDisplay.objects.Count; j++)
                {
                    pos.y += ImagePack.fieldHeight;
                    int selectedMob = GetOptionPosition(editingDisplay.objects[j], mobIds);
                    string search = editingDisplay.objectsSearch[j];
                    selectedMob = ImagePack.DrawDynamicFilteredListSelector(pos, Lang.GetTranslate("Target Mob") + (j + 1) + ":", ref search, selectedMob, mobList);
                    editingDisplay.objectsSearch[j] = search;
                    editingDisplay.objects[j] = mobIds[selectedMob];
                    pos.x += pos.width;
                    pos.y += ImagePack.fieldHeight;
                    if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Delete Mob")))
                    {

                        editingDisplay.objects.RemoveAt(j);
                        editingDisplay.objectsSearch.RemoveAt(j);
                    }
                    pos.x -= pos.width;
                }
                pos.y += ImagePack.fieldHeight;
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Add Mob")))
                {
                    editingDisplay.objects.Add(0);
                    editingDisplay.objectsSearch.Add("");
                }
            }
            else if ( editingDisplay.type == 4)
            {
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Objective Item"));
           


                for (int j = 0; j < editingDisplay.objects.Count; j++)
                {
                    pos.y += ImagePack.fieldHeight;
                    int selectedMob = GetOptionPosition(editingDisplay.objects[j], itemIds);
                    string search = editingDisplay.objectsSearch[j];
                    selectedMob = ImagePack.DrawDynamicFilteredListSelector(pos, Lang.GetTranslate("Item ") + (j + 1) + ":", ref search, selectedMob, itemsList);
                    editingDisplay.objectsSearch[j] = search;
                    editingDisplay.objects[j] = itemIds[selectedMob];
                    pos.x += pos.width;
                    pos.y += ImagePack.fieldHeight;
                    if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Delete item")))
                    {

                        editingDisplay.objects.RemoveAt(j);
                        editingDisplay.objectsSearch.RemoveAt(j);
                    }
                    pos.x -= pos.width;
                }
                pos.y += ImagePack.fieldHeight;
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Add item")))
                {
                    editingDisplay.objects.Add(0);
                    editingDisplay.objectsSearch.Add("");
                }
            }
            else if (editingDisplay.type == 6)
            {
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Objective Ability"));

            

                for (int j = 0; j < editingDisplay.objects.Count; j++)
                {
                    pos.y += ImagePack.fieldHeight;
                    int selectedability = GetOptionPosition(editingDisplay.objects[j], abilityIds);
                    string search = editingDisplay.objectsSearch[j];
                    selectedability = ImagePack.DrawDynamicFilteredListSelector(pos, Lang.GetTranslate("Ability ") + (j + 1) + ":", ref search, selectedability, abilityList);
                    editingDisplay.objectsSearch[j] = search;
                    editingDisplay.objects[j] = abilityIds[selectedability];
                    pos.x += pos.width;
                    pos.y += ImagePack.fieldHeight;
                    if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Delete Ability")))
                    {

                        editingDisplay.objects.RemoveAt(j);
                        editingDisplay.objectsSearch.RemoveAt(j);
                    }
                    pos.x -= pos.width;
                }
                pos.y += ImagePack.fieldHeight;
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Add Ability")))
                {
                    editingDisplay.objects.Add(0);
                    editingDisplay.objectsSearch.Add("");
                }
            }
            else if (editingDisplay.type == 3)
            {
                ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Objective Skills"));



                for (int j = 0; j < editingDisplay.objects.Count; j++)
                {
                    pos.y += ImagePack.fieldHeight;
                    int selectedskill = GetOptionPosition(editingDisplay.objects[j], skillsIds);
                    string search = editingDisplay.objectsSearch[j];
                    selectedskill = ImagePack.DrawDynamicFilteredListSelector(pos, Lang.GetTranslate("Skill ") + (j + 1) + ":", ref search, selectedskill, skillsList);
                    editingDisplay.objectsSearch[j] = search;
                    editingDisplay.objects[j] = skillsIds[selectedskill];
                    pos.x += pos.width;
                    pos.y += ImagePack.fieldHeight;
                    if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Delete Skill")))
                    {

                        editingDisplay.objects.RemoveAt(j);
                        editingDisplay.objectsSearch.RemoveAt(j);
                    }
                    pos.x -= pos.width;
                }
                pos.y += ImagePack.fieldHeight;
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Add Skill")))
                {
                    editingDisplay.objects.Add(0);
                    editingDisplay.objectsSearch.Add("");
                }
            }
            pos.width *= 2;
            pos.y += ImagePack.fieldHeight;
            ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Bonuses"));
            pos.y += ImagePack.fieldHeight;


            for (int i = 0; i < editingDisplay.bonuses.Count; i++)
            {
                pos.width = pos.width / 2;
                // Generate search string if none exists
                if (effectSearchInput.Count <= i)
                {
                    effectSearchInput.Add("");
                }

               

                string searchString = effectSearchInput[i];
                int selectedBonus = GetOptionPosition(editingDisplay.bonuses[i].BonusType, bonusIds);
                selectedBonus = ImagePack.DrawDynamicFilteredListSelector(pos, Lang.GetTranslate("Option Name") + ": ", ref searchString, selectedBonus, bonusOptions);
                editingDisplay.bonuses[i].BonusType = bonusIds[selectedBonus];
                pos.y += ImagePack.fieldHeight;

                if (bonusOptionsParam[selectedBonus].Contains("v"))
                {

                    editingDisplay.bonuses[i].BonusValue = ImagePack.DrawField(pos, Lang.GetTranslate("Value") + ":", editingDisplay.bonuses[i].BonusValue);
                    pos.x += pos.width;
                }
                if (bonusOptionsParam[selectedBonus].Contains("p"))
                    editingDisplay.bonuses[i].BonusValuePercentage = ImagePack.DrawField(pos, Lang.GetTranslate("Percentage") + ":", editingDisplay.bonuses[i].BonusValuePercentage);
                if (bonusOptionsParam[selectedBonus].Contains("v"))
                    pos.x -= pos.width;




             /*   editingDisplay.bonuses[i].BonusValue = ImagePack.DrawField(pos, Lang.GetTranslate("Value") + ":", editingDisplay.bonuses[i].BonusValue);
                pos.x += pos.width;
                editingDisplay.bonuses[i].BonusValuePercentage = ImagePack.DrawField(pos, Lang.GetTranslate("Value percentage") + ":", editingDisplay.bonuses[i].BonusValuePercentage);
                pos.x -= pos.width;
                */
                // Save back the search string
                effectSearchInput[i] = searchString;

                pos.x += pos.width;
                pos.y += ImagePack.fieldHeight;
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Remove Bonus")))
                {
                    editingDisplay.bonuses.RemoveAt(i);
                }
                pos.x -= pos.width;
                pos.y += ImagePack.fieldHeight;
                pos.width = pos.width * 2;
            }

            if (editingDisplay.bonuses.Count < vipOptions.Length)
            {
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Add Bonus")))
                {
                    BonusEntry statEntry = new BonusEntry(-1, 0, 0, 0);
                    editingDisplay.bonuses.Add(statEntry);
                }
            }

            pos.y += ImagePack.fieldHeight;
            ImagePack.DrawLabel(pos.x, pos.y, Lang.GetTranslate("Bonus Stats"));
            pos.y += ImagePack.fieldHeight;

            for (int i = 0; i < editingDisplay.stats.Count; i++)
            {
                pos.width = pos.width / 2;
                // Generate search string if none exists
                if (statsSearchInput.Count <= i)
                {
                    statsSearchInput.Add("");
                }
                string searchString = statsSearchInput[i];
                editingDisplay.stats[i].itemStatName = ImagePack.DrawDynamicFilteredListSelector(pos, Lang.GetTranslate("Stat Name") + ": ", ref searchString, editingDisplay.stats[i].itemStatName, statOptions);
                pos.y += ImagePack.fieldHeight;
                editingDisplay.stats[i].itemStatValue = ImagePack.DrawField(pos, Lang.GetTranslate("Value") + ":", editingDisplay.stats[i].itemStatValue);
                pos.x += pos.width;
                editingDisplay.stats[i].itemStatValuePercentagef = ImagePack.DrawField(pos, Lang.GetTranslate("Value percentage") + ":", editingDisplay.stats[i].itemStatValuePercentagef);
                pos.x -= pos.width;

                // Save back the search string
                statsSearchInput[i] = searchString;

                pos.x += pos.width;
                pos.y += ImagePack.fieldHeight;
                if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Remove Stat")))
                {
                    editingDisplay.stats.RemoveAt(i);
                }
                pos.x -= pos.width;
                pos.y += ImagePack.fieldHeight;
                pos.width = pos.width * 2;
            }
            if (ImagePack.DrawButton(pos.x, pos.y, Lang.GetTranslate("Add Stat")))
            {
                StatEntry statEntry = new StatEntry("", 0, 0f);
                editingDisplay.stats.Add(statEntry);
            }

            pos.width = pos.width / 2;
            pos.x += pos.width;
            pos.y += ImagePack.fieldHeight;


            // Save data		
            pos.x -= ImagePack.innerMargin;
            pos.y += 3f * ImagePack.fieldHeight;
            if (!newEntity)
            {
                Color temp = GUI.color;
                GUI.color = Color.red;
                //   ImagePack.DrawText(pos, "   "+Lang.GetTranslate("Attention, erasing is permanent !!! "));
                GUI.color = temp;
                pos.y += ImagePack.fieldHeight;
            }
            pos.width /= 3;

            showSave = true;
            if (!newEntity)
            {
                showDelete = true;
            }
            else
            {
                showDelete = false;
            }
            showCancel = true;
            if (resultTimeout != -1 && resultTimeout < Time.realtimeSinceStartup && result != "")
            {
                result = "";
            }

            if (!newEntity)
                EnableScrollBar(pos.y - box.y + ImagePack.fieldHeight + 140);
            else
                EnableScrollBar(pos.y - box.y + ImagePack.fieldHeight);
        }

        public override void save()
        {
        
                if (newEntity)
                    InsertEntry();
                else
                    UpdateEntry();
         
            state = State.Loaded;
            resetSearch(true);
        }

        public override void delete()
        {
            if (EditorUtility.DisplayDialog(Lang.GetTranslate("Delete")+" " + Lang.GetTranslate(functionName) + " " + Lang.GetTranslate("entry")+" ?", Lang.GetTranslate("Are you sure you want to delete")+" " + editingDisplay.Name + " ?", Lang.GetTranslate("Delete"), Lang.GetTranslate("Do Not delete")))
            {
                DeleteEntry();
                newSelectedDisplay = 0;
                state = State.Loaded;
            }
            resetSearch(true);
        }

        public override void cancel()
        {
            resetSearch(true);
            if (selectedDisplay > -1)
                editingDisplay = LoadEntity(displayKeys[selectedDisplay]);
            else
                editingDisplay = LoadEntity(selectedDisplay);
            if (newEntity)
                state = State.New;
            else
                state = State.Loaded;
        }




        // Insert new entries into the table
        void InsertEntry()
        {
            if (rows != null)
                rows.Clear();
            string query = "INSERT INTO " + tableName;
            query += " (name ,type ,description ,value ,objects) ";
            query += "VALUES ";
            query += "(?name ,?type ,?description ,?value, ?objects) ";
            int achivID = -1;
            List<Register> update = new List<Register>();
            update.Add(new Register("name", "?name", MySqlDbType.VarChar, editingDisplay.Name.ToString(), Register.TypesOfField.String));
            update.Add(new Register("type", "?type", MySqlDbType.Int32, editingDisplay.type.ToString(), Register.TypesOfField.Int));
            update.Add(new Register("description", "?description", MySqlDbType.VarChar, editingDisplay.description, Register.TypesOfField.String));
            update.Add(new Register("objects", "?objects", MySqlDbType.VarChar, string.Join(";", editingDisplay.objects.ConvertAll(i => i.ToString()).ToArray()), Register.TypesOfField.String));
            update.Add(new Register("value", "?value", MySqlDbType.Int32, editingDisplay.value.ToString(), Register.TypesOfField.Int));
            achivID = DatabasePack.Insert(DatabasePack.contentDatabasePrefix, query, update);
            if (achivID != -1)
            {
                // Update online table to avoid access the database again			
                editingDisplay.id = achivID;
              
                editingDisplay.isLoaded = true;

                foreach (BonusEntry entry in editingDisplay.bonuses)
                {
                    if (entry.BonusType>0)
                    {
                        InsertAchievementOption(entry, achivID);
                    }
                }
                foreach (StatEntry entry in editingDisplay.stats)
                {
                    if (!entry.itemStatName.Equals("~ none ~"))
                    {
                        InsertAchievementStatOption(entry, achivID);
                    }
                }

                newItemCreated = true;
                linkedTablesLoaded = false;
                dataLoaded = false;
                Load();
                // Configure the correponding prefab
                NewResult(Lang.GetTranslate("New entry inserted"));
            }
            else
            {
                NewResult("Error" + Lang.GetTranslate("Error occurred, please check the Console"));
            }

        }

        void InsertAchievementOption(BonusEntry entry,int id)
        {
            string query = "INSERT INTO achievement_bonuses ";
            query += " (achievement_id ,bonus_settings_id ,value ,valuep ) ";
            query += "VALUES ";
            query += "("+id+ " ," +entry.BonusType+" ,"+entry.BonusValue + " ,"+ entry.BonusValuePercentage + ") ";
            int achivID = -1;
            List<Register> update = new List<Register>();
            achivID = DatabasePack.ExecuteNonQuery(DatabasePack.contentDatabasePrefix, query);
            entry.id = achivID;
        }

        void InsertAchievementStatOption(StatEntry entry, int id)
        {
            string query = "INSERT INTO achievement_stats ";
            query += " (achievement_id ,stat ,value ,valuep ) ";
            query += "VALUES ";
            query += "(" + id + " ,'" + entry.itemStatName + "' ," + entry.itemStatValue + " ," + entry.itemStatValuePercentagef + ") ";
            int achivID = -1;
            List<Register> update = new List<Register>();
            achivID = DatabasePack.ExecuteNonQuery(DatabasePack.contentDatabasePrefix, query);
           // entry.id = achivID;
        }


        // Update existing entries in the table based on the iddemo_table
        void UpdateEntry()
        {
            NewResult(Lang.GetTranslate("Updating..."));
            if (rows != null)
                rows.Clear();
            string query = "UPDATE "+ tableName;
            query += " SET ";
            query += " name =?name,type=?type ,description =?description,value=?value,objects=?objects  ";
            query += " WHERE id=?id";
            List<Register> update = new List<Register>();
            update.Add(new Register("name", "?name", MySqlDbType.VarChar, editingDisplay.Name.ToString(), Register.TypesOfField.String));
            update.Add(new Register("type", "?type", MySqlDbType.Int32, editingDisplay.type.ToString(), Register.TypesOfField.Int));
            update.Add(new Register("description", "?description", MySqlDbType.VarChar, editingDisplay.description, Register.TypesOfField.String));
            update.Add(new Register("value", "?value", MySqlDbType.Int32, editingDisplay.value.ToString(), Register.TypesOfField.Int));
            update.Add(new Register("objects", "?objects", MySqlDbType.String, string.Join(";", editingDisplay.objects.ConvertAll(i => i.ToString()).ToArray()), Register.TypesOfField.String));
            update.Add(new Register("id", "?id", MySqlDbType.Int32, editingDisplay.id.ToString(), Register.TypesOfField.Int));
            // Update the database
            DatabasePack.Update(DatabasePack.contentDatabasePrefix, query, update);

            string query2 = "DELETE FROM achievement_bonuses WHERE achievement_id=" + editingDisplay.id;

            // Setup the register data		
            List<Register> update1 = new List<Register>();
            update1.Add(new Register("achievement_id", "?achievement_id", MySqlDbType.Int32, editingDisplay.id.ToString(), Register.TypesOfField.Int));
            // Update the database
            DatabasePack.Update(DatabasePack.contentDatabasePrefix, query2, update1);
            foreach (BonusEntry entry in editingDisplay.bonuses)
            {
                if (entry.BonusType > 0)
                {
                    InsertAchievementOption(entry, editingDisplay.id);
                }
            }

            query2 = "DELETE FROM achievement_stats WHERE achievement_id=" + editingDisplay.id;

            // Setup the register data		
            update1 = new List<Register>();
            update1.Add(new Register("achievement_id", "?achievement_id", MySqlDbType.Int32, editingDisplay.id.ToString(), Register.TypesOfField.Int));

            DatabasePack.Update(DatabasePack.contentDatabasePrefix, query2, update1);

            foreach (StatEntry entry in editingDisplay.stats)
            {
                if (!entry.itemStatName.Equals("~ none ~"))
                {
                    InsertAchievementStatOption(entry, editingDisplay.id);
                }
            }


            // Update online table to avoid access the database again			
            // dataRegister[displayKeys[selectedDisplay]] = editingDisplay;
            linkedTablesLoaded = false;
            dataLoaded = false;
             // Remove the prefab
            // Configure the correponding prefab
            NewResult(Lang.GetTranslate("Entry") + "  " + editingDisplay.Name + " " + Lang.GetTranslate("updated"));
          Load();
         }





        // Delete entries from the table
        void DeleteEntry()
        {

            string query = "UPDATE " + tableName;
            query += " SET  isactive = 0 ";
            query += " WHERE id=" + editingDisplay.id;
            DatabasePack.Update(DatabasePack.contentDatabasePrefix, query, new List<Register>());

            // Update online table to avoid access the database again		
            selectedDisplay = -1;
            newSelectedDisplay = 0;
            dataLoaded = false;
            NewResult(Lang.GetTranslate("Entry")+" " + editingDisplay.Name + " " + Lang.GetTranslate("Deleted"));
             Load();
       }

        void DeleteForever(int id)
        {
            if (EditorUtility.DisplayDialog(Lang.GetTranslate("Delete") + " " + Lang.GetTranslate(functionName) + " " + Lang.GetTranslate("entry") + " ?", Lang.GetTranslate("Are you sure you want to delete") + " ?", Lang.GetTranslate("Delete"), Lang.GetTranslate("Do Not delete")))
            {
                NewResult(Lang.GetTranslate("Deleting..."));
                List<Register> where = new List<Register>();
                where.Add(new Register("id", "?id", MySqlDbType.Int32, id.ToString(), Register.TypesOfField.Int));
                DatabasePack.Delete(DatabasePack.contentDatabasePrefix, tableName, where);

                dataLoaded = false;
                dataRestoreLoaded = false;
                // Load();
                NewResult(Lang.GetTranslate("Entry Permanent Deleted"));
            }
        }

        void RestoreEntry(int id)
        {
            NewResult(Lang.GetTranslate("Restoring..."));
            string query = "UPDATE " + tableName + " SET isactive = 1 where id = " + id;
            DatabasePack.Update(DatabasePack.contentDatabasePrefix, query, new List<Register>());
            dataLoaded = false;
            dataRestoreLoaded = false;
            //Load();
            NewResult(Lang.GetTranslate("Entry Restored"));
        }

        void Duplicate()
        {
            // Update name of current entry by adding "1" to the end
            editingDisplay.Name = editingDisplay.Name + " (Clone)";
            editingDisplay.id = -1;
            InsertEntry();
            state = State.Loaded;
            linkedTablesLoaded = false;
            dataLoaded = false;
            Load();
        }

        public int GetOptionPosition(int id, int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == id)
                    return i;
            }
            return 0;
        }

    }
}