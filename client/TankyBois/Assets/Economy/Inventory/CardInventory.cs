using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory
{

    public List<Card> cards
    { 
        get;
        private set;
    }

    public CardInventory()
    {
        cards = new List<Card>();
        cards.Add(new IncomeCard(2, 0, 3, 0, true));
        cards.Add(new UpgradeCard(2, true));
        cards.Add(new TradeCard(0, 2, -2, 1, true));
        cards.Add(new TradeCard(3, 1, 0, -1, true));
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
        
    }

    public void PickupCards()
    {
        foreach (Card card in cards)
        {
            card.usable = true;
        }
    }

}
