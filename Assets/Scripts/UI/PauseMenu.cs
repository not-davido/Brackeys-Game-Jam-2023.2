using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuRoot;
    public GameObject main;
    public GameObject messageBeforeQuiting;

    private bool canPause;

    private void Awake()
    {
        EventManager.AddListener<GameCompletedEvent>(GameEnded);
    }

    // Start is called before the first frame update
    void Start() {
        SetPauseMenuActivation(false);
        messageBeforeQuiting.SetActive(false);
        canPause = true;
    }

    // Update is called once per frame
    void Update() {
        if (!canPause) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame ||
            Keyboard.current.pKey.wasPressedThisFrame) {

            if (messageBeforeQuiting.activeSelf) {
                main.SetActive(true);
                messageBeforeQuiting.SetActive(false);
                return;
            }

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

    private void OnDisable()
    {
        EventManager.RemoveListener<GameCompletedEvent>(GameEnded);
    }
}