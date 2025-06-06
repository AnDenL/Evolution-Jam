using UnityEngine;
using UnityEngine.Tilemaps;
public class Cell : Tile
{
    [SerializeField] private int energyCost = 1;
    [SerializeField] private int energyValue = 1;
    [SerializeField] private float hitpoints;
    public int EnergyCost => energyCost;
    public int EnergyValue => energyValue;
    public float Hitpoints => hitpoints;
    public void OnClick()
    {
        if (CellManagment.curEnergy >= energyCost)
        {
            CellManagment.curEnergy -= energyCost;
            CellManagment.curEnergy += energyValue;
            //CellManagment.Instance.UpdateEnergyValue();
            // logic for placing
        }
        else
        {
            Debug.Log("Not enough energy to place this cell.");
        }
    }
}
