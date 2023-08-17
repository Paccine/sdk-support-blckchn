using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Web3Wallet;
using TMPro;

namespace Atavism
{

    public class UGUITradeWindow : MonoBehaviour
    {
        [Header("P4 DEV STUDIO - WEB3 INTEGRATION")]
        public TMP_InputField toAccountInput;  // Conta para enviar erc20
        public TMP_InputField amountInput;  // Quantidade de tokens erc20 a enviar



        TradeToken tradeToken;
        public UGUIPanelTitleBar titleBar;
        public Text myName;
        public TextMeshProUGUI TMPMyName;
        public Text partnerName;
        public TextMeshProUGUI TMPPartnerName;
        public List<UGUITradeSlot> myOfferEntries;
        public List<Image> myCurrencyIcons;
        public List<InputField> myCurrencyInput;
        public List<TMP_InputField> TMPMyCurrencyInput;
        public List<UGUITradeOffer> partnerOfferEntries;
        public List<UGUICurrency> partnerOfferedCurrencies;
        public Text myStatus;
        public TextMeshProUGUI TMPMyStatus;
        public Text partnerStatus;
        public TextMeshProUGUI TMPPartnerStatus;
        bool showing = false;
        [SerializeField]
        GameObject panel;


        

        // Use this for initialization
        void Start()
        {
            if (titleBar != null)
                titleBar.SetOnPanelClose(CancelTrade);
            Hide();
            AtavismEventSystem.RegisterEvent("TRADE_START", this);
            AtavismEventSystem.RegisterEvent("TRADE_OFFER_UPDATE", this);
            AtavismEventSystem.RegisterEvent("TRADE_COMPLETE", this);
            AtavismEventSystem.RegisterEvent("ITEM_ICON_UPDATE", this);
        }

        void OnDestroy()
        {
            AtavismEventSystem.UnregisterEvent("TRADE_START", this);
            AtavismEventSystem.UnregisterEvent("TRADE_OFFER_UPDATE", this);
            AtavismEventSystem.UnregisterEvent("TRADE_COMPLETE", this);
            AtavismEventSystem.UnregisterEvent("ITEM_ICON_UPDATE", this);
        }

        public void Show()
        {
            GetComponent<CanvasGroup>().alpha = 1f;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            showing = true;

            //if (titleBar != null)
            //	titleBar.SetPanelTitle(ClientAPI.GetObjectNode(AtavismTrade.Instance.TradePartnerOid.ToLong()).Name);
            if (myName != null)
                myName.text = ClientAPI.GetPlayerObject().Name;
            if (TMPMyName != null)
                TMPMyName.text = ClientAPI.GetPlayerObject().Name;
            if (partnerName != null)
                partnerName.text = ClientAPI.GetObjectNode(AtavismTrade.Instance.TradePartnerOid.ToLong()).Name;

               if (TMPPartnerName != null)
                TMPPartnerName.text = ClientAPI.GetObjectNode(AtavismTrade.Instance.TradePartnerOid.ToLong()).Name;

            AtavismTrade.Instance.NewTradeStarted();
            AtavismUIUtility.BringToFront(gameObject);
            // Handle currency
            for (int i = 0; i < myCurrencyInput.Count; i++)
            {
                if (myCurrencyInput[i] != null)
                    myCurrencyInput[i].text = "";
            }
            for (int i = 0; i < TMPMyCurrencyInput.Count; i++)
            {
                if (TMPMyCurrencyInput[i] != null)
                    TMPMyCurrencyInput[i].text = "";
            }
            if (panel != null)
                panel.SetActive(true);
        }

        public void Hide()
        {
            if (panel != null)
                panel.SetActive(false);
            GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            showing = false;

            // Set all referenced items back to non referenced
            for (int i = 0; i < myOfferEntries.Count; i++)
            {
                myOfferEntries[i].ResetSlot();
            }
        }

