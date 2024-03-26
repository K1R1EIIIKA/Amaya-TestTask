using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelTask : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private GridSpawner _gridSpawner;
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private CellConfig[] configs;
    [FormerlySerializedAs("_endGame")] [SerializeField] private EndGameLogic endGameLogic;

    private List<CellCharacteristic> _characteristics;
    private List<CellCharacteristic> _completedCharacteristics = new List<CellCharacteristic>();
    [NonSerialized] public Letter TaskLetter;
    [NonSerialized] public int CurrentLevel;

    private void Start()
    {
        _characteristics = configs.SelectMany(config => config.Cells).ToList();
        GenerateTask(_levelConfig.GridSizes[CurrentLevel]);
    }

    private void ResetCharacteristics()
    {
        _characteristics = configs.SelectMany(config => config.Cells).ToList();
        _characteristics = _characteristics.Except(_completedCharacteristics).ToList();
        
        print(string.Join(", ", _characteristics.Select(characteristic => characteristic.letter).ToArray()));
        print(_characteristics.Count + " " + _completedCharacteristics.Count);
    }

    public void GenerateTask(Vector2Int gridSize)
    {
        var cells = _gridSpawner.CreateGrid(gridSize, _characteristics);

        var randomCell = cells[Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)];
        TaskLetter = randomCell.CellLetter;
        string letterString = randomCell.CellName;
        
        string task = "Find " + letterString;
        _taskText.text = task;
        
        if (!_gridSpawner.IsFirstGrid) return;
        
        _taskText.DOFade(0, 0).SetLink(gameObject);
        _taskText.DOFade(1, 2).SetLink(gameObject);
    }

    public void CompleteTask()
    {
        if (endGameLogic.IsEndGame(CurrentLevel))
        {
            endGameLogic.EndGame();
            return;
        }
        CurrentLevel = (CurrentLevel + 1) % _levelConfig.GridSizes.Length;
        
        _gridSpawner.RemoveGrid();

        _characteristics = configs.SelectMany(config => config.Cells).ToList();

        _completedCharacteristics.Add(_characteristics.Find(characteristic => characteristic.letter == TaskLetter));
        ResetCharacteristics();
        print(_characteristics.Count);

        GenerateTask(_levelConfig.GridSizes[CurrentLevel]);
    }
    
    public void ResetTask()
    {
        CurrentLevel = 0;
        _gridSpawner.RemoveGrid();
        
        _completedCharacteristics.Clear();
        ResetCharacteristics();
        _gridSpawner.IsFirstGrid = true;
        
        GenerateTask(_levelConfig.GridSizes[CurrentLevel]);
    }
}