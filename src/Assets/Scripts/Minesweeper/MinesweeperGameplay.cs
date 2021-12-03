using UnityEngine;

namespace Minesweeper
{
    /// <summary>
    /// MonoBehaviour to start the game.
    /// </summary>
    public class MinesweeperGameplay : MonoBehaviour
    {
        private MinesweeperPresenter _minesweeperPresenter;
        
        private void Awake()
        {
            _minesweeperPresenter = new MinesweeperPresenter();
        }
    }
}