﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField]
    public GameObject t1SpiceField;
    [SerializeField]
    public GameObject t2SpiceField;
    [SerializeField]
    public GameObject t3SpiceField;
    [SerializeField]
    public GameObject t4SpiceField;
    [SerializeField]
    public GameObject templateCardButton;


    public ContractInventory contractInventory { get; private set; }
    public SpiceInventory spiceInventory { get; private set; }
    public CardInventory cardInventory { get; private set; }

    List<GameObject> cardButtons;
    List<Card> displayedCards;

    private void Update()
    {
        //Debug.Log(displayedCards);
        if (displayedCards.Count != cardInventory.cards.Count)
        {
            displayedCards = new List<Card>();
            foreach (Card card in cardInventory.cards) displayedCards.Add(card);
            UpdateCards();
        }
          
        UpdateSpices();
    }

    public Player()
    {
        contractInventory = new ContractInventory();
        spiceInventory = new SpiceInventory();
        cardInventory = new CardInventory();
        cardButtons = new List<GameObject>();
        displayedCards = new List<Card>();
    }

    private void UpdateSpices()
    {
        t1SpiceField.GetComponent<Text>().text = "Tier 1 Spice Count: " + spiceInventory.t1SpiceCount.ToString();
        t2SpiceField.GetComponent<Text>().text = "Tier 2 Spice Count: " + spiceInventory.t2SpiceCount.ToString();
        t3SpiceField.GetComponent<Text>().text = "Tier 3 Spice Count: " + spiceInventory.t3SpiceCount.ToString();
        t4SpiceField.GetComponent<Text>().text = "Tier 4 Spice Count: " + spiceInventory.t4SpiceCount.ToString();
    }


    private void UpdateCards()
    {
        foreach (GameObject gameObject in cardButtons)
        {
            Destroy(gameObject);
        }

        cardButtons = new List<GameObject>();
        int yOffset = 0;

        foreach (Card card in cardInventory.cards)
        {
            Type t = card.GetType();
            GameObject duplicate = Instantiate(templateCardButton, templateCardButton.transform.parent);
            duplicate.transform.position = new Vector3(templateCardButton.transform.position.x, templateCardButton.transform.position.y + yOffset, templateCardButton.transform.position.z);
            duplicate.SetActive(true);
            duplicate.GetComponent<Button>().onClick.AddListener(() => card.ConsumeCard(spiceInventory));
            duplicate.GetComponent<Button>().onClick.AddListener(() => DisableButton(duplicate));

            GameObject buttonText = duplicate.transform.Find("Text").gameObject;
            if (t == typeof(IncomeCard))
            {
                IncomeCard incomeCard = (IncomeCard) card;
                buttonText.GetComponent<Text>().text = $"Income: {incomeCard.t1Spice},{incomeCard.t2Spice},{incomeCard.t3Spice},{incomeCard.t4Spice}";
            }
            else if (t == typeof(UpgradeCard))
            {
                UpgradeCard upgradeCard = (UpgradeCard) card;
                buttonText.GetComponent<Text>().text = $"Upgrade: {upgradeCard.upgradeCount} upgrades";
            }
            else if (t == typeof(TradeCard))
            {
                TradeCard tradeCard = (TradeCard) card;
                buttonText.GetComponent<Text>().text = $"Trade: {tradeCard.t1Spice},{tradeCard.t2Spice},{tradeCard.t3Spice},{tradeCard.t4Spice}";
            }

            cardButtons.Add(duplicate);

            yOffset -= 70;
        }
    }

    private void DisableButton(GameObject gameObject)
    {
        gameObject.GetComponent<Button>().interactable = false;
    }

    public void PickupCards()
    {
        cardInventory.PickupCards();
        foreach (GameObject gameObject in cardButtons)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void BuyCard(GameObject gameObject)
    {
        int cardIndex = Shop.Singleton.cardButtonDict[gameObject];

        bool successful = Shop.Singleton.cardShop.BuyCard(spiceInventory, cardInventory, Shop.Singleton.cardShop.cards[cardIndex], cardIndex);

        if (successful)
            Shop.Singleton.CycleCardShop();
    }

    public void BuyContract(GameObject gameObject)
    {
        int contractIndex = Shop.Singleton.contractButtonDict[gameObject];

        bool successful = Shop.Singleton.contractShop.BuyContract(spiceInventory, contractInventory, Shop.Singleton.contractShop.contracts[contractIndex]);

        if (successful)
            Shop.Singleton.CycleContractShop();
    }
}