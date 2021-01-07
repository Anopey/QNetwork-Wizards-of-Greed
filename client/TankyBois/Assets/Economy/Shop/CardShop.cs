using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardShop //hidden light bulb somewhere 😳
{
    public List<Card> cards { get; private set; }
    public List<int[]> bonusSpices { get; private set; }

    public CardShop()
    {
        cards = new List<Card>();

        cards.Insert(0, new IncomeCard(3));
        cards.Insert(0, new TradeCard(-3, 0, 0, 1));
        cards.Insert(0, new TradeCard(3, -1));
        cards.Insert(0, new IncomeCard(1, 1));
        cards.Insert(0, new TradeCard(2, 3, -2));
        cards.Insert(0, new TradeCard(3, -2, 1));

        bonusSpices = new List<int[]>();
        foreach (Card card in cards)
        {
            int[] temp = { 0, 0, 0, 0 };
            bonusSpices.Add(temp);
        }
    }

    public bool BuyCard(SpiceInventory spiceInventory, CardInventory cardInventory, Card card, int cardIndex)
    {
        int sum = spiceInventory.t1SpiceCount + spiceInventory.t2SpiceCount + spiceInventory.t3SpiceCount + spiceInventory.t4SpiceCount;
        if (sum < 5 - cardIndex) return false;

        int price = 5 - cardIndex; //# of spices the card costs

        cardInventory.AddCard(card);

        int[] currentSpices = { spiceInventory.t1SpiceCount, spiceInventory.t2SpiceCount, spiceInventory.t3SpiceCount, spiceInventory.t4SpiceCount };
        for (int i = 0; i < 4; i++)
        {
            price -= (currentSpices[i] < price ? currentSpices[i] : price);
            currentSpices[i] -= (currentSpices[i] < price ? currentSpices[i] : price);
        }

        spiceInventory.ModifySpices(currentSpices[0] - spiceInventory.t1SpiceCount, currentSpices[1] - spiceInventory.t2SpiceCount, currentSpices[2] - spiceInventory.t3SpiceCount, currentSpices[4] - spiceInventory.t4SpiceCount);

        ModifyBonusSpices(currentSpices[0] - spiceInventory.t1SpiceCount, currentSpices[1] - spiceInventory.t2SpiceCount, currentSpices[2] - spiceInventory.t3SpiceCount, currentSpices[4] - spiceInventory.t4SpiceCount);

        return true;
    }

    private void ModifyBonusSpices(int t1Spice, int t2Spice, int t3Spice, int t4Spice) //💡
    {
        int totalSpiceCount = t1Spice + t2Spice + t3Spice + t4Spice;
        int[] spices = { t1Spice, t2Spice, t3Spice, t4Spice };

        int leftMost = cards.Count-1; //bad name, oh well 🤷
        for (int i = cards.Count-1; i > 0; i--)
            if (bonusSpices[i].Sum() == bonusSpices[i - 1].Sum())
                leftMost = i-1;

        int index = leftMost

        while (totalSpiceCount > 0)
        {
            bonusSpices[index][]
        }
        

    }

    private void HighestValue(int[] spices)
    {

    }
}
