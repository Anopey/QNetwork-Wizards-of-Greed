using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    ContractInventory contractInventory;
    SpiceInventory spiceInventory;
    CardInventory cardInventory;

    public Player()
    {
        contractInventory = new ContractInventory();
        spiceInventory = new SpiceInventory();
        cardInventory = new CardInventory();
    }
}
