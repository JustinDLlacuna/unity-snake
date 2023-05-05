using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridRenderer : MonoBehaviour
{
    private static GridRenderer instance;

    private bool gridSpawned;
    private bool cellsChanged;
    private GameObject[,] grid;
    private HashSet<GameObject> changedCells;

    private Settings settings;

    [Range(0, 1f)]
    [SerializeField] private float cellColorA;
    [SerializeField] private RectTransform parentRect;

    public static GridRenderer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridRenderer>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GridRenderer).Name;
                    instance = obj.AddComponent<GridRenderer>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

	        gridSpawned = false;
            cellsChanged = false;
            grid = new GameObject[GridConstants.COLS, GridConstants.ROWS];
            changedCells = new HashSet<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }

        settings = Settings.Instance;
    }

    public void DrawGrid()
    {
        if (gridSpawned)
        {
            return;
        }
        else if (grid == null)
        {
            grid = new GameObject[GridConstants.COLS, GridConstants.ROWS];
        }

        float rectWidth = parentRect.rect.width;
        float rectHeight = parentRect.rect.height;
        float widthRatio = rectWidth / GridConstants.COLS;
        float heightRatio = rectHeight / GridConstants.ROWS;
        float squareLength = widthRatio < heightRatio ? widthRatio : heightRatio;
        float halfSquareLength = squareLength / 2f;
        float xOffset = 0f;
        float yOffset = 0f;

        Color gridColor = Color.HSVToRGB(settings.GridHue.h, settings.GridHue.s, settings.GridHue.v);

        //Calculating offset of the cells based on the shortest side of the screen.
        if (squareLength == widthRatio)
        {
            yOffset += (rectHeight - (squareLength * GridConstants.ROWS)) / 2f;
        }
        else
        {
            xOffset += (rectWidth - (squareLength * GridConstants.COLS)) / 2f;
        } 

        //Drawing cells
        for (int y = 0; y < GridConstants.ROWS; y++)
        {
            for (int x = 0; x < GridConstants.COLS; x++)
            {
                GameObject cell = new GameObject();
                cell.name = "Cell " + x + ", " + y;
                cell.transform.SetParent(parentRect.transform);
                cell.transform.localScale = Vector3.one;

                Image image = cell.AddComponent(typeof(Image)) as Image;
                image.color = gridColor;

                RectTransform rectTransform = image.rectTransform;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;
                rectTransform.anchoredPosition3D = new Vector3(
                (x * squareLength) + halfSquareLength + xOffset, 
                (y * squareLength) + halfSquareLength + yOffset, parentRect.anchoredPosition3D.z);
                rectTransform.sizeDelta = new Vector2(squareLength, squareLength);

                grid[x, y] = cell;
            }
        }

        gridSpawned = true;
    }

    public void RefreshGrid()
    {
        if (!gridSpawned || !cellsChanged)
        {
            return; 
        }

        //Resetting each cell color to default.
        foreach (GameObject cell in changedCells)
        {
            cell.GetComponent<Image>().color = settings.GridColor;
        }

        changedCells.Clear(); 

        cellsChanged = false;
    }

    public void ResetGrid()
    {
        if (!gridSpawned)
        {
            return;
        }

        //Resetting each cell color to default.
        foreach (GameObject cell in grid)
        {

            cell.GetComponent<Image>().color = settings.GridColor;
        }

        changedCells.Clear();
        cellsChanged = false;
    }

    public void DestroyGrid()
    {
        if (!gridSpawned)
        {
            return;
        }

        foreach (GameObject cell in grid)
        {
            Destroy(cell);
        }

        grid = null;
        gridSpawned = false;
        cellsChanged = false;
    }

    public void UpdateCell(int x, int y, Color color)
    {
        if (!gridSpawned || GridConstants.IsInvalidCoord(x, y))
        {
            return;
        }

        GameObject cell = grid[x, y];

        if (cell.GetComponent<Image>().color.Equals(color))
        {
            return;
        }

        cell.GetComponent<Image>().color = color;

        if (color.Equals(settings.GridColor))
        {
            changedCells.Remove(cell);

            if (changedCells.Count == 0)
            {
                cellsChanged = false;
            }
        } 
        else
        {
            changedCells.Add(cell);
            cellsChanged = true;
        }     
    }

    public void FadeCells()
    {
        if (!gridSpawned)
        {
            return;
        }

        //Fading each cell.
        foreach (GameObject cell in grid)
        {
            Color cellColor = cell.GetComponent<Image>().color;
            Color fadedColor = new Color(cellColor.r, cellColor.g, cellColor.b, cellColorA);
            cell.GetComponent<Image>().color = fadedColor;
        }
    }

    public void UnFadeCells()
    {
        if (!gridSpawned)
        {
            return;
        }

        //Unfading each cell.
        foreach (GameObject cell in grid)
        {
            Color cellColor = cell.GetComponent<Image>().color;
            Color unfadedColor = new Color(cellColor.r, cellColor.g, cellColor.b, 1f);
            cell.GetComponent<Image>().color = unfadedColor;
        }
    }
}
