using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : Singleton<GameFlowManager>
{
    [SerializeField] float DelayBeforeLoadingNextScene = 1;

    public bool GameIsEnding { get; private set; }

    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.AddListener<PlayerWinEvent>(OnPlayerWin);
        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<PlayerWinEvent>(OnPlayerWin);
        EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsEnding) {
            if (ScreenFade.Instance.NormalizedTime >= 1) {
                SceneManager.LoadScene(0);
            }
        }
    }

    void EndGame(bool win) {
        GameIsEnding = true;

        if (win) {
            // no time for a cutscene lol
        } else {
            ScreenFade.Instance.FadeIn(2.5f, 1);
        }
    }

    void OnPlayerWin(PlayerWinEvent evt) => EndGame(true);
    void OnPlayerDeath(PlayerDeathEvent evt) => EndGame(false);
}
