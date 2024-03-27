using Configs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class EndGameLogic : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private GameObject _endGamePanel;
        [SerializeField] private GameObject _finishGamePanel;
        
        [Header("Dependencies")]
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
        
        public void FinishGame()
        {
            _finishGamePanel.SetActive(true);
            _clickHandler.CanClick = false;
        }
        
        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}