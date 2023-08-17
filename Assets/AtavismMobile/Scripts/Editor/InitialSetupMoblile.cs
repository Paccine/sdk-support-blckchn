using UnityEngine;
using UnityEditor;
using System.IO;

namespace Atavism
{
    [InitializeOnLoad]
    class InitialSetupMobile
    {
        static InitialSetupMobile()
        {

            if (!File.Exists(Path.GetFullPath("Assets/..") + "/InitialSetupMobile.txt"))
            {
                EditorApplication.update += Update;

            }
        }

        [MenuItem("Window/Atavism/Atavism Mobile Setup")]
        public static void SetupAtavismUnity()
        {
            Setup();
        }

        static void Update()
        {
            PlayerPrefs.SetInt("AtavismSetupMobile", 1);
            PlayerPrefs.Save();
            EditorApplication.update -= Update;
            TextWriter tw = new StreamWriter(Path.GetFullPath("Assets/..") + "/InitialSetupMobile.txt", true);
            tw.Close();
            Setup();
        }


        static void Setup()
        {
            string MainFolder = "Assets/Dragonsan/Scenes/Mobile/";
            string SceneType = ".unity";
            string ScenesLoginName = "Login";
            string ScenesMainWorldName = "MainWorld";
            string sceneL = MainFolder + ScenesLoginName + SceneType;
            string sceneMW = MainFolder + ScenesMainWorldName + SceneType;
            if (!File.Exists(sceneL))
            {
                return;
            }

            EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
            int i = 0;
            bool exist = false;
            for (i = 0; i < original.Length; i++)
            {
                if (original[i].path.Contains(ScenesLoginName))
                {
                    original[i].enabled = false;
                }

                if (original[i].path.Equals(sceneL))
                {
                    original[i].enabled = true;
                    exist = true;
                }
            }

            EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + (exist ? 0 : 1)];
            if (!exist)
            {
                System.Array.Copy(original, 0, newSettings, 1, original.Length);

                EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(sceneL, true);
                newSettings[0] = sceneToAdd;

            }
            else
            {
                System.Array.Copy(original, newSettings, original.Length);

            }

            EditorBuildSettings.scenes = newSettings;

            if (!File.Exists(sceneMW))
            {
                return;
            }

            original = EditorBuildSettings.scenes;
            i = 0;
            exist = false;
            for (i = 0; i < original.Length; i++)
            {
                if (original[i].path.Contains(ScenesMainWorldName))
                {
                    original[i].enabled = false;
                }

                if (original[i].path.Equals(sceneMW))
                {
                    original[i].enabled = true;
                    exist = true;
                }
            }

            newSettings = new EditorBuildSettingsScene[original.Length + (exist ? 0 : 1)];
            System.Array.Copy(original, newSettings, original.Length);
            int index = original.Length;
            if (!exist)
            {
                EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(sceneMW, true);
                newSettings[index] = sceneToAdd;
            }

            EditorBuildSettings.scenes = newSettings;

            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            if (!symbols.Contains("AT_MOBILE"))
            {
                symbols += ";" + "AT_MOBILE";
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
        }

    }
}
