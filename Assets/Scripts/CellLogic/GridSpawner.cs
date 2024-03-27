using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellLogic
{
    public class GridSpawner : MonoBehaviour
    {
        [NonSerialized] public bool IsFirstGrid = true;
    
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Vector2 _spacing;
        [SerializeField] private Transform _gridContainer;
    
        private Cell[,] _cells;
    
        public Cell[,] CreateGrid(Vector2Int gridSize, List<CellCharacteristic> _characteristics)
        {
            _cells = new Cell[gridSize.x, gridSize.y];
            Vector2 centerPoint = new Vector2(gridSize.x / 2, gridSize.y / 2);
        
            if (gridSize.x % 2 == 0)
                centerPoint.x -= 0.5f;
            if (gridSize.y % 2 == 0)
                centerPoint.y -= 0.5f;
        
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (_characteristics.Count == 0)
                        break;

                    SpawnCell(_characteristics, x, y, x - centerPoint.x, y - centerPoint.y);
                }
            }
        
            return _cells;
        }

        public void RemoveGrid()
        {
            if (_cells == null) return;
        
            foreach (var cell in _cells)
            {
                if (cell != null)
                    Destroy(cell.gameObject);
            }
        }

        private void SpawnCell(List<CellCharacteristic> _characteristics, int x, int y, float xPos, float yPos)
        {
            Cell cell = Instantiate(_cellPrefab, new Vector3(xPos + _spacing.x * xPos, yPos + _spacing.y * yPos), Quaternion.identity, _gridContainer);

            cell.transform.name = $"Cell {x} {y}";
        
            if (IsFirstGrid)
                BounceCell(cell, x, y);
        
            var randomCell = _characteristics[Random.Range(0, _characteristics.Count)];
            cell.SetLetter(randomCell);
            _characteristics.Remove(randomCell);
                
            cell.name = $"Cell {x}, {y}: {randomCell.letter}";
            _cells[x, y] = cell;
        }

        private void BounceCell(Cell cell, float x, float y)
        {
            cell.transform.localScale = Vector3.zero;
            cell.transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBounce)
                .SetDelay(x * 0.1f + y * 0.1f)
                .SetLink(cell.gameObject);
        }
    }
}
