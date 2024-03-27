using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic
{
    public class WinLogic : MonoBehaviour
    {
        [Header("Properties")]
        public float WinTime = 2f;
        
        [Header("Dependencies")]
        [SerializeField] private ClickHandler _clickHandler;
        [SerializeField] private LevelTask _levelTask;
        [SerializeField] private EndGameLogic _endGameLogic;
    
        public void Win()
        {
            StartCoroutine(WinCoroutine());
        }

        private IEnumerator WinCoroutine()
        {
            _clickHandler.CanClick = false;

            yield return new WaitForSeconds(WinTime);

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