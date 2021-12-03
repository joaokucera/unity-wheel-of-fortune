using System;

namespace Minesweeper
{
    public interface IMinesweeperPresenter
    {
        void NewGame();
        void SaveGame();
        void LoadGame();
        void ShowSettings();
        void ShowHighScores();
        void RevealSquare(int row, int col);
        void FlagSquare(int row, int col);
    }

    /// <summary>
    /// It will subscribe to the service's delegates/actions and will pass data to the view in order to update the visualization.
    /// </summary>
    public class MinesweeperPresenter : IMinesweeperPresenter
    {
        private MinesweeperService _minesweeperService;
        private MinesweeperView _minesweeperView;
        
        public void NewGame()
        {
            throw new NotImplementedException();
        }

        public void SaveGame()
        {
            throw new NotImplementedException();
        }

        public void LoadGame()
        {
            throw new NotImplementedException();
        }

        public void ShowSettings()
        {
            throw new NotImplementedException();
        }

        public void ShowHighScores()
        {
            throw new NotImplementedException();
        }

        public void RevealSquare(int row, int col)
        {
            throw new NotImplementedException();
        }

        public void FlagSquare(int row, int col)
        {
            throw new NotImplementedException();
        }
    }
}
