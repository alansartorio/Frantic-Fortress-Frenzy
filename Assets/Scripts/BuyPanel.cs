using System;
using System.Collections.Generic;
using UnityEngine;

public class BuyPanel : MonoBehaviour
{
    public List<DefenderData> defenders;
    public GameObject buySlot;
    public GameDirector gameDirector;

    void Start()
    {
        foreach (var defender in defenders)
        {
            var slot = Instantiate(buySlot, transform).GetComponent<BuySlot>();

            slot.defender = defender;
        }
        
        GetComponentsInChildren<BuySlot>().ForEach(s => s.PartialScoreChanged(gameDirector.partialScore));
    }

    private void OnEnable()
    {
        gameDirector.OnPartialScoreChange.AddListener(PartialScoreChanged);
    }

    private void OnDisable()
    {
        gameDirector.OnPartialScoreChange.RemoveListener(PartialScoreChanged);
    }

    private void PartialScoreChanged(int score)
    {
        GetComponentsInChildren<BuySlot>().ForEach(s => s.PartialScoreChanged(score));
    }
}