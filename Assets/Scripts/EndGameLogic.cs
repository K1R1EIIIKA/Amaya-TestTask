using UnityEngine;

public class EndGameLogic : MonoBehaviour
{
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private LevelTask _levelTask;

    public bool IsEndGame(int currentLevel)
    {
        return currentLevel == _levelConfig.GridSizes.Length - 1;
    }
    
    public void EndGame()
    {
        _endGamePanel.SetActive(true);
    }
    
    public void RestartGame()
    {
        _endGamePanel.SetActive(false);
        _levelTask.CurrentLevel = 0;
        _levelTask.ResetTask();
    }
}