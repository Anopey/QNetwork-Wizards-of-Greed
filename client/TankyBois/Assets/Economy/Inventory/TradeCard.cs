using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeCard : Card
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
    public TradeCard(int t1Spice = 0, int t2Spice = 0, int t3Spice = 0, int t4Spice = 0, bool usable = true)
    {
        this.t1Spice = t1Spice;
        this.t2Spice = t2Spice;
        this.t3Spice = t3Spice;
        this.t4Spice = t4Spice;
        this.usable = usable;
    }

    public override bool ConsumeCard(SpiceInventory spiceInventory, int multiplier = 1)
    {
        if (!usable && spiceInventory.t1SpiceCount > -t1Spice * multiplier
            && spiceInventory.t2SpiceCount > -t2Spice * multiplier
            && spiceInventory.t3SpiceCount > -t3Spice * multiplier
            && spiceInventory.t4SpiceCount > -t4Spice * multiplier) return false;

        usable = false;
        spiceInventory.ModifySpices(t1Spice * multiplier, t2Spice * multiplier, t3Spice * multiplier, t4Spice * multiplier);
        return true;

    }

    public override Card GenerateCard()
    {
        var rand = new System.Random();

        int spiceInputType = rand.Next(1, 5);
        int original = spiceInputType;
        int inputAmount = rand.Next(1, 7 - spiceInputType);
        int inputVal = inputAmount * spiceInputType;

        int outputVal = inputVal + rand.Next(2, 5);
        outputVal = outputVal > 10 ? 10 : outputVal;

        int[] spices = new int[4] { 0, 0, 0, 0 };

        while (outputVal > 0)
        {
            if (outputVal == spiceInputType)
            {
                outputVal = original;
                spices = new int[4] { 0, 0, 0, 0 };
            }

            int curSpice = rand.Next(1, 5);
            if (curSpice == spiceInputType || curSpice > outputVal) continue;

            spices[curSpice - 1]++;
            outputVal -= curSpice;
        }

        spices[spiceInputType - 1] = -inputAmount;

        Card newCard = new TradeCard(spices[0], spices[1], spices[2], spices[3]);

        return newCard;
    }
}
