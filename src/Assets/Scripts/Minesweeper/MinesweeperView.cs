using UnityEngine;

namespace Minesweeper
{
    public interface IMinesweeperView
    {
        int Rows { get; set; }
        int Columns { get; set; }

        void ShowBoard();
        void ShowRevealed(int row, int col, int adjacentMines);
        void ShowUnrevealed(int row, int col);
        void ShowMine(int row, int col);
        void ShowFlag(int row, int col);
    }

    /// <summary>
    /// It will display the board and squares to the player.
    /// A presenter is responsible to send data to the view thou.
    /// </summary>
    public class MinesweeperView : IMinesweeperView
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        
        public void ShowBoard()
        {
            throw new System.NotImplementedException();
        }

        public void ShowRevealed(int row, int col, int adjacentMines)
        {
            throw new System.NotImplementedException();
        }

        public void ShowUnrevealed(int row, int col)
        {
            throw new System.NotImplementedException();
        }

        public void ShowMine(int row, int col)
        {
            throw new System.NotImplementedException();
        }

        public void ShowFlag(int row, int col)
        {
            throw new System.NotImplementedException();
        }
    }
}