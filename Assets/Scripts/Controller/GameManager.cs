using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private bool isPaused;
    private bool isSnakeColorPrimary;
    private float currentTime;
    private float ticksPerSecond;
    private ScreenInput.Swipe previousSwipe;
    private Cell fruitCell;
    private Cell[,] grid;
    private LinkedList<Cell> snake;
    private GridRenderer gridRenderer;
    private UIManager uiManager;
    private ScoreKeeper scoreKeeper;
    private Settings settings;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        isSnakeColorPrimary = true;
        currentTime = Time.time;
        previousSwipe = ScreenInput.Swipe.Up;
        grid = new Cell[GridConstants.COLS, GridConstants.ROWS];

        //Populating grid with cells.
        for (int y = 0; y < GridConstants.ROWS; y++)
        {
            for (int x = 0; x < GridConstants.COLS; x++)
            {
                bool isCellStart = (x == GridConstants.START_X) && (y == GridConstants.START_Y);
                grid[x, y] = new Cell(x, y, (isCellStart ? Cell.CellState.Snake : Cell.CellState.Empty));
            }
        }

        //Instantiating snake.
        snake = new LinkedList<Cell>();
        snake.AddFirst(grid[GridConstants.START_X, GridConstants.START_Y]);

        scoreKeeper = ScoreKeeper.Instance;
        settings = Settings.Instance;
    }

    //Spawn fruit, draw grid, and stop game.
    private void Start()
    {
        uiManager = UIManager.Instance;

        gridRenderer = GridRenderer.Instance;
        gridRenderer.DrawGrid();
        gridRenderer.FadeCells();

        StopGame();
    }

    private void Update()
    {
        if (!isPaused)
        {
            ScreenInput.Swipe swipeThisFrame = ScreenInput.GetSwipe();
            bool canMoveSnake = CanMoveSnake(swipeThisFrame);

            if (currentTime < Time.time)
            {
                //Move snake based on swipe this frame. 
                bool didMove = MoveSnake(canMoveSnake ? swipeThisFrame : previousSwipe);

                if(didMove)
                    RefreshGrid();

                //If snake did not move this frame, kill snake and fade grid.
                if (!didMove)
                {
                    MusicPlayer.Instance.PlayGameOver();
                    return;
                }

                currentTime = Time.time + (1f / ticksPerSecond);
            } 
            
            //Assign previous swipe to swipe this frame if valid.
            if (canMoveSnake)
            {
                previousSwipe = swipeThisFrame;
            }
        }      
    }

    //Save high score and settings when app paused.
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveHighScore();
            settings.SaveSettings();
        }       
    }

    //Save high score and settings when app quit.
    private void OnApplicationQuit()
    {
        SaveHighScore();
        settings.SaveSettings();
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    //Loads ticks per second from settings, refreshes grid, resets current score, shows grid unfaded, and resumes game.
    public void StartGame()
    {
        ticksPerSecond = settings.TicksPerSecond;

        gridRenderer.ResetGrid();
        scoreKeeper.ResetCurrentScore();
        uiManager.ShowGamePanel();
        gridRenderer.UnFadeCells();

        Cell startCell = grid[GridConstants.START_X, GridConstants.START_Y];
        snake.AddFirst(startCell);
        startCell.State = Cell.CellState.Snake;

        SpawnFruit();
        RefreshGrid();
        ResumeGame();

        if(settings.UseMusic)
            MusicPlayer.Instance.PlayMusic();
    }

    //Snake can move if swipe is not None, is 1 snake, or next cell is not 2nd snake body.
    private bool CanMoveSnake(ScreenInput.Swipe swipe)
    {
        if (swipe.Equals(ScreenInput.Swipe.None))
        {
            return false;
        }

        if (snake.Count == 1)
        {
            return true;
        }
      
        Cell snakeHead = snake.First.Value;
        Cell secondSnake = snake.First.Next.Value;
        Vector2Int nextCoord = GetNextCoord(new Vector2Int(snakeHead.X, snakeHead.Y), swipe);

        return (nextCoord.x != secondSnake.X) && (nextCoord.y != secondSnake.Y);
    }
    
    private bool MoveSnake(ScreenInput.Swipe swipe)
    {       
        Cell snakeHead = snake.First.Value;
        Vector2Int nextCoord = GetNextCoord(new Vector2Int(snakeHead.X, snakeHead.Y), swipe);

        //Stops game if snake hits edges or itself.
        if (GridConstants.IsInvalidCoord(nextCoord.x, nextCoord.y) || grid[nextCoord.x, nextCoord.y].State.Equals(Cell.CellState.Snake))
        {
            StopGame();
            return false;
        }

        Cell nextCell = grid[nextCoord.x, nextCoord.y];

        snake.AddFirst(nextCell);

        //Move snake as normal if next cell is empty.
        if (nextCell.State.Equals(Cell.CellState.Empty))
        {
            snake.Last.Value.State = Cell.CellState.Empty;
            snake.RemoveLast();
        }
        //Spawn fruit if nextcell is fruit, increment current score, and play beep.
        else if (nextCell.State.Equals(Cell.CellState.Fruit))
        {
            SpawnFruit();
            uiManager.IncrementCurrentScore();

            if (uiManager.GetBeepToggleValue())
            {
                MusicPlayer.Instance.PlayEatFruit();
            }            
        }

        nextCell.State = Cell.CellState.Snake;

        return true;
    }

    private Color GetColor(Cell.CellState state)
    {
        switch (state) 
        {
            case Cell.CellState.Snake:
                return isSnakeColorPrimary ? settings.PrimaryColor : settings.SecondaryColor;

            case Cell.CellState.Fruit:
                return settings.FruitColor;

            case Cell.CellState.Empty:
            default:
                return settings.GridColor;
        }
    }

    private Vector2Int GetNextCoord(Vector2Int coord, ScreenInput.Swipe swipe)
    {
        switch (swipe)
        {
            case ScreenInput.Swipe.Up:
                return new Vector2Int(coord.x, coord.y + 1);

            case ScreenInput.Swipe.Left:
                return new Vector2Int(coord.x - 1, coord.y);

            case ScreenInput.Swipe.Down:
                return new Vector2Int(coord.x, coord.y - 1);

            case ScreenInput.Swipe.Right:
                return new Vector2Int(coord.x + 1, coord.y);

            case ScreenInput.Swipe.None:
            default:
                return coord;
        }
    }

    private void KillSnake()
    {
        //Settings each cell in snake to empty.
        foreach (Cell cell in snake)
        {
            cell.State = Cell.CellState.Empty;
        }

        snake.Clear();       
    }

    private void SpawnFruit()
    {
        List<Cell> gridList = new List<Cell>();

        //Adds each cell to grid list.
        foreach (Cell cell in grid)
        {
            gridList.Add(cell);
        }

        //Removes snake cells from grid list.
        foreach (Cell cell in snake)
        {
            gridList.Remove(cell);
        }

        int index = Random.Range(0, gridList.Count);

        if (fruitCell != null)
        {
            fruitCell.State = Cell.CellState.Empty;
        }
        
        //Spawn fruit at random cell.
        fruitCell = gridList[index];
        fruitCell.State = Cell.CellState.Fruit;
    }

    //Shows resume button, and sets time scale to 0.
    private void PauseGame()
    {
        uiManager.ShowResumeButton();
        Time.timeScale = 0;
        isPaused = true;
    }

    //Shows pause button, and sets time scale to 1.
    private void ResumeGame()
    {
        uiManager.ShowPauseButton();
        Time.timeScale = 1f;
        isPaused = false;
    }

    //Pauses game, updates high score of current tick, and show start panel.
    public void StopGame()
    {
        PauseGame();
        KillSnake();

        if(fruitCell != null)
            fruitCell.State = Cell.CellState.Empty;

        gridRenderer.ResetGrid();
        gridRenderer.FadeCells();
        MusicPlayer.Instance.StopMusic();
        scoreKeeper.UpdateHighScore((int)ticksPerSecond);
        uiManager.ShowStartPanel();
        previousSwipe = ScreenInput.Swipe.Up;
    }

    private void RefreshGrid()
    {
        float count = 0f;

        //Each cell is now default color.
        gridRenderer.RefreshGrid();

        isSnakeColorPrimary = true;
 
        //Redraws snake faded.
        foreach (Cell cell in snake)
        {         
            Color snakeColor = GetColor(cell.State);
            snakeColor.a = Mathf.Lerp(1f, 0.15f, count++ / snake.Count);
            gridRenderer.UpdateCell(cell.X, cell.Y, snakeColor);
            isSnakeColorPrimary = !isSnakeColorPrimary;
        }

        //Redraws fruit.
        if(fruitCell != null)
            gridRenderer.UpdateCell(fruitCell.X, fruitCell.Y, settings.FruitColor);
    }

    //Updates high score of current tick, and saves it.
    private void SaveHighScore()
    {
        scoreKeeper.UpdateHighScore((int)ticksPerSecond);
        scoreKeeper.SaveHighScore();
    }  

    private class Cell
    {
        public enum CellState
        {
            Snake,
            Fruit,
            Empty
        };

        private int x;
        private int y;

        private CellState cellState;

        public int X { get { return x; } }
        public int Y { get { return y; } }

        public CellState State { get { return cellState; } set { cellState = value; } }

        public Cell(int x, int y, CellState cellState = CellState.Empty)
        {
            this.x = x;
            this.y = y;
            this.cellState = cellState;
        }
    }
}
