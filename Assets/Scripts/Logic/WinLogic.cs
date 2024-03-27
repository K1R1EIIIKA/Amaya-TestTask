using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic
{
    public class WinLogic : MonoBehaviour
    {
        [Header("Properties")]
        private float _winTime = 2f;
        public float WinTime => _winTime;
        
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
            _clickHandler.SetCanClick(false);

            yield return new WaitForSeconds(_winTime);

            _clickHandler.SetCanClick(true);
        
            if (_endGameLogic.IsEndGame(_levelTask.CurrentLevel))
            {
                _endGameLogic.EndGame();
            }
            else
            {
                _levelTask.LevelComplete();
                _levelTask.CompleteTask();
            }
        }
    }
}