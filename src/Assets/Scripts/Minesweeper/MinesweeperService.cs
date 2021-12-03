namespace Minesweeper
{
    /// <summary>
    /// It will listen to player's action (sent by MinesweeperEventArgs) and will update the model.
    /// And it will provide delegates/callbacks upon player's action.
    /// </summary>
    public class MinesweeperService
    {
        // Rows
        // Columns
        // Delegates/Events to handle every action (game won/lost, square revealed, square flagged, etc)
        private MinesweeperModel _minesweeperModel;

        private MinesweeperEventArgs _minesweeperEventArgs;
        
        public MinesweeperService(int rows, int columns)
        {
            // _minesweeperModel = new ...
        }
        
        // Method to execute every action (update game state, reveal a square, flag a square, etc)
    }
}