        void UpdateTradeWindow()
        {
            for (int i = 0; i < myOfferEntries.Count; i++)
            {
                myOfferEntries[i].UpdateTradeSlotData(AtavismTrade.Instance.GetTradeItemInfo(true,i));
            }
            for (int i = 0; i < partnerOfferEntries.Count; i++)
            {
                partnerOfferEntries[i].UpdateTradeOfferData(AtavismTrade.Instance.GetTradeItemInfo(false,i));
            }

            // Handle currency
            for (int i = 0; i < myCurrencyIcons.Count; i++)
            {
                if (i < Inventory.Instance.GetMainCurrencies().Count)
                {
                    myCurrencyIcons[i].sprite = Inventory.Instance.GetMainCurrency(i).icon;
                }
            }

            for (int i = 0; i < partnerOfferedCurrencies.Count; i++)
            {
                if (i < AtavismTrade.Instance.TheirCurrencyOffers.Count)
                {
                    partnerOfferedCurrencies[i].gameObject.SetActive(true);
                    partnerOfferedCurrencies[i].SetCurrencyDisplayData(AtavismTrade.Instance.TheirCurrencyOffers[i]);
                }
                else
                {
                    partnerOfferedCurrencies[i].gameObject.SetActive(false);
                }
            }

            // If accepted set the colour of the panels
            if (AtavismTrade.Instance.AcceptedByMe)
            {
#if AT_I2LOC_PRESET
           if (myStatus!=null)  myStatus.text = I2.Loc.LocalizationManager.GetTranslation("Accepted");
           if (TMPMyStatus!=null)  TMPMyStatus.text = I2.Loc.LocalizationManager.GetTranslation("Accepted");
#else
                if (myStatus != null)
                    myStatus.text = "Accepted";
                if (TMPMyStatus != null)
                    TMPMyStatus.text = "Accepted";
#endif
            }
            else
            {
#if AT_I2LOC_PRESET
          if (myStatus!=null)   myStatus.text = I2.Loc.LocalizationManager.GetTranslation("Pending...");
          if (TMPMyStatus!=null)   TMPMyStatus.text = I2.Loc.LocalizationManager.GetTranslation("Pending...");
#else
                if (myStatus != null)
                    myStatus.text = "Pending...";
                if (TMPMyStatus != null)
                    TMPMyStatus.text = "Pending...";
#endif
            }

            if (AtavismTrade.Instance.AcceptedByPartner)
            {
#if AT_I2LOC_PRESET
           if (partnerStatus!=null)  partnerStatus.text = I2.Loc.LocalizationManager.GetTranslation("Accepted");
           if (TMPPartnerStatus!=null)  TMPPartnerStatus.text = I2.Loc.LocalizationManager.GetTranslation("Accepted");
#else
                if (partnerStatus != null)
                    partnerStatus.text = "Accepted";
                if (TMPPartnerStatus != null)
                    TMPPartnerStatus.text = "Accepted";
#endif
            }
            else
            {
#if AT_I2LOC_PRESET
          if (partnerStatus!=null)   partnerStatus.text = I2.Loc.LocalizationManager.GetTranslation("Pending...");
          if (TMPPartnerStatus!=null)   TMPPartnerStatus.text = I2.Loc.LocalizationManager.GetTranslation("Pending...");
#else
                if (partnerStatus != null)
                    partnerStatus.text = "Pending...";
                if (TMPPartnerStatus != null)
                    TMPPartnerStatus.text = "Pending...";
#endif
            }
        }

        public void OnEvent(AtavismEventData eData)
        {
            if (eData.eventType == "TRADE_START")
            {
                if (!showing)
                    Show();
                UpdateTradeWindow();
            }
            else if (eData.eventType == "TRADE_OFFER_UPDATE")
            {
                UpdateTradeWindow();
            }
            else if (eData.eventType == "ITEM_ICON_UPDATE")
            {
                UpdateTradeWindow();
            }
            else if (eData.eventType == "TRADE_COMPLETE")
            {
                OnTransferToken();
                Hide();
            }
        }

        /// <summary>
        /// Updates the currency amount for the first "main" currency
        /// </summary>
        /// <param name="currencyAmount">Currency amount.</param>
        public void SetCurrency1(string currencyAmount)
        {
            if (currencyAmount == "")
                currencyAmount = "0";
            AtavismTrade.Instance.SetCurrencyAmount(Inventory.Instance.GetMainCurrency(0).id, int.Parse(currencyAmount));
        }

        /// <summary>
        /// Updates the currency amount for the first "main" currency
        /// </summary>
        /// <param name="currencyAmount">Currency amount.</param>
        public void SetCurrency2(string currencyAmount)
        {
            if (currencyAmount == "")
                currencyAmount = "0";
            AtavismTrade.Instance.SetCurrencyAmount(Inventory.Instance.GetMainCurrency(1).id, int.Parse(currencyAmount));
        }

