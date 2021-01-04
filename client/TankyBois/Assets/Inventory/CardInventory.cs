using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory
{
    
    List<Card> cards = new List<Card>();

    public CardInventory()
    {
        cards.Add(new Card(2, 0, 0, 0, true));
        cards.Add(new UpgradeCard(2, true));
    }

}
