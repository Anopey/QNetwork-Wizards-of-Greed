using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{

    [SerializeField]
    private GameObject templateContract;
    [SerializeField]
    private GameObject templateUpgradecard;
    [SerializeField]
    private GameObject templateIncomecard;
    [SerializeField]
    private GameObject templateTradecard;
    [SerializeField]
    private GameObject templateBonusSpices;
    
    public CardShop cardShop { get; private set; }
    public ContractShop contractShop { get; private set; }

    public Dictionary<GameObject, int> cardButtonDict { get; private set; }
    public Dictionary<GameObject, int> contractButtonDict { get; private set; }
    public static Shop Singleton { get; private set; }

    private int cardAmount = 6;
    private int contractAmount = 6;

    private void Start()
    {
        if (!Singleton)
            Singleton = this;
        else
            Debug.LogError("Singleton already exists");

        CreateCardButtons();
        CreateContractButtons();
    }

    public Shop()
    {
        cardShop = new CardShop();
        contractShop = new ContractShop();
        cardButtonDict = new Dictionary<GameObject, int>();
        contractButtonDict = new Dictionary<GameObject, int>();
    }

    private void CreateCardButtons()
    {
        int counter = 0;

        foreach (Card card in cardShop.cards) //create the card buttons based on templatebutton
        {
            Type t = card.GetType();

            GameObject duplicate = new GameObject();

            if (t == typeof(IncomeCard))
            {
                duplicate = Instantiate(templateIncomecard, templateIncomecard.transform.parent);
                duplicate.transform.SetSiblingIndex(counter);
            }
            else if (t == typeof(UpgradeCard))
            {
                duplicate = Instantiate(templateUpgradecard, templateUpgradecard.transform.parent);
                duplicate.transform.SetSiblingIndex(counter);
            }
            else if (t == typeof(TradeCard))
            {
                duplicate = Instantiate(templateTradecard, templateTradecard.transform.parent);
                duplicate.transform.SetSiblingIndex(counter);
            }

            duplicate.SetActive(true); //template button is default inactive
            cardButtonDict.Add(duplicate, counter);

            counter++;
        }
    }

    private void CreateContractButtons() //literally the same as CreateCardButtons but all card is replaced with contract
    {
        int counter = 0;

        foreach (Contract contract in contractShop.contracts)
        {
            Type t = contract.GetType();
            if (counter == contractAmount - 2) contract.bonusPoints = 1;
            if (counter == contractAmount - 1) contract.bonusPoints = 3;

              GameObject duplicate = Instantiate(templateShopContract, templateShopContract.transform.parent);
            duplicate.transform.position = new Vector3(templateShopContract.transform.position.x + counter * 300, templateShopContract.transform.position.y, templateShopContract.transform.position.z);
            duplicate.SetActive(true);
            contractButtonDict.Add(duplicate, counter);

            GameObject buttonText = duplicate.transform.Find("Text").gameObject;

            buttonText.GetComponent<Text>().text = $"Cost: {contract.t1Spice},{contract.t2Spice},{contract.t3Spice},{contract.t4Spice}";


            counter++;
        }
    }

    public void CycleCardShop(int cardIndex)
    {
        var rand = new System.Random();
        Card newCard = null;

        int cardType = rand.Next(1,11); //determine what type card will be created
        if (cardType < 7)
            newCard = new TradeCard().GenerateCard();
        else if (cardType < 10)
            newCard = new IncomeCard().GenerateCard();
        else
            newCard = new UpgradeCard().GenerateCard();
        

        cardShop.cards.RemoveAt(cardIndex); //remove bought card
        cardShop.cards.Insert(0, newCard);

        foreach(KeyValuePair<GameObject, int> keyValuePair in cardButtonDict) //Destroy old buttons
        {
            Destroy(keyValuePair.Key);
        }

        CreateCardButtons();

    }

    public void CycleContractShop(int contractIndex)
    {
        var rand = new System.Random();

        Contract newContract = new Contract().GenerateContract();


        contractShop.contracts.RemoveAt(contractIndex); //remove bought contract
        contractShop.contracts.Insert(0, newContract);

        foreach (KeyValuePair<GameObject, int> keyValuePair in contractButtonDict) //Destroy old buttons
        {
            Destroy(keyValuePair.Key);
        }

        CreateContractButtons();
    }
}
