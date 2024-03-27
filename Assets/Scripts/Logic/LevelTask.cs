using System;
using System.Collections.Generic;
using System.Linq;
using CellLogic;
using Configs;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Logic
{
    public class LevelTask : MonoBehaviour
    {
        [NonSerialized] public Letter TaskLetter;
        [NonSerialized] public int CurrentLevel;
        
        [Header("Properties")]
        [SerializeField] private TextMeshProUGUI _taskText;
        [SerializeField] private CellConfig[] _configs;
        
        [Header("Dependencies")]
        [SerializeField] private GridSpawner _gridSpawner;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LoadingPanel _loading;
        [SerializeField] ElementsAnimator _elementsAnimator;
        [SerializeField] EndGameLogic _endGameLogic;

        private List<CellCharacteristic> _allCharacteristics = new();
        private List<CellCharacteristic> _possibleRandomCharacteristics = new();
        private readonly List<CellCharacteristic> _completedCharacteristics = new();
        private List<CellCharacteristic> _characteristics;
        
        private readonly UnityEvent _onLoadComplete = new();
        

        private void Start()
        {
            _allCharacteristics = _configs.SelectMany(config => config.Cells).ToList();
            ResetCharacteristics();
            
            GenerateTask(_levelConfig.GridSizes[CurrentLevel]);
        
            _onLoadComplete.AddListener(() => _gridSpawner.RemoveGrid());
            _onLoadComplete.AddListener(() => GenerateTask(_levelConfig.GridSizes[CurrentLevel]));
        }

        private void ResetCharacteristics()
        {
            _characteristics = ChooseCharacteristics();
            _possibleRandomCharacteristics = _allCharacteristics.Except(_completedCharacteristics).ToList();
            print(string.Join(", ", _possibleRandomCharacteristics.Select(characteristic => characteristic.letter)));
        }

        private void GenerateTask(Vector2Int gridSize)
        {
            if (_possibleRandomCharacteristics.Count == 0)
            {
                _endGameLogic.FinishGame();
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
            CellCharacteristic randCell = _possibleRandomCharacteristics[Random.Range(0, _possibleRandomCharacteristics.Count)];
            
            TaskLetter = randCell.letter;
            
            _characteristics = _characteristics.OrderBy(_ => Random.value).Take(gridSize.x * gridSize.y).ToList();
            _taskText.text = "Find " + randCell.letterName;
            
            return randCell;
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
}