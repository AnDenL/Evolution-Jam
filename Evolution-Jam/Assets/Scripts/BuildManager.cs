using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cells;
    [SerializeField] private LayerMask layers;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject[] possibleCells;

    private Cell selected = null;

    public static int Energy = 100;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) selected = GetCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
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
        foreach (GameObject cell in possibleCells)
        {
            Vector2 checkPos = (Vector3)pos + Quaternion.Euler(0, 0, angle) * Vector3.up;
            angle += 60;
            Collider2D coll = Physics2D.OverlapCircle(checkPos, 0.2f, layers);
            if (!coll) cell.SetActive(true);
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