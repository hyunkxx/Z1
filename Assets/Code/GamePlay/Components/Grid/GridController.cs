using UnityEngine;

public class GridController : Singleton<GridController>
{
    int rows = 4;
    int cols = 9;
    float cellSize = 1f;
    Vector2 gridOrigin = new Vector2(-4f, 1.75f);

    public GameObject GridPrefabs;
    public GameObject GridGroup;
    public GameObject WaitingGridGroup;
    GridSlot[] grid;

    int GetIndex(int x, int y) => y * cols + x;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.GameMode.OnChangeGameState += OnChangeGameState;
    }
    protected override void Update()
    {
        base.Update();
    }

    private void OnChangeGameState(EGameState state)
    {
        switch (state)
        {
            case EGameState.EnterGame:
                CreateGrid();
                break;
        }
    }

    void CreateGrid()
    {
        grid = new GridSlot[rows * cols];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject cell = Instantiate(GridPrefabs, GridGroup.transform);
                cell.transform.position = GridToWorld(x, y);
                grid[GetIndex(x, y)] = cell.GetComponent<GridSlot>();
            }
        }
    }

    public GridSlot GetEmptyWaitingGrid()
    {
        for(int i = 0; i < WaitingGridGroup.transform.childCount; ++i)
        {
            if(WaitingGridGroup.transform.GetChild(i).GetComponent<GridSlot>().CurObject == null)
            {
                return WaitingGridGroup.transform.GetChild(i).GetComponent<GridSlot>();
            }
        }

        return null;
    }

    public bool isClampPos(Vector2 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt(-(worldPos.y - gridOrigin.y) / cellSize);

        if (x > cols - 1 || y > rows - 1 || x < 0 || y < 0)
            return false;

        return true;
    }

    public GridSlot GetNearestCell(Vector2 worldPos)
    {
        if (!isClampPos(worldPos)) return null;

        int x = Mathf.RoundToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt(-(worldPos.y - gridOrigin.y) / cellSize);

        x = Mathf.Clamp(x, 0, cols - 1);
        y = Mathf.Clamp(y, 0, rows - 1);

        return grid[GetIndex(x, y)];
    }

    public Vector2 GridToWorld(float x, float y)
    {
        return gridOrigin + new Vector2(x * cellSize, -y * cellSize);
    }

    public Vector2 SnapPos(float x, float y)
    {
        return new Vector2(x, y);
    }

    public void SwapGrid(GridSlot _curSlot, GridSlot _nextSlot)
    {
        if(_nextSlot.isStay)
        {
            GameObject TempObj = _curSlot.CurObject;
            TempObj = _curSlot.CurObject;
            _curSlot.CurObject = _nextSlot.CurObject;
            _nextSlot.CurObject = TempObj;

            _curSlot.CurObject.transform.position = _curSlot.transform.position;
            _nextSlot.CurObject.transform.position = _nextSlot.transform.position;
        }
        else
        {
            _nextSlot.isStay = true;
            _curSlot.isStay = false;

            _nextSlot.CurObject = _curSlot.CurObject;
            _curSlot.CurObject = null;

            _nextSlot.CurObject.transform.position = _nextSlot.transform.position;
        }

    }
}
