using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool gameIsStarting;

    private void Start() {
        ScreenFade.Instance.FadeOut(1);
    }

    private void Update()
    {
        if (gameIsStarting && ScreenFade.Instance.NormalizedTime >= 1) {
            SceneManager.LoadScene(1);
        }
    }

    public void StartGame() {
        ScreenFade.Instance.FadeIn(1);
        gameIsStarting = true;
    }
}
