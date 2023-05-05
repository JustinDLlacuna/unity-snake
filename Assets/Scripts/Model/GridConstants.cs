public class GridConstants
{
    public const int COLS = 30;
    public const int ROWS = 30;
    public const int START_X = COLS / 2;
    public const int START_Y = ROWS / 2;

    public static bool IsInvalidCoord(int x, int y)
    {
        return (x < 0) || (x >= COLS) || (y < 0) || (y >= ROWS);
    }
}