        /// <summary>
        /// Updates the currency amount for the first "main" currency
        /// </summary>
        /// <param name="currencyAmount">Currency amount.</param>
        public void SetCurrency3(string currencyAmount)
        {
            if (currencyAmount == "")
                currencyAmount = "0";
            AtavismTrade.Instance.SetCurrencyAmount(Inventory.Instance.GetMainCurrency(2).id, int.Parse(currencyAmount));
        }

        bool checkCurrency()
        {
            List<Vector2> currencies = new List<Vector2>();
            foreach (string currencyID in AtavismTrade.Instance.MyCurrencyOffers.Keys)
            {
                currencies.Add(new Vector2(int.Parse(currencyID), AtavismTrade.Instance.MyCurrencyOffers[currencyID]));
            }

            if (Inventory.Instance.DoesPlayerHaveEnoughCurrency(currencies))
            {
                Debug.Log("Player does have enough currency");
                return true;
            }
            Debug.Log("Player does not have enough currency");
            return false;
        }

        
        public void AcceptTrade()
        {
            AtavismTrade.Instance.AcceptTrade();
            
        }

        public void CancelTrade()
        {
            AtavismTrade.Instance.CancelTrade();
            Hide();
        }

        async public void OnTransferToken()
        {


            // https://chainlist.org/
            string chainId = "97"; // ID da Chain
                                   // contrato para interagir 
            string contract = "0xCc2EDAb5213bcf0eB3cE72fc11479d926cE3127f";
            // valor em wei
            string value = "0";
            // abi in json format
            string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\"," +
                "\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":" +
                "\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\"" +
                ":\"Approval\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":" +
                "\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}," +
                "{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"buy\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":" +
                "[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":" +
                "\"uint256\"}],\"name\":\"decreaseAllowance\"," +
                "\"outputs\":[{\"internalType\":\"bool\",\"name\":" +
                "\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\"," +
                "\"type\":\"uint256\"}],\"name\":\"farm\"," +
                "\"outputs\":[],\"stateMutability\":\"nonpayable\"," +
                "\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":" +
                "\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":" +
                "\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":" +
                "\"amount\",\"type\":\"uint256\"}]," +
                "\"name\":\"sell\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\"," +
                "\"type\":\"uint256\"},{\"internalType\":" +
                "\"contract IERC20\",\"name\":\"token\",\"type\":\"address\"}],\"name\":\"swap\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}," +
                "{\"inputs\":[{\"internalType\":\"address\"," +
                "\"name\":\"to\",\"type\":\"address\"}," +
                "{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\"," +
                "\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true," +
                "\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"}" +
                ",{\"indexed\":false," +
                "\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\"" +
                ",\"name\":\"from\",\"type\":" +
                "\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}]," +
                "\"name\":\"transferFrom\",\"outputs\"" +
                ":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"withdraw\"," +
                "\"outputs\":[],\"stateMutability\":" +
                "\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":" +
                "\"address\",\"name\":\"spender\",\"type\":\"address\"}]," +
                "\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":" +
                "\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}]," +
                "\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":" +
                "\"function\"},{\"inputs\":[],\"name\":" +
                "\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"}," +
                "{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":" +
                "\"string\",\"name\":\"\",\"type\":" +
                "\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":" +
                "\"address\"," +
                "\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":" +
                "\"view\",\"type\":\"function\"}," +
                "{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}]," +
                "\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\"," +
                "\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\"," +
                "\"name\":\"\",\"type\":\"address\"}],\"name\":\"yields\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\"," +
                "\"type\":\"function\"}]"; //ABI.ERC_20;

            // método para chamar no contrato
            string method = ETH_METHOD.Transfer; // "transferFrom";//ETH_METHOD.Transfer;

            // conta para qual vai enviar
            string toAccount = toAccountInput.text;

            // quantidade que vai enviar
            string amount = amountInput.text;

            // create data to interact with smart contract
            var contractData = new Contract(abi, contract);
            var data = contractData.Calldata(method, new object[]
            {
            toAccount,
            amount
            });
            // gas limit OPTIONAL
            string gasLimit = "";
            // gas price OPTIONAL
            string gasPrice = "";
            // send transaction
            string response = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
            print(response);
        }
    }
}