using Configs;
using UnityEngine;

namespace Logic
{
    public class EndGameLogic : MonoBehaviour
    {
        [SerializeField] private GameObject _endGamePanel;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LevelTask _levelTask;
        [SerializeField] private ClickHandler _clickHandler;

        public bool IsEndGame(int currentLevel)
        {
            return currentLevel == _levelConfig.GridSizes.Length - 1;
        }
    
        public void EndGame()
        {
            _endGamePanel.SetActive(true);
            _clickHandler.CanClick = false;
        }
    
        public void RestartGame()
        {
            _endGamePanel.SetActive(false);
            _levelTask.CurrentLevel = 0;
            _levelTask.ResetTask();
        }
    }
}