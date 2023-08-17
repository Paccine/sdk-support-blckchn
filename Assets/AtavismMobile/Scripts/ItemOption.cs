using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Atavism;

namespace Atavism
{
    public class ItemOption : MonoBehaviour
    {

        public UGUIAtavismActivatable uGUIAtavismActivatable; //
        [SerializeField] TMP_InputField divideCount;
        [SerializeField] Transform slotsInventory;
        [SerializeField] Transform slotsEquip;
        [SerializeField] Transform slotsMerchant;
        [SerializeField] CanvasGroup merchantCanvas;
        [SerializeField] CanvasGroup UpgradeCanvas;
        [SerializeField] CanvasGroup BankCanvas;
        int count = 1;
        [SerializeField] GameObject DivideGO, EquipGO, UnequipGO, UpgradeGO, UseGO, CleanGO, MoveGO, SellGO, BuyGO;

        GameObject merchant01;
        GameObject merchant02;


        // Use this for initialization
        void Start()
        {
            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            divideCount.text = count.ToString();
            merchant01 = GameObject.Find("Dragonsan MerchantPanel");
            merchant02 = GameObject.Find("BankPanel");
        }


        public void SellOFF()
        {
            SellGO.SetActive(false);
        }

        public void Refresh()
        {
            if (uGUIAtavismActivatable != null)
            {
                // uGUIAtavismActivatable.Clicked();
#if AT_MOBILE                
                ActivatePanel(uGUIAtavismActivatable.state, uGUIAtavismActivatable.item);
#endif
            }
        }

        public void Equip()
        {
            if (uGUIAtavismActivatable != null)
            {
#if AT_MOBILE
                uGUIAtavismActivatable.OnClick();
                uGUIAtavismActivatable = null;
#endif
            }
        }


        public void Divide()
        {

            if (divideCount.text.Length > 0)
            {
                count = int.Parse(divideCount.text);
            }

            if (uGUIAtavismActivatable != null && count > 0)
            {
#if AT_MOBILE                
                uGUIAtavismActivatable.Divide(count);
#endif
            }
        }

        public void IncrementCount()
        {
            if (count < 199)
            {
                count++;
                divideCount.text = count.ToString();
            }
        }

        public void DecrementCount()
        {
            if (count > 1)
            {
                count--;
                divideCount.text = count.ToString();
            }
        }

        public void ChangeValue()
        {
            if (int.Parse(divideCount.text) > 199)
            {
                divideCount.text = "199";
            }

            if (int.Parse(divideCount.text) < 1)
            {
                divideCount.text = "1";
            }

            if (divideCount.text.Length == 0)
            {
                divideCount.text = "1";
            }

            count = int.Parse(divideCount.text);
        }


        public void Hide()
        {
            uGUIAtavismActivatable = null;
            GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void TurnOffAll()
        {
            EquipGO.SetActive(false);
            UnequipGO.SetActive(false);
            UpgradeGO.SetActive(false);
            UseGO.SetActive(false);
            CleanGO.SetActive(false);
            MoveGO.SetActive(false);
            DivideGO.SetActive(false);
            SellGO.SetActive(false);
            BuyGO.SetActive(false);
        }

        public void ActivatePanel(string state, AtavismInventoryItem item)
        {
            TurnOffAll();
            switch (state)
            {
                case "Upgrade":
                    CleanGO.SetActive(true);
                    break;
                case "Bank":

                    MoveGO.SetActive(true);
                    if (BankCanvas.alpha == 0)
                    {
                        Hide();
                    }

                    break;
                case "Merchant":

                    BuyGO.SetActive(true);
                    if (merchantCanvas.alpha == 0)
                    {
                        Hide();
                    }

                    break;
                case "Inventory":
                    if (item.itemType.ToString() == "Weapon" || item.itemType.ToString() == "Armor")
                    {
                        if (UpgradeCanvas.alpha == 1) // if Upgrade is open
                        {
                            UpgradeGO.SetActive(true);
                        }
                        else if (merchantCanvas.alpha == 1) // if Merchant is open
                        {
                            SellGO.SetActive(true);
                        }
                        else if (BankCanvas.alpha == 1) // if Bank is open
                        {
                            MoveGO.SetActive(true);
                        }
                        else
                        {
                            EquipGO.SetActive(true);
                        }

                    }
                    else
                    {
                        if (UpgradeCanvas.alpha == 1) // if Upgrade is open
                        {
                            UseGO.SetActive(true);
                        }
                        else if (merchantCanvas.alpha == 1) // if Merchant is open
                        {
                            SellGO.SetActive(true);
                        }
                        else if (BankCanvas.alpha == 1) // if Bank is open
                        {
                            MoveGO.SetActive(true);
                        }
                        else
                        {
                            if (item.itemType.ToString() == "Consumable")
                            {
                                UseGO.SetActive(true);
                            }

                            DivideGO.SetActive(true);
                        }
                    }

                    break;

                case "Equip":

                    if (merchantCanvas.alpha == 1) // if Merchant is open
                    {
                        UnequipGO.SetActive(true);
                    }
                    else
                    {
                        UnequipGO.SetActive(true);
                    }

                    break;
            }

            EquipGO.transform.SetAsLastSibling();
            UnequipGO.transform.SetAsLastSibling();
            UpgradeGO.transform.SetAsLastSibling();
            UseGO.transform.SetAsLastSibling();
            CleanGO.transform.SetAsLastSibling();
            MoveGO.transform.SetAsLastSibling();
            DivideGO.transform.SetAsLastSibling();
            SellGO.transform.SetAsLastSibling();
            BuyGO.transform.SetAsLastSibling();

        }
    }
}