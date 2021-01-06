using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IncomeCard : Card
{
    public int t1Spice
    {
        get;
        private set;
    }
    public int t2Spice
    {
        get;
        private set;
    }
    public int t3Spice
    {
        get;
        private set;
    }
    public int t4Spice
    {
        get;
        private set;
    }

    public IncomeCard(int t1Spice = 0, int t2Spice = 0, int t3Spice = 0, int t4Spice = 0, bool usable = true)
    {
        this.t1Spice = t1Spice;
        this.t2Spice = t2Spice;
        this.t3Spice = t3Spice;
        this.t4Spice = t4Spice;
        this.usable = usable;
    }

    public override bool ConsumeCard(SpiceInventory spiceInventory, int multiplier = 1)
    {
        if (!usable) return false;

        usable = false;
        spiceInventory.ModifySpices(t1Spice, t2Spice, t3Spice, t4Spice);
        return true;

    }

    public override Card GenerateCard()
    {
        var rand = new System.Random();

        int value = rand.Next(3, 5);
        int[] spices = new int[4] { 0, 0, 0, 0 };

        while (value > 0)
        {
            int curSpice = rand.Next(1, 5);
            if (curSpice > value) continue;

            spices[curSpice - 1]++;
            value -= curSpice;
        }

        Card newCard = new IncomeCard(spices[0], spices[1], spices[2], spices[3]);
        return newCard;
    }
}
