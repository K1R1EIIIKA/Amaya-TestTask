using System;
using DG.Tweening;
using UnityEngine;
using CellLogic;

namespace Logic
{
    public class ClickHandler : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private LevelTask _levelTask;
        [SerializeField] private WinLogic _winLogic;
        [SerializeField] ElementsAnimator _elementsAnimator;

        [NonSerialized] public bool CanClick = true;
        
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && CanClick)
                HandleMouseClick();
        }

        private void HandleMouseClick()
        {
            var hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
                HandleCellInteraction(hit.collider.GetComponent<Cell>());
        }

        private void HandleCellInteraction(Cell cell)
        {
            if (cell == null) return;

            _elementsAnimator.BounceCell(cell);

            if (cell.CellLetter == _levelTask.TaskLetter)
            {
                _elementsAnimator.SpawnParticle(cell);
                _winLogic.Win();
            }
        }
    }
}