using TMPro;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [SerializeField] private GameObject[] cells;
    [SerializeField] private SpriteRenderer[] cellsRenderer;
    [SerializeField] private LayerMask layers;
    [SerializeField] private SpriteRenderer preview;
    [SerializeField] private TextMeshProUGUI energyAmount;

    private int? selected;
    [SerializeField] private int Energy;
    private Color red = new Color(1,0,0);
    private int[] prices = new int[] { 10, 50, 50, 50 };

    /*
                [SerializeField] private GameObject overlay,selectType;
                [SerializeField] private GameObject[] possibleCells;
                private List<Transform> typesTr;
                private Cell selected = null,toched = null;

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
                            angle -= 60;
                            Collider2D coll = Physics2D.OverlapCircle(checkPos, 0.2f, layers);
                            if (!coll || coll.isTrigger) cell.SetActive(true);
                            else cell.SetActive(false);
                        }
                    }
                    private Cell GetCell(Vector2 pos)
    {
        Collider2D coll = Physics2D.OverlapCircle(pos, 0.1f, layers);
        if (!coll) return null;
        Cell cell = coll.GetComponent<Cell>();
        cell.OnSelect();
        return cell;
    }
                */

    private void Start()
    {
        instance = this;
        SetEnergy(100);
        preview.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (selected != null)
        {
            if (CheckPosition(ToHexGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition))))
                 preview.color = cellsRenderer[(int)selected].color;
            else preview.color = red;

            if (Input.GetMouseButtonDown(0)) Place();

            preview.transform.position = ToHexGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void Select(int i)
    {
        selected = i;
        preview.gameObject.SetActive(true);
        preview.sprite = cellsRenderer[i].sprite;
        preview.color = cellsRenderer[i].color;
    }
    
    private bool Purchase(int cost)
    {
        if (Energy < cost) return false;
        // else
        SetEnergy(-cost);
        return true;
    }

    public void SetEnergy(int value)
    {
        Energy += value;
        energyAmount.text = " : " + Energy;
    }

    private void Place()
    {
        Vector2 pos = ToHexGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (CheckPosition(pos) && Purchase(prices[(int)selected]))
            Instantiate(cells[(int)selected], pos, Quaternion.identity);

        if (Input.GetKey(KeyCode.LeftControl) && Energy > prices[(int)selected]) return;
        
        selected = null;
        preview.gameObject.SetActive(false);
    }

    private bool CheckPosition(Vector2 pos)
    {
        if (Physics2D.OverlapCircle(pos, 0.2f, layers)) return false;
        else if (Physics2D.OverlapCircle(pos, 1f, layers)) return true;
        return false;
    }

    private Vector2 ToHexGrid(Vector2 pos)
    {
        float x = Mathf.Round(pos.x * 1.05f);
        float y = Mathf.Round(x % 2 == 0 ? pos.y + 0.25f : pos.y);

        float w = x % 2 == 0 ? y - 0.5f : y;

        return new Vector2(x * 0.95f, w);
    }
}
