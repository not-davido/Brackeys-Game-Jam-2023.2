using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] List<Level> Levels = new();

    [SerializeField] bool debug;
    [SerializeField] int startAtLevel = 0;
    [SerializeField] Transform spawn;

    //Transform startPosition;
    //int currentLevel;
    Level currentLevel;

    private void Start()
    {
        if (debug) {
            if (startAtLevel >= Levels.Count || startAtLevel < 0) {
                Debug.LogWarning("Level number does not exist");
            } else {
                foreach (var level in Levels) {
                    level.gameObject.SetActive(false);
                }

                Levels[startAtLevel].gameObject.SetActive(true);
                GameManager.Instance.GetPlayer().transform.position = spawn.position;
                return;
            }
        }

        foreach (var level in Levels) {
            level.gameObject.SetActive(false);
        }

        Levels[0].gameObject.SetActive(true);

        //currentLevel = Levels[0].levelNumber;
    }

    //public void NextLevel(Level level, Transform startingPosition) {
    //    currentLevel = level.levelNumber;
    //    startPosition = startingPosition;
    //}

    public void NextLevel(Level level) {
        currentLevel = level;
    }

    /// <summary>
    /// Update to current level.
    /// </summary>
    public void UpdateLevel() {
        foreach (var level in Levels) {
            if (level == currentLevel) {
                level.gameObject.SetActive(true);
            } else {
                level.gameObject.SetActive(false);
            }
        }
    }
}
