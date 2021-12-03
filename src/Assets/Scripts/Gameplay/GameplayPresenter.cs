using Promises;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class GameplayPresenter : MonoBehaviour
    {
        [SerializeField] private WheelComponentView wheelComponentView;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button spinButton;
        [SerializeField] private Text initialWinValueText;
        [SerializeField] private Text multiplierValueText;
        [SerializeField] private Text finalWinValueText;

        private IDailyBonusService _dailyBonusService;
        private ICoroutineService _coroutineService;
        private int _currentInitialWin;
        private int _currentMultiplier;
        private int _currentFinalWin;

        private void Awake()
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            spinButton.onClick.AddListener(OnSpinButtonClicked);
        }

        public void Start()
        {
            IServiceLocator serviceLocator = new ServiceLocator(this);
            
            _dailyBonusService = serviceLocator.GetService<IDailyBonusService>();
            _coroutineService = serviceLocator.GetService<ICoroutineService>();

            UpdateInitialWinValue();
        }

        private void UpdateInitialWinValue()
        {
            IPromise<int> initialWinPromise = _dailyBonusService.GetInitialWinPromise();
            _coroutineService.ExecutePromise(initialWinPromise).Then(UpdateInitialWinValue);
        }
        
        private void UpdateInitialWinValue(int initialWin)
        {
            _currentInitialWin = initialWin;
            initialWinValueText.text = _currentInitialWin.ToString("N");
        }
        
        private void UpdateMultiplierValue()
        {
            IPromise<int> multiplierPromise = _dailyBonusService.GetMultiplierPromise();
            _coroutineService.ExecutePromise(multiplierPromise).Then(UpdateMultiplierValue);
        }

        private void UpdateMultiplierValue(int multiplier)
        {
            _currentMultiplier = multiplier;
            multiplierValueText.text = _currentMultiplier.ToString();

            UpdateFinalWinValue();
            UpdatePlayerBalance();
        }
        
        private void UpdateFinalWinValue()
        {
            _currentFinalWin = _currentInitialWin * _currentMultiplier;
            finalWinValueText.text = _currentFinalWin.ToString("N");
        }

        private void UpdatePlayerBalance()
        {
            _dailyBonusService.SetPlayerBalance(_currentFinalWin);
            
            IPromise<long> playerBalancePromise = _dailyBonusService.GetPlayerBalancePromise();
            _coroutineService.ExecutePromise(playerBalancePromise).Then(delegate(long playerBalance)
            {
                Debug.Log($"playerBalance: {playerBalance}");
                
                wheelComponentView.StopSpin();
                spinButton.interactable = true;
            });
        }

        private void OnSpinButtonClicked()
        {
            spinButton.interactable = false;
            wheelComponentView.StartSpin();

            UpdateMultiplierValue();
        }

        private void OnCloseButtonClicked()
        {
            Application.Quit();
        }
    }
}