using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    bool gameIsStarting;

    protected override void Start() {
        base.Start();

        ScreenFade.Instance.FadeOut(1);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {

            if (OnMenuEscape(MenuType.Plot, MenuType.Main))
                return;

            if (OnMenuEscape(MenuType.Instruction, MenuType.Main))
                return;

            if (OnMenuEscape(MenuType.Settings, MenuType.Main, GameOption.Get<GraphicOption>().Apply))
                return;

            if (OnMenuEscape(MenuType.Credits, MenuType.Main))
                return;
        }

        if (gameIsStarting && ScreenFade.Instance.NormalizedTime >= 1) {
            SceneManager.LoadScene(1);
        }
    }

    public void StartGame() {
        ScreenFade.Instance.FadeIn(1);
        gameIsStarting = true;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
