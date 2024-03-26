using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Vector2 spacing;

    [SerializeField] private Transform gridContainer;
    
    private Cell[,] cells;
    private bool _isFirstGrid = true;

    public Cell[,] CreateGrid(Vector2Int gridSize, List<CellCharacteristic> _characteristics)
    {
        cells = new Cell[gridSize.x, gridSize.y];
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
        
        // _isFirstGrid = false;
        
        return cells;
    }

    public void RemoveGrid()
    {
        if (cells == null) return;
        
        foreach (var cell in cells)
        {
            if (cell != null)
                Destroy(cell.gameObject);
        }
    }

    private void SpawnCell(List<CellCharacteristic> _characteristics, int x, int y, float xPos, float yPos)
    {
        Cell cell = Instantiate(cellPrefab, new Vector3(xPos + spacing.x * xPos, yPos + spacing.y * yPos), Quaternion.identity, gridContainer);

        cell.transform.name = $"Cell {x} {y}";
        
        if (_isFirstGrid)
            BounceCell(cell, x, y);
        
        var randomCell = _characteristics[Random.Range(0, _characteristics.Count)];
        cell.SetLetter(randomCell);
        _characteristics.Remove(randomCell);
                
        cell.name = $"Cell {x}, {y}: {randomCell.letter}";
        cells[x, y] = cell;
    }

    private void BounceCell(Cell cell, float i, float i1)
    {
        cell.transform.localScale = Vector3.zero;
        cell.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBounce)
            .SetDelay(i * 0.1f + i1 * 0.1f)
            .SetLink(cell.gameObject);
    }
}
