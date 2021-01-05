using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
