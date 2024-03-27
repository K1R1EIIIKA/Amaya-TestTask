using System;
using System.Collections.Generic;
using DG.Tweening;
using Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellLogic
{
    public class GridSpawner : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Vector2 _spacing;
        [SerializeField] private Transform _gridContainer;
        
        [Header("Dependencies")]
        [SerializeField] ElementsAnimator _elementsAnimator;
        
        [NonSerialized] public bool IsFirstGrid = true;
        
        private Cell[,] _cells;

        public Cell[,] CreateGrid(Vector2Int gridSize, List<CellCharacteristic> characteristics)
        {
            _cells = new Cell[gridSize.x, gridSize.y];
            var centerPoint = GetCenterPoint(gridSize);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (characteristics.Count == 0)
                        break;

                    SpawnCell(characteristics, x, y, x - centerPoint.x, y - centerPoint.y);
                }
            }

            return _cells;
        }

        private static Vector2 GetCenterPoint(Vector2Int gridSize)
        {
            Vector2 centerPoint = new Vector2(gridSize.x / 2, gridSize.y / 2);

            if (gridSize.x % 2 == 0)
                centerPoint.x -= 0.5f;
            if (gridSize.y % 2 == 0)
                centerPoint.y -= 0.5f;
            return centerPoint;
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

        private void SpawnCell(List<CellCharacteristic> characteristics, int x, int y, float xPos, float yPos)
        {
            Vector3 cellPosition = new Vector3(xPos + _spacing.x * xPos, yPos + _spacing.y * yPos, 0);
            Cell cell = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, _gridContainer);

            if (IsFirstGrid)
               _elementsAnimator.BounceAppearingCell(cell, x, y);

            SetCellParams(characteristics, x, y, cell);
        }

        private void SetCellParams(List<CellCharacteristic> characteristics, int x, int y, Cell cell)
        {
            cell.transform.name = $"Cell {x} {y}";
            
            var randomCell = characteristics[Random.Range(0, characteristics.Count)];
            cell.SetLetter(randomCell);
            characteristics.Remove(randomCell);

            cell.name = $"Cell {x}, {y}: {randomCell.letter}";
            _cells[x, y] = cell;
        }
    }
}