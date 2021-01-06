using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card //copyrighted by ozan
{
    public bool usable;
    public abstract bool ConsumeCard(SpiceInventory spiceInventory, int multiplier = 1); //IncomeCard and UpgradeCard will always have multiplier defaulted to one.
}