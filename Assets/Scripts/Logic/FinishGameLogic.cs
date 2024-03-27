using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Logic
{
    public class FinishGameLogic : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private GameObject _finishGamePanel;
        [SerializeField] private Image _finishGameImage;
        
        [Header("Dependencies")]
        [SerializeField] private ElementsAnimator _elementsAnimator;
        [SerializeField] private LoadingPanel _loadingPanel;
        [SerializeField] private ClickHandler _clickHandler;

        public void FinishGame()
        {
            _finishGamePanel.SetActive(true);
            _elementsAnimator.FadeEndPanel(_finishGameImage, 0.25f, 0.85f);
            
            _clickHandler.SetCanClick(false);
        }
        
        public void ResetGame()
        {
            _loadingPanel.LoadEndPanel();
            StartCoroutine(ReloadScene());
        }
        
        private IEnumerator ReloadScene()
        {
            yield return new WaitForSeconds(1f);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}