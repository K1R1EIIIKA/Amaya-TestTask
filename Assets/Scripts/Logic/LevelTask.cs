using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelTask : MonoBehaviour
{
    [NonSerialized] public Letter TaskLetter;
    [NonSerialized] public int CurrentLevel;
    
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private GridSpawner _gridSpawner;
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private CellConfig[] _configs;
    [SerializeField] private EndGameLogic _endGameLogic;
    [SerializeField] private LoadingPanel _loading;

    private List<CellCharacteristic> _characteristics;
    private List<CellCharacteristic> _completedCharacteristics = new();
    private readonly UnityEvent _onLoadComplete = new UnityEvent();
    private List<CellCharacteristic> _allCharacteristics = new();

    private void Start()
    {
        _characteristics = ChooseCharacteristics();
        _allCharacteristics = _configs.SelectMany(config => config.Cells).ToList();
        GenerateTask(_levelConfig.GridSizes[CurrentLevel]);
        
        _onLoadComplete.AddListener(() => _gridSpawner.RemoveGrid());
        _onLoadComplete.AddListener(() => GenerateTask(_levelConfig.GridSizes[CurrentLevel]));
    }

    private void ResetCharacteristics()
    {
        _characteristics = ChooseCharacteristics();
        print(string.Join(", ", _completedCharacteristics.Select(characteristic => characteristic.letter)));
        _characteristics = _characteristics.Except(_completedCharacteristics).ToList();
        print(string.Join(", ", _characteristics.Select(characteristic => characteristic.letter)));
    }

    public void GenerateTask(Vector2Int gridSize)
    {
        print(string.Join(", ", _characteristics.Select(characteristic => characteristic.letter)));
        var cells = _gridSpawner.CreateGrid(gridSize, _characteristics);

        var randomCell = GetRandomCell(gridSize, cells);
        
        TaskLetter = randomCell.CellLetter;
        string letterString = randomCell.CellName;
        
        string task = "Find " + letterString;
        _taskText.text = task;
        
        if (_gridSpawner.IsFirstGrid)
        {
            FadeText();
            _gridSpawner.IsFirstGrid = false;
        }
    }

    private Cell GetRandomCell(Vector2Int gridSize, Cell[,] cells)
    {
        while (true)
        {
            var randomX = Random.Range(0, gridSize.x);
            var randomY = Random.Range(0, gridSize.y);
            
            var randomCell = cells[randomX, randomY];
            if (randomCell != null)
                return randomCell;
        }
    }

    private void FadeText()
    {
        _taskText.DOFade(0, 0).SetLink(gameObject);
        _taskText.DOFade(1, 2).SetLink(gameObject);
    }

    public void CompleteTask()
    {
        _completedCharacteristics.Add(_allCharacteristics.Find(characteristic => characteristic.letter == TaskLetter));
        
        _gridSpawner.RemoveGrid();
        
        ResetCharacteristics();

        GenerateTask(_levelConfig.GridSizes[CurrentLevel]);
    }
    
    private List<CellCharacteristic> ChooseCharacteristics()
    {
        return _configs[Random.Range(0, _configs.Length)].Cells.ToList();
    }
    
    public void ResetTask()
    {
        _completedCharacteristics.Add(_allCharacteristics.Find(characteristic => characteristic.letter == TaskLetter));
        _gridSpawner.IsFirstGrid = true;
        CurrentLevel = 0;

        ResetCharacteristics();
        
        _loading.LoadPanel(_onLoadComplete);
    }
}