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
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 spacing;
    [SerializeField] private CellConfig[] configs;

    [SerializeField] private Transform gridContainer;
    
    private List<CellCharacteristic> _characteristics = new();
    public List<CellCharacteristic> Characteristics => _characteristics;
    
    private Cell[,] cells;
    
    private void Start()
    {
        CreateGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveGrid();
            CreateGrid();
        }
    }

    public void CreateGrid()
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

                SpawnCell(x, y, x - centerPoint.x, y - centerPoint.y);
            }
        }
    }

    public void RemoveGrid()
    {
        if (cells == null) return;
        
        foreach (var cell in cells)
        {
            if (cell != null)
                Destroy(cell.gameObject);
        }
            
        _characteristics = configs.SelectMany(config => config.Cells).ToList();
    }

    private void SpawnCell(int x, int y, float xPos, float yPos)
    {
        Cell cell = Instantiate(cellPrefab, new Vector3(xPos + spacing.x * xPos, yPos + spacing.y * yPos), Quaternion.identity, gridContainer);
        cell.transform.name = $"Cell {x} {y}";
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
        cell.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).SetDelay(i * 0.1f + i1 * 0.1f);
    }

    public Cell GetCell(Vector2Int coordinates)
    {
        return cells[coordinates.x, coordinates.y];
    }
    
    public Vector2Int GetGridSize()
    {
        return gridSize;
    }
}
