using System;
using System.Collections.Generic;
using System.Linq;
using CellLogic;
using Configs;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Logic
{
    public class LevelTask : MonoBehaviour
    {
        [NonSerialized] private Letter _taskLetter;
        [NonSerialized] private int _currentLevel;
        
        public Letter TaskLetter => _taskLetter;
        public int CurrentLevel => _currentLevel;

        [Header("Properties")] 
        [SerializeField] private TextMeshProUGUI _taskText;

        [SerializeField] private CellConfig[] _configs;

        [Header("Dependencies")] 
        [SerializeField] private GridSpawner _gridSpawner;

        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LoadingPanel _loading;
        [SerializeField] ElementsAnimator _elementsAnimator;

        [SerializeField] FinishGameLogic _finishGameLogic;

        private List<CellCharacteristic> _allCharacteristics = new();
        private List<CellCharacteristic> _possibleRandomCharacteristics = new();
        private readonly List<CellCharacteristic> _completedCharacteristics = new();
        private List<CellCharacteristic> _characteristics;

        private readonly UnityEvent _onLoadComplete = new();
        
        public void LevelComplete()
        {
            _currentLevel++;
        }

        public void LevelReset()
        {
            _currentLevel = 0;
        }

        private void Start()
        {
            _allCharacteristics = _configs.SelectMany(config => config.Cells).ToList();
            ResetCharacteristics();

            GenerateTask(_levelConfig.GridSizes[_currentLevel]);

            _onLoadComplete.AddListener(() => _gridSpawner.RemoveGrid());
            _onLoadComplete.AddListener(() => GenerateTask(_levelConfig.GridSizes[_currentLevel]));
        }

        private void ResetCharacteristics()
        {
            _characteristics = ChooseCharacteristics();
            _possibleRandomCharacteristics = _allCharacteristics.Except(_completedCharacteristics).ToList();

            Debug.Log("Available characters: " + 
                      string.Join(", ", _possibleRandomCharacteristics.Select(characteristic => characteristic.letter)));
        }

        private void GenerateTask(Vector2Int gridSize)
        {
            if (_possibleRandomCharacteristics.Count == 0)
            {
                _finishGameLogic.FinishGame();
                return;
            }

            var randCell = GetRandomCell(gridSize);

            if (!_characteristics.Contains(randCell))
            {
                _characteristics.Remove(_characteristics[Random.Range(0, _characteristics.Count)]);
                _characteristics.Add(randCell);
            }

            _gridSpawner.CreateGrid(gridSize, _characteristics);

            if (_gridSpawner.IsFirstGrid)
            {
                _elementsAnimator.FadeText(_taskText, this);
                _gridSpawner.IsFirstGrid = false;
            }
        }

        private CellCharacteristic GetRandomCell(Vector2Int gridSize)
        {
            var searchList = _possibleRandomCharacteristics
                .Where(characteristic => _characteristics.Contains(characteristic)).ToList();
            
            CellCharacteristic randCell = searchList[Random.Range(0, searchList.Count)];

            _taskLetter = randCell.letter;

            _characteristics = _characteristics.OrderBy(_ => Random.value).Take(gridSize.x * gridSize.y).ToList();
            _taskText.text = "Find " + randCell.letterName;

            return randCell;
        }

        public void CompleteTask()
        {
            _completedCharacteristics.Add(
                _allCharacteristics.Find(characteristic => characteristic.letter == _taskLetter));

            _gridSpawner.RemoveGrid();
            ResetCharacteristics();

            GenerateTask(_levelConfig.GridSizes[_currentLevel]);
        }

        private List<CellCharacteristic> ChooseCharacteristics()
        {
            return _configs[Random.Range(0, _configs.Length)].Cells.ToList();
        }

        public void ResetTask()
        {
            _completedCharacteristics.Add(
                _allCharacteristics.Find(characteristic => characteristic.letter == _taskLetter));
            
            _gridSpawner.IsFirstGrid = true;
            _currentLevel = 0;

            ResetCharacteristics();

            _loading.LoadPanel(_onLoadComplete);
        }
    }
}