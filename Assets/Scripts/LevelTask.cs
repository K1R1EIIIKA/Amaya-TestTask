using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<CellCharacteristic> _characteristics;
    private List<CellCharacteristic> _completedCharacteristics = new List<CellCharacteristic>();
    public Letter taskLetter;

    private int _currentLevel = 0;

    private void Start()
    {
        _characteristics = configs.SelectMany(config => config.Cells).ToList();
        GenerateTask(_levelConfig.GridSizes[_currentLevel]);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            _currentLevel = (_currentLevel + 1) % _levelConfig.GridSizes.Length;
            _gridSpawner.RemoveGrid();
            ResetCharacteristics();
            GenerateTask(_levelConfig.GridSizes[_currentLevel]);
        }

        else if (Input.GetKeyDown(KeyCode.R))
        {
            _gridSpawner.RemoveGrid();
            ResetCharacteristics();
            GenerateTask(_levelConfig.GridSizes[_currentLevel]);
        }
    }

    private void ResetCharacteristics()
    {
        _characteristics = configs.SelectMany(config => config.Cells).ToList();
        _characteristics = _characteristics.Except(_completedCharacteristics).ToList();
        // print(string.Join(", ", _completedCharacteristics.Select(characteristic => characteristic.letter).ToArray()));
        print(string.Join(", ", _characteristics.Select(characteristic => characteristic.letter).ToArray()));
        print(_characteristics.Count + " " + _completedCharacteristics.Count);
    }

    private void GenerateTask(Vector2Int gridSize)
    {
        var cells = _gridSpawner.CreateGrid(gridSize, _characteristics);

        taskLetter = cells[Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)].CellLetter;

        string task = "Find " + taskLetter;
        _taskText.text = task;
    }

    public void CompleteTask()
    {
        _currentLevel = (_currentLevel + 1) % _levelConfig.GridSizes.Length;
        _gridSpawner.RemoveGrid();

        _characteristics = configs.SelectMany(config => config.Cells).ToList();
//TODO:        all characteristics 
        _completedCharacteristics.Add(_characteristics.Find(characteristic => characteristic.letter == taskLetter));
        ResetCharacteristics();
        print(_characteristics.Count);

        GenerateTask(_levelConfig.GridSizes[_currentLevel]);
    }
}