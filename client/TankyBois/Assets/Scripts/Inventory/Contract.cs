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
        set;
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

    public Contract GenerateContract()
    {
        var rand = new System.Random();

        int[] spices = new int[4] { 0, 0, 0, 0 };

        int curSpice = 0;
        int spiceCount = 0;
        while (spiceCount < 4) //always at least 4 spices
        {
            int curSpiceCount = rand.Next(0, curSpice + 2); //max 2 yellow, 3 red, 4 green, 5 brown

            if (curSpiceCount + spiceCount > 6) curSpiceCount = 6 - spiceCount; //max 5 spices total

            spices[curSpice] += curSpiceCount;
            spiceCount += curSpiceCount;
            curSpice++; //go to next spice (yellow -> red)
            curSpice %= 4; //if at spice 4 (past brown), go back to spice 0 (yellow)
        }

        Contract newContract = new Contract(spices[0], spices[1], spices[2], spices[3]);
        return newContract;
    }
}
