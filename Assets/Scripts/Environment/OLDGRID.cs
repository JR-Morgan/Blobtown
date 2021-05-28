using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OLDGRID
{
    private int width;
    private int height;
    public float cellSize;
    private int[,] gridArray;
    private Vector3 originPosition;
    private TextMesh[,] debugTextArray;

    public OLDGRID(int width, int height, float cellSize, Vector3 originPosition, bool textBool)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x=0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (textBool)
                {
                    debugTextArray[x,y] = CreateWorldText(null, gridArray[x, y].ToString(), GetCellOriginWorldPosition(x,y) + new Vector3(cellSize/2,cellSize/2,0), 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                }
                Debug.DrawLine(GetCellOriginWorldPosition(x, y), GetCellOriginWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetCellOriginWorldPosition(x, y), GetCellOriginWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetCellOriginWorldPosition(0, height), GetCellOriginWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetCellOriginWorldPosition(width, 0), GetCellOriginWorldPosition(width, height), Color.white, 100f);

    }

    private Vector3 GetCellOriginWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public Vector3 GetCellCenterWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition + new Vector3(cellSize/2, cellSize/2, 0);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x < 0 || y <0 || x > width || y > height)
        {
            Debug.LogError("invalid grid position at: x=" + x + " y= " + y);
        }

        gridArray[x, y] = value;

    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }


    private TextMesh CreateWorldText(Transform parent, string text, Vector3 localposition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localposition;
        
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;

        return textMesh;
    }
}
