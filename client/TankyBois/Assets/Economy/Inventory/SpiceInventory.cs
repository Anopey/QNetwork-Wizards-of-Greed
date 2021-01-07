using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceInventory
{

    public int t1SpiceCount;
    public int t2SpiceCount;
    public int t3SpiceCount;
    public int t4SpiceCount;

    public SpiceInventory(int t1SpiceCount = 0, int t2SpiceCount = 0, int t3SpiceCount = 0, int t4SpiceCount = 0)
    {
        this.t1SpiceCount = t1SpiceCount;
        this.t2SpiceCount = t2SpiceCount;
        this.t3SpiceCount = t3SpiceCount;
        this.t4SpiceCount = t4SpiceCount;
    }

    public void ModifySpices(int t1Spice = 0, int t2Spice = 0, int t3Spice = 0, int t4Spice = 0)
    {
        t1SpiceCount += t1Spice;
        t2SpiceCount += t2Spice;
        t3SpiceCount += t3Spice;
        t4SpiceCount += t4Spice;
    }

}
