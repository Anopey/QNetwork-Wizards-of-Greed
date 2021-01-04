using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract
{

    int t1Spice;
    int t2Spice;
    int t3Spice;
    int t4Spice;

    public int points
    {
        get;
    }
    public int bonusPoints
    {
        get;
        private set;
    }

    public Contract(int t1Spice = 0, int t2Spice = 0, int t3Spice = 0, int t4Spice = 0, int bonusPoints = 0, int points = -1)
    {
        this.t1Spice = t1Spice;
        this.t2Spice = t2Spice;
        this.t3Spice = t3Spice;
        this.t4Spice = t4Spice;
        if (points == -1)
            this.points = t1Spice * 1 + t2Spice * 2 + t3Spice * 3 + t4Spice * 4;
        else
            this.points = points;
        this.bonusPoints = bonusPoints;
    }

    public bool BuyContract(SpiceInventory spiceInventory)
    {
        if (spiceInventory.t1SpiceCount < t1Spice && spiceInventory.t2SpiceCount < t2Spice &&
            spiceInventory.t3SpiceCount < t3Spice && spiceInventory.t4SpiceCount < t4Spice) return false;

        spiceInventory.ModifySpices(-t1Spice, -t2Spice, -t3Spice, -t4Spice);

        return true;
    }
}
