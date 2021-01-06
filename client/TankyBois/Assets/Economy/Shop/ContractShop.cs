using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractShop
{

    public List<Contract> contracts { get; private set; }

    public ContractShop()
    {
        contracts = new List<Contract>();

        contracts.Add(new Contract(0, 0, 3, 2));
        contracts.Add(new Contract(0, 3, 1, 2));
        contracts.Add(new Contract(0, 0, 0, 5));
        contracts.Add(new Contract(0, 0, 1, 4));
        contracts.Add(new Contract(0, 2, 2, 3));
        contracts.Add(new Contract(2, 2, 1, 1));
    }

    public bool BuyContract(SpiceInventory spiceInventory, ContractInventory contractInventory, Contract contract)
    {
        if (!(spiceInventory.t1SpiceCount > contract.t1Spice
            && spiceInventory.t2SpiceCount > contract.t2Spice
            && spiceInventory.t3SpiceCount > contract.t3Spice
            && spiceInventory.t4SpiceCount > contract.t4Spice)) return false; //if not capable of buying card

        spiceInventory.ModifySpices(-contract.t1Spice, -contract.t2Spice, -contract.t3Spice, -contract.t4Spice);
        contractInventory.AddContract(contract);

        return true;
    }
}
