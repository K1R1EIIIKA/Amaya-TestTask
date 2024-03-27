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
        
        [Header("Dependencies")]
        [SerializeField] private ClickHandler _clickHandler;
        [SerializeField] private ElementsAnimator _elementsAnimator;
    
        public void LoadPanel(UnityEvent task)
        {
            gameObject.SetActive(true);
            _elementsAnimator.FadePanel(task, _loadingImage, _fadeDuration);

            StartCoroutine(UnloadPanel());
        }

        private IEnumerator UnloadPanel()
        {
            yield return new WaitForSeconds(_fadeDuration);
        
            _clickHandler.CanClick = true;
            _loadingImage.DOFade(0, _fadeDuration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}