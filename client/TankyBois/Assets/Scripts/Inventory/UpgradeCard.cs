using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : Card
{

    public int upgradeCount
    {
        get;
        private set;
    }

    int lastUpgrade = -1;
    int upgradesLeft = 0;

    private Coroutine upgradeProcess;

    public UpgradeCard(int upgradeCount = 0, bool usable = true)
    {
        this.upgradeCount = upgradeCount;
        this.usable = usable;
    }

    public override bool ConsumeCard(SpiceInventory spiceInventory, int multiplier = 1)
    {
        if (!usable) return false;

        upgradesLeft = upgradeCount;

        ProcessUpgrade(spiceInventory);

        usable = false;
        return true;
    }

    IEnumerator ProcessUpgrade(SpiceInventory spiceInventory)
    {
        if (lastUpgrade == 1)
        {
            spiceInventory.t1SpiceCount -= 1;
            spiceInventory.t2SpiceCount += 1;
            upgradesLeft -= 1;
            lastUpgrade = -1;
        }
        else if (lastUpgrade == 2)
        {
            spiceInventory.t2SpiceCount -= 1;
            spiceInventory.t3SpiceCount += 1;
            upgradesLeft -= 1;
            lastUpgrade = -1;
        }
        else if (lastUpgrade == 3)
        {
            spiceInventory.t3SpiceCount -= 1;
            spiceInventory.t4SpiceCount += 1;
            upgradesLeft -= 1;
            lastUpgrade = -1;
        }

        if (upgradeCount > 0)
        {
            yield return new WaitWhile(() => lastUpgrade == -1);
        }
    }
    
    public void UseUpgrade(int spiceTier)
    {
        lastUpgrade = spiceTier;
    }

    public override Card GenerateCard()
    {
        Card newCard = new UpgradeCard(3);
        return newCard;
    }
}
