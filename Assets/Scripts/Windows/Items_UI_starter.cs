using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Items_UI_starter : MonoBehaviour
{

    private Dictionary<string, Vector2> cells_positions = new Dictionary<string, Vector2>();

    private Vector2 start_position;

    void Awake()
    {
        start_position = transform.GetChild(0).position;

        GridLayoutGroup gl = transform.gameObject.GetComponent<GridLayoutGroup>();

        gl.childAlignment = TextAnchor.LowerLeft;

        // —оздаю матриу позиций клеток
        Vector2[,] cells = new Vector2[4,5];
        
        int k = 1;


        

        for (int i = cells.GetLength(0)-1; i >= 0; i--)
        {

            for (int j = 0; j < cells.GetLength(1); j++)
            {

                float posX = start_position.x + (gl.cellSize.x + gl.spacing.x) * j;
                float posY = start_position.y + (gl.cellSize.y + gl.spacing.y) * i;
                cells_positions.Add($"ItemSlot({k})", new Vector2(posX, posY));
                k++;
            }
        }

    }



    public Vector2 GetDefaultPosition(string cellName)
    {
        try
        {
            return cells_positions[cellName];
        }
        catch (KeyNotFoundException ex)
        {
            Debug.Log($"»м€ €чейки {cellName} не найдено в списке!" + ex);
            return Vector2.zero;
        }
    }

}
