using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : Singleton<GameFlowManager>
{
    [SerializeField] float DelayBeforeLoadingNextScene = 1;

    bool won;

    public bool GameIsEnding { get; private set; }
    public bool GameIsQuiting => GameManager.Instance.GameIsQuiting;

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
    void LateUpdate()
    {
        if (GameIsQuiting) {
            if (ScreenFade.Instance.NormalizedTime >= 1) {
                SceneManager.LoadScene(0);
            }
        }

        if (GameIsEnding) {
            if (ScreenFade.Instance.NormalizedTime >= 1) {
                if (won) {
                    GameOverMessage.Instance.Message("You did it! You are a true cave adventurer!", true);
                } else {
                    GameOverMessage.Instance.Message("You died!", false);
                }
            }
        }
    }

    void EndGame(bool win) {
        GameIsEnding = true;
        won = win;

        if (win) {
            ScreenFade.Instance.FadeIn(2.5f, 1, true);
        } else {
            ScreenFade.Instance.FadeIn(2.5f, 1, true);
        }
    }

    void OnPlayerWin(PlayerWinEvent evt) => EndGame(true);
    void OnPlayerDeath(PlayerDeathEvent evt) => EndGame(false);
}
