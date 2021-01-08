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

        //REMOVE SPICES FROM INVENTORY EQUAL TO COST OF CARD
        int price = 5 - cardIndex;

        cardInventory.AddCard(card);

        int[] currentSpices = { spiceInventory.t1SpiceCount, spiceInventory.t2SpiceCount, spiceInventory.t3SpiceCount, spiceInventory.t4SpiceCount };
        for (int i = 0; i < 4; i++)
        {
            int temp = currentSpices[i] < price ? currentSpices[i] : price;
            price -= temp;
            currentSpices[i] -= temp;
        }

        int[] paidSpices = { spiceInventory.t1SpiceCount - currentSpices[0], spiceInventory.t2SpiceCount - currentSpices[1],
            spiceInventory.t3SpiceCount - currentSpices[2], spiceInventory.t4SpiceCount - currentSpices[3]};

        spiceInventory.ModifySpices(-paidSpices[0], -paidSpices[1], -paidSpices[2], -paidSpices[3]); //Remove spices from inventory

        spiceInventory.ModifySpices(bonusSpices[cardIndex][0], bonusSpices[cardIndex][1], bonusSpices[cardIndex][2], bonusSpices[cardIndex][3]); //add bonus spice to inventory
        bonusSpices[cardIndex] = new int[4] { 0, 0, 0, 0 };

        for (int i = cardIndex; i >= 1; i--)
        {
            bonusSpices[i] = bonusSpices[i - 1];
        }
        bonusSpices[0] = new int[4] {0, 0, 0, 0};

        ModifyBonusSpices(paidSpices);

        return true;
    }

    private void ModifyBonusSpices(int[] paidSpices) //💡
    {

        int totalSpiceCount = paidSpices.Sum();

        int index = bonusSpices.Count-1;

        while (totalSpiceCount > 0)
        {
            int highest = LowestValue(paidSpices);
            bonusSpices[index][highest]++;
            paidSpices[highest]--;
            if (index < 0) index = bonusSpices.Count - 1;
            totalSpiceCount--;
            index--;
        }

    }

    private int LowestValue(int[] spices)
    {
        int last = spices.Length;
        int index = 0;
        while (spices[index] == 0) index++;
        return index;
    }
}
