using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory
{
    
    struct Card
    {
        int t1Spice;
        int t2Spice;
        int t3Spice;
        int t4Spice;
        bool usable;
    }

    List<Card> cards = new List<Card>();

    public CardInventory()
    {


    }
}
