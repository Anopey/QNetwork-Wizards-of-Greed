using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract
{

    public int t1Spice { get; private set; }
    public int t2Spice { get; private set; }
    public int t3Spice { get; private set; }
    public int t4Spice { get; private set; }

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

    public void UpdateBonuspoints(int bonusPoints)
    {
        this.bonusPoints = bonusPoints;
    }
}
