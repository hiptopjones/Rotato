using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameSettings gameSettings;

    [SerializeField]
    private LevelSettings[] levelSettings;

    [SerializeField]
    private CodedBasicGameEventListener levelCompletedListener;

    private LevelManager levelManager;
    private int levelIndex;
    private bool isLevelActive;

    private void OnEnable()
    {
        levelCompletedListener.OnEnable(OnLevelCompleted);
    }

    private void OnDisable()
    {
        levelCompletedListener.OnDisable();
    }

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(LevelManager)}");
        }
    }

    private void Update()
    {
        // TODO: Keep track of time per level
        // TODO: Manage any pre-game / pre-level cut-scenes or effects

        if (!isLevelActive)
        {
            isLevelActive = true;
            PlayNextLevel();
        }
    }

    private void OnLevelCompleted()
    {
        PlayNextLevel();
    }

    private void PlayNextLevel()
    {
        // TODO: If false, end game
        if (levelIndex < levelSettings.Length)
        {
            levelManager.PlayLevel(levelSettings[levelIndex++]);
        }
    }
}
