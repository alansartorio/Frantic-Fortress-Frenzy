using System.Collections.Generic;
using UnityEngine;

public class BuyPanel : MonoBehaviour
{
    public List<DefenderData> defenders;
    public GameObject buySlot;

    void Start()
    {
        foreach (var defender in defenders)
        {
            var slot = Instantiate(buySlot, transform).GetComponent<BuySlot>();
            
            slot.defender = defender;
        }
    }
}
