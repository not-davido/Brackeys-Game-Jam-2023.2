using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointInitializer : MonoBehaviour
{
    public bool Initialize;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode load) {
        EnableAllCheckpoints(Initialize);
    }

    void EnableAllCheckpoints(bool value) {
        var checkpoints = FindObjectsByType<LevelCheckpoint>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var point in checkpoints) {
            point.gameObject.SetActive(value);
        }
    }
}
