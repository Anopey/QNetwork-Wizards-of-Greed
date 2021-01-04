using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCard : Card
{

    int upgradeCount;
    bool usable;

    public UpgradeCard(int upgradeCount = 0, bool usable = true)
    {
        this.upgradeCount = upgradeCount;
        this.usable = usable;
    }

}
