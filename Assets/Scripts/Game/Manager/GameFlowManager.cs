using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : Singleton<GameFlowManager>
{
    [SerializeField] float DelayBeforeLoadingNextScene = 1;

    float delayTimer;

    public bool GameIsEnding { get; private set; }

    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnDisable()
    {
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

        } else {
            ScreenFade.Instance.FadeIn(2.5f, 1);
        }
    }

    void OnPlayerDeath(PlayerDeathEvent evt) => EndGame(false);
}
