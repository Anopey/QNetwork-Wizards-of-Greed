using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public bool usable;
    public abstract bool ConsumeCard(SpiceInventory spiceInventory, int multiplier = 1);
}