using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : Menu
{
    public GameObject menuRoot;

    bool canPause;

    private void OnEnable()
    {
        EventManager.AddListener<GameCompletedEvent>(GameEnded);
    }

    private void OnDisable() {
        EventManager.RemoveListener<GameCompletedEvent>(GameEnded);
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        SetPauseMenuActivation(false);
        canPause = true;
    }

    // Update is called once per frame
    void Update() {
        if (!canPause) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame) {

            if (OnMenuEscape(MenuType.Settings, MenuType.Main, GameOption.Get<GraphicOption>().Apply))
                return;

            if (OnMenuEscape(MenuType.MessageBeforeQuiting, MenuType.Main))
                return;

            SetPauseMenuActivation(!menuRoot.activeSelf);
        }
    }

    void SetPauseMenuActivation(bool active) {
        menuRoot.SetActive(active);

        Time.timeScale = menuRoot.activeSelf ? 0 : 1;

        GamePauseEvent evt = Events.GamePauseEvent;
        evt.paused = menuRoot.activeSelf;
        EventManager.Broadcast(evt);
    }

    public void ClosePauseMenu() {
        SetPauseMenuActivation(false);
    }

    public void Quit() {
        canPause = false;

        menuRoot.SetActive(false);

        Time.timeScale = 1;

        EventManager.Broadcast(Events.GameQuitEvent);
    }

    void GameEnded(GameCompletedEvent evt) {
        canPause = false;
    }
}