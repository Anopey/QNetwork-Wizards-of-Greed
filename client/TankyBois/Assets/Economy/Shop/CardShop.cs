using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShop
{
    public List<Card> cards { get; private set; }

    public CardShop()
    {
        cards = new List<Card>();

        cards.Add(new IncomeCard(3));
        cards.Add(new TradeCard(-3, 0, 0, 1));
        cards.Add(new TradeCard(3, -1));
        cards.Add(new IncomeCard(1, 1));
        cards.Add(new TradeCard(2, 3, -2));
        cards.Add(new TradeCard(3, -2, 1));
    }

    public bool BuyCard(SpiceInventory spiceInventory, CardInventory cardInventory, Card card, int cardIndex)
    {
        int sum = spiceInventory.t1SpiceCount + spiceInventory.t2SpiceCount + spiceInventory.t3SpiceCount + spiceInventory.t4SpiceCount;
        if (sum < 5 - cardIndex) return false;

        int price = 5 - cardIndex;

        cardInventory.AddCard(card);

        while (price > 0)
        {
            while (spiceInventory.t1SpiceCount > 0 && price > 0)
            {
                price -= 1;
                spiceInventory.ModifySpices(-1);
            }
            while (spiceInventory.t2SpiceCount > 0 && price > 0)
            {
                price -= 1;
                spiceInventory.ModifySpices(0, -1);
            }
            while (spiceInventory.t3SpiceCount > 0 && price >  0)
            {
                price -= 1;
                spiceInventory.ModifySpices(0, 0, -1);
            }
            while (spiceInventory.t4SpiceCount > 0 && price >  0)
            {
                price -= 1;
                spiceInventory.ModifySpices(0, 0, 0, -1);
            }
        }

        return true;
    }
}
