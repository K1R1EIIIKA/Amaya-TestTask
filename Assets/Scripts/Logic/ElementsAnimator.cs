using System;
using CellLogic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Logic
{
    public class ElementsAnimator : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private GameObject _particlePrefab;
        [SerializeField] private float _shakeDuration = 0.5f;
        [SerializeField] private float _bounceDuration = 0.5f;
        [SerializeField] private float _bounceDelay = 0.1f;
        [SerializeField] private float _fadeDuration = 2f;
        
        [Header("Dependencies")]
        [SerializeField] WinLogic _winLogic;
        
        public void SpawnParticle(Cell cell)
        {
            Transform cellTransform = cell.transform;
            GameObject particle = Instantiate(_particlePrefab, cellTransform.position - Vector3.forward,
                Quaternion.identity, cellTransform);

            Destroy(particle, _winLogic.WinTime);
        }

        public void BounceCell(Cell cell)
        {
            if (cell.IsShaking) return;

            cell.IsShaking = true;

            cell.LetterObject.transform.DOShakePosition(_shakeDuration, new Vector3(0.3f, 0, 0), 10, 0, false, true)
                .SetEase(Ease.InBounce)
                .OnComplete(() => FinishShake(cell))
                .SetLink(cell.gameObject);
        }
        
        public void BounceAppearingCell(Cell cell, float x, float y)
        {
            cell.transform.localScale = Vector3.zero;
            cell.transform.DOScale(Vector3.one, _bounceDuration)
                .SetEase(Ease.OutBounce)
                .SetDelay(x * _bounceDelay + y * _bounceDelay)
                .SetLink(cell.gameObject);
        }

        private void FinishShake(Cell cell)
        {
            cell.IsShaking = false;
        }
        
        public void FadeText(TextMeshProUGUI taskText, LevelTask levelTask)
        {
            taskText.DOFade(0, 0).SetLink(levelTask.gameObject);
            taskText.DOFade(1, _fadeDuration).SetLink(levelTask.gameObject);
        }
        
        public void FadePanel(UnityEvent task, Image loadingImage, float fadeDuration)
        {
            loadingImage.DOFade(0, 0);
            loadingImage.DOFade(1, fadeDuration).OnComplete(task.Invoke);
        }
    }
}