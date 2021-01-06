using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCard : Card
{

    public int upgradeCount
    {
        get;
        private set;
    }

    public UpgradeCard(int upgradeCount = 0, bool usable = true)
    {
        this.upgradeCount = upgradeCount;
        this.usable = usable;
    }

    public override bool ConsumeCard(SpiceInventory spiceInventory, int multiplier = 1)
    {
        if (!usable) return false;

        //IMPLEMENT

        usable = false;
        return true;
    }

    public override Card GenerateCard()
    {
        var rand = new System.Random();

        int upgradeCount = rand.Next(2, 4);

        Card newCard = new UpgradeCard(upgradeCount);
        return newCard;
    }
}
