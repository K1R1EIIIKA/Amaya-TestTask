using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private Image _loadingImage;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private ClickHandler _clickHandler;
    
    public void LoadPanel(UnityEvent task)
    {
        gameObject.SetActive(true);
        
        _loadingImage.DOFade(0, 0);
        _loadingImage.DOFade(1, _fadeDuration).OnComplete(task.Invoke);
        
        StartCoroutine(UnloadPanel());
    }
    
    private IEnumerator UnloadPanel()
    {
        yield return new WaitForSeconds(_fadeDuration);
        
        _clickHandler.CanClick = true;
        _loadingImage.DOFade(0, _fadeDuration).OnComplete(OnUnloadComplete);
    }
    
    private void OnUnloadComplete()
    {
        gameObject.SetActive(false);
    }
}