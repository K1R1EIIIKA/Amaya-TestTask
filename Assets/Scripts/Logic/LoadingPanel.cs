using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Logic
{
    public class LoadingPanel : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private Image _loadingImage;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private GameObject _endGamePanel;

        [Header("Dependencies")] 
        [SerializeField] private ClickHandler _clickHandler;
        [SerializeField] private ElementsAnimator _elementsAnimator;

        public void LoadPanel(UnityEvent task)
        {
            gameObject.SetActive(true);
            _elementsAnimator.FadePanel(task, _loadingImage, _fadeDuration);

            StartCoroutine(UnloadPanel());
        }

        public void LoadEndPanel()
        {
            gameObject.SetActive(true);
            _elementsAnimator.FadeEndPanel(_loadingImage, _fadeDuration, 1f);
        }

        private IEnumerator UnloadPanel()
        {
            yield return new WaitForSeconds(_fadeDuration);
            _endGamePanel.SetActive(false);

            _clickHandler.SetCanClick(true);
            _loadingImage.DOFade(0, _fadeDuration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}