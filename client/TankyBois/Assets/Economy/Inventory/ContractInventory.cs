using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractInventory
{
    public List<Contract> contracts
    {
        get;
        set;
    }

    public ContractInventory()
    {
        contracts = new List<Contract>();
    }

    public void AddContract(Contract contract)
    {
        contracts.Add(contract);
    }

    public int CalculatePoints(SpiceInventory spiceInventory)
    {

        int totalPoints = 0;

        foreach (Contract contract in contracts)
        {
            totalPoints += contract.bonusPoints + contract.points;
        }

        totalPoints += spiceInventory.t2SpiceCount + spiceInventory.t3SpiceCount + spiceInventory.t4SpiceCount; //each non-yellow spice is worth a point

        return totalPoints;
    }

}
