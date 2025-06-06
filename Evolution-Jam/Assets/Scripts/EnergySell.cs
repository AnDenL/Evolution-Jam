using System.Collections;
using UnityEngine;

public class EnergySell : Cell
{
    [SerializeField] private float produceTime;

    protected override void Start()
    {
        base.Start();
        Invoke("Produce", produceTime);
    }

    private void Produce()
    {
        BuildManager.instance.SetEnergy(3);
        Invoke("Produce", produceTime);
    }
}
