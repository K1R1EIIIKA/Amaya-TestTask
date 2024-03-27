using System.Collections;
using Configs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Logic
{
    public class EndGameLogic : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private GameObject _endGamePanel;
        [SerializeField] private Image _endGameImage;
        
        [Header("Dependencies")]
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LevelTask _levelTask;
        [SerializeField] private ClickHandler _clickHandler;
        [SerializeField] private ElementsAnimator _elementsAnimator;

        public bool IsEndGame(int currentLevel)
        {
            return currentLevel == _levelConfig.GridSizes.Length - 1;
        }
    
        public void EndGame()
        {
            _endGamePanel.SetActive(true);
            _endGameImage.color = new Color(0, 0, 0, 0);
            _elementsAnimator.FadePanel(_endGameImage, 0.25f, 0.85f);
            
            _clickHandler.SetCanClick(false);
        }
    
        public void RestartGame()
        {
            _levelTask.LevelReset();
            _levelTask.ResetTask();
        }
    }
}