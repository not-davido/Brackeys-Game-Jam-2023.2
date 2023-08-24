using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] List<Level> Levels = new();
    [SerializeField] float PreventInputAfterLevelTransitionDuration = 1;

    [SerializeField] bool debug;
    [SerializeField] int startAtLevel = 0;
    [SerializeField] Transform spawn;

    Level currentLevel;
    float inputDisabledTimer;

    public bool InputIsDisabled => Time.time < inputDisabledTimer + PreventInputAfterLevelTransitionDuration;

    private void OnEnable()
    {
        EventManager.AddListener<LevelTransitionEvent>(OnLevelTransition);
    }

    private void OnDisable() {
        EventManager.RemoveListener<LevelTransitionEvent>(OnLevelTransition);
    }

    private void Start()
    {
        ScreenFade.Instance.FadeOut(0.7f);

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
    }

    /// <summary>
    /// Get the next level for transition. Use UpdateLevel() to update.
    /// </summary>
    /// <param name="level"></param>
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

    void OnLevelTransition(LevelTransitionEvent evt) {
        if (evt.isTransitioningOut)
            inputDisabledTimer = Time.time;
    }
}
