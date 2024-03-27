using System.Collections;
using UnityEngine;

namespace Logic
{
    public class WinLogic : MonoBehaviour
    {
        [SerializeField] private ClickHandler _clickHandler;
        [SerializeField] private LevelTask _levelTask;
        [SerializeField] private float _winTime = 2f;
        [SerializeField] private EndGameLogic _endGameLogic;
    
        public void Win()
        {
            StartCoroutine(WinCoroutine());
        }

        private IEnumerator WinCoroutine()
        {
            _clickHandler.CanClick = false;

            yield return new WaitForSeconds(_winTime);

            _clickHandler.CanClick = true;
        
            if (_endGameLogic.IsEndGame(_levelTask.CurrentLevel))
            {
                _endGameLogic.EndGame();
            }
            else
            {
                _levelTask.CurrentLevel++;
                _levelTask.CompleteTask();
            }
        }
    }
}