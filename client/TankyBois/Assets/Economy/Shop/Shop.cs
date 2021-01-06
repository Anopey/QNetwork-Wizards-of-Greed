using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{

    [SerializeField]
    private GameObject templateShopCard;
    [SerializeField]
    private GameObject templateShopContract;

    public CardShop cardShop { get; private set; }
    public ContractShop contractShop { get; private set; }

    public Dictionary<GameObject, int> cardButtonDict { get; private set; }
    public Dictionary<GameObject, int> contractButtonDict { get; private set; }
    public static Shop Singleton { get; private set; }


    private void Start()
    {
        if (!Singleton)
            Singleton = this;
        else
            Debug.LogError("Singleton already exists");

        int counter = 0;

        foreach (Card card in cardShop.cards) //create the card buttons based on templatebutton
        {
            GameObject duplicate = Instantiate(templateShopCard, templateShopCard.transform.parent);
            duplicate.transform.position = new Vector3(templateShopCard.transform.position.x + counter * 300, templateShopCard.transform.position.y, templateShopCard.transform.position.z);
            duplicate.SetActive(true);
            cardButtonDict.Add(duplicate, counter);

            counter++;
        }

        counter = 0;

        foreach (Contract contract in contractShop.contracts) //do the same but for contracts
        {
            GameObject duplicate = Instantiate(templateShopContract, templateShopContract.transform.parent);
            duplicate.transform.position = new Vector3(templateShopContract.transform.position.x + counter * 300, templateShopContract.transform.position.y, templateShopContract.transform.position.z);
            duplicate.SetActive(true);
            contractButtonDict.Add(duplicate, counter);

            counter++;
        }
    }

    public Shop()
    {
        cardShop = new CardShop();
        contractShop = new ContractShop();
        cardButtonDict = new Dictionary<GameObject, int>();
        contractButtonDict = new Dictionary<GameObject, int>();
    }

    public void CycleCardShop()
    {
        //GENERATE CARD

        //UPDATE DICTIONARY

        //UPDATE BUTTONS
    }

    public void CycleContractShop()
    {
        //DITTO
    }
}
