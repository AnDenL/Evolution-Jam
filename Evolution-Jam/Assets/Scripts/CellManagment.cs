using UnityEngine;
using UnityEngine.UI;
public class CellManagment : MonoBehaviour
{
    static public int curEnergy = 10;
    [SerializeField] private GameObject cellChoose;
    [SerializeField] private Text energyValueText;
    private void Start()
    {
        updateEnergyValue();
    }
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cellChoose.SetActive(false);
            //if click on void cell open menu with choose of type of cell to spawn
            //set pos of cell
            updateEnergyValue();
        }
    }
    void updateEnergyValue() => energyValueText.text = curEnergy.ToString();
}

