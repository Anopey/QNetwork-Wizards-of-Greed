using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory
{
    
    List<Card> cards = new List<Card>();

    public CardInventory()
    {
        cards.Add(new Card(2, 0, 0, 0, true));
        cards.Add(new UpgradeCard(2, true));
    }

    public bool BuyContract(SpiceInventory spiceInventory, ContractInventory contractInventory, Contract contract)
    {
        if (!contract.BuyContract(spiceInventory)) return false;

        contractInventory.contracts.Add(contract);

        return true;
    }

}
