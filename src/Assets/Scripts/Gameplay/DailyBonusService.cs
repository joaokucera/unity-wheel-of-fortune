using Promises;
using Server.API;

namespace Test
{
    public interface IDailyBonusService
    {
        void SetPlayerBalance(long playerBalance);
        IPromise<long> GetPlayerBalancePromise();
        IPromise<int> GetInitialWinPromise();
        IPromise<int> GetMultiplierPromise();
    }

    /// <summary>
    /// Service/Mediator between external DLL/plugin and the game.
    /// </summary>
    public class DailyBonusService : IDailyBonusService
    {
        private readonly GameplayApi _gameplayApi;

        public DailyBonusService()
        {
            _gameplayApi = new GameplayApi();
            _gameplayApi.Initialise();
        }

        public void SetPlayerBalance(long playerBalance)
        {
            _gameplayApi.SetPlayerBalance(playerBalance);
        }

        public IPromise<long> GetPlayerBalancePromise()
        {
            return _gameplayApi.GetPlayerBalance();
        }

        public IPromise<int> GetInitialWinPromise()
        {
            return _gameplayApi.GetInitialWin();
        }

        public IPromise<int> GetMultiplierPromise()
        {
            return _gameplayApi.GetMultiplier();
        }
    }
}