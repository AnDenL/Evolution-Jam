using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cells;
    [SerializeField] private LayerMask layers;
    [SerializeField] private GameObject overlay,selectType;
    [SerializeField] private GameObject[] possibleCells;
    private List<Transform> typesTr;
    private Cell selected = null,toched = null;

    public static int Energy = 100;

    enum SelectStage
    {
        ParentCell,
        Position,
        Type
    }
    SelectStage stage = SelectStage.ParentCell;
    private void Start()
    {
        typesTr = new List<Transform>();
        for (int i =0;i<selectType.transform.childCount;i++)
        {
            typesTr.Add(selectType.transform.GetChild(i));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) switch (stage)
            {
                case SelectStage.ParentCell:
                    selected = GetCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (selected!=null&&selected.gameObject.CompareTag("Player"))
                    stage = SelectStage.Position;
                    break;
                case SelectStage.Position:
                    toched = null;
                    toched = GetCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (toched!=null&&toched.CompareTag("Invincible"))
                    {
                        foreach (GameObject cell in possibleCells)cell.SetActive(false);
                        //selected = GetCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        toched.gameObject.SetActive(true);
                        stage = SelectStage.Type;
                        selectType.SetActive(true);
                    }
                    break;
                case SelectStage.Type:
                    Cell toched2 = GetCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (toched2 && toched2.CompareTag("CellType")) { 
                        int cellType = GetCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject.transform.GetSiblingIndex();
                        if (Purchase(cellType)) {
                            Instantiate(cells[cellType], toched.transform.position, Quaternion.identity);
                            selectType.SetActive(false);
                            selected = null;
                            overlay.SetActive(false);
                            stage = SelectStage.ParentCell;
                        }
                        else Debug.Log("Not enough energy to build this cell.");
                    }
                    break;
            }
        SetOverlay();
    }

    public static bool Purchase(int cost)
    {
        if (Energy < cost) return false;
        // else
        Energy -= cost;
        return true;
    }

    private void SetOverlay()
    {
       if (!selected)
        {
            overlay.SetActive(false);
            return;
        } 

        overlay.SetActive(true);
        Vector2 pos = selected.transform.position;
        overlay.transform.position = pos;
        float angle = 0;
        if (stage == SelectStage.Position)
        foreach (GameObject cell in possibleCells)
        {
            Vector2 checkPos = (Vector3)pos + Quaternion.Euler(0, 0, angle) * Vector3.up;
            angle += 60;
            Collider2D coll = Physics2D.OverlapCircle(checkPos, 0.2f, layers);
            if (!coll|| coll.isTrigger) cell.SetActive(true);
            else cell.SetActive(false);
        }
    }

    private Cell GetCell(Vector2 pos)
    {
        Collider2D coll = Physics2D.OverlapCircle(pos, 0.1f, layers);
        if (!coll) return null;
        if (coll.CompareTag("Enemy")) return null;
        Cell cell = coll.GetComponent<Cell>();
        cell.OnSelect();
        return cell;
    }
}