using System;
using DG.Tweening;
using UnityEngine;
using CellLogic;

namespace Logic
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private LevelTask _levelTask;
        [SerializeField] private float _shakeDuration = 0.5f;
        [SerializeField] private GameObject _particlePrefab;
        [SerializeField] private WinLogic _winLogic;

        private Camera _mainCamera;
        [NonSerialized] public bool CanClick = true;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && CanClick)
            {
                HandleMouseClick();
            }
        }

        private void HandleMouseClick()
        {
            var hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                HandleCellInteraction(hit.collider.GetComponent<Cell>());
            }
        }

        private void HandleCellInteraction(Cell cell)
        {
            if (cell != null)
            {
                BounceCell(cell);

                if (cell.CellLetter == _levelTask.TaskLetter)
                {
                    SpawnParticle(cell);
                    _winLogic.Win();
                }
            }
        }

        private void SpawnParticle(Cell cell)
        {
            Transform cellTransform = cell.transform;
            GameObject particle = Instantiate(_particlePrefab, cellTransform.position - Vector3.forward,
                Quaternion.identity, cellTransform);

            Destroy(particle, 2);
        }

        private void BounceCell(Cell cell)
        {
            if (cell.IsShaking) return;

            cell.IsShaking = true;

            cell.LetterObject.transform.DOShakePosition(_shakeDuration, new Vector3(0.3f, 0, 0), 10, 0, false, true)
                .SetEase(Ease.InBounce)
                .OnComplete(() => FinishShake(cell))
                .SetLink(cell.gameObject);
        }

        private void FinishShake(Cell cell)
        {
            cell.IsShaking = false;
        }
    }
}