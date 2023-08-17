using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Atavism
{
    public class SaveAccountsAsPlayerprefs : MonoBehaviour
    {
        [SerializeField] Toggle remember;

        [SerializeField] TMP_InputField username_if;

        [SerializeField] TMP_InputField password_if;


        private void Start()
        {
            if (PlayerPrefs.HasKey("saved_username"))
            {
                username_if.text = PlayerPrefs.GetString("saved_username");
                password_if.text = PlayerPrefs.GetString("saved_password");
            }

            if (PlayerPrefs.HasKey("remember"))
            {
                if (PlayerPrefs.GetInt("remember") == 1)
                {
                    remember.isOn = true;
                }
                else
                {
                    remember.isOn = false;
                }
            }
        }



        public void SaveAccount() //it must be called by the Login Button
        {
            if (remember.isOn)
            {
                PlayerPrefs.SetString("saved_username", username_if.text);
                PlayerPrefs.SetString("saved_password", password_if.text);
                PlayerPrefs.SetInt("remember", 1);
            }
            else
            {
                if (PlayerPrefs.HasKey("saved_username"))
                {
                    PlayerPrefs.DeleteKey("saved_username");
                    PlayerPrefs.DeleteKey("saved_password");
                    PlayerPrefs.SetInt("remember", 0);
                }
            }
        }

    }
}