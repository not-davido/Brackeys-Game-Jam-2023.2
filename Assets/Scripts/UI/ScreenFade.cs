using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : Singleton<ScreenFade>
{
    [SerializeField] CanvasGroup FadeCanvas;

    //[Header("Level transitioning")]
    //[SerializeField] float LevelTransitionDuration = 0.5f;
    //[SerializeField] float DelayBeforeNextLevel = 0.3f;

    //[Header("Player Damage")]
    //[SerializeField] float PlayerDamageFadeDuration = 0.5f;

    float transitionTimer;
    float delayTimer;
    //bool levelIsTransitioningIn;
    //bool levelIsTransitioningOut;

    float transitionIn;
    float delayBetween;
    float transitionOut;
    bool fadingIn;
    bool fadingOut;

    public float NormalizedTime { get; private set; }
    public bool FadeInFinished { get; private set; }

    /// <summary>
    /// Checks if the player can be visible or not during a fade transition.
    /// </summary>
    //bool playerIsActive;
    //bool playerTookDamage;

    //private void Awake()
    //{
    //    EventManager.AddListener<GameTransitionEvent>(OnLevelTransition);
    //    EventManager.AddListener<PlayerDamageEvent>(OnPlayerDamageTransition);
    //}

    // Update is called once per frame
    void Update()
    {
        if (fadingIn) {
            NormalizedTime = (Time.time - transitionTimer) / transitionIn;
            FadeCanvas.alpha = NormalizedTime;

            if (NormalizedTime >= 1) {
                fadingIn = false;
                FadeInFinished = true;
                delayTimer = Time.time;
            }
        }

        if (FadeInFinished && Time.time > delayTimer + delayBetween) {
            fadingOut = true;
            FadeInFinished = false;
            transitionTimer = Time.time;
        }

        if (fadingOut) {
            NormalizedTime = 1 - (Time.time - transitionTimer) / transitionOut;
            FadeCanvas.alpha = NormalizedTime;

            if (NormalizedTime <= 0) {
                fadingOut = false;
            }
        }



        //if (levelIsTransitioningIn) {
        //    float timeRatio = (Time.time - transitionTimer) / LevelTransitionDuration;
        //    FadeCanvas.alpha = timeRatio;

        //    if (timeRatio >= 1) {
        //        levelIsTransitioningIn = false;
        //        levelIsTransitioningOut = true;
        //        transitionTimer = Time.time + DelayBeforeNextLevel;
        //        GameManager.Instance.SetPlayerActive(false);
        //        playerIsActive = false;
        //    }
        //}

        //if (levelIsTransitioningOut) {
        //    float timeRatio = 1 - (Time.time - transitionTimer) / LevelTransitionDuration;
        //    FadeCanvas.alpha = timeRatio;

        //    if (timeRatio <= 1 && !playerIsActive) {
        //        var player = GameManager.Instance.GetPlayer();

        //        player.gameObject.SetActive(true);
        //        player.ResetVelocity();
        //        player.ResetMove();

        //        LevelManager.Instance.UpdateLevel();
        //        playerIsActive = true;
        //    }

        //    if (timeRatio <= 0) {
        //        levelIsTransitioningOut = false;

        //        GameTransitionEvent evt = Events.GameTransitionEvent;
        //        evt.isTransitioning = false;
        //        EventManager.Broadcast(evt);
        //    }
        //}

        //if (playerTookDamage) {
        //    float timeRatio = (Time.time - transitionTimer) / PlayerDamageFadeDuration;
        //    FadeCanvas.alpha = timeRatio;

        //    if (timeRatio >= 1) {
        //        GameManager.Instance.SetPlayerPositionAfterDamage();
        //        playerTookDamage = false;
        //    }
        //}
    }

    //void OnLevelTransition(GameTransitionEvent evt) {
    //    levelIsTransitioningIn = evt.isTransitioning;
    //    transitionTimer = Time.time;
    //}

    //void OnPlayerDamageTransition(PlayerDamageEvent evt) {
    //    playerTookDamage = true;
    //    transitionTimer = Time.time;
    //}

    //private void OnDisable()
    //{
    //    EventManager.RemoveListener<GameTransitionEvent>(OnLevelTransition);
    //    EventManager.RemoveListener<PlayerDamageEvent>(OnPlayerDamageTransition);
    //}

    /// <summary>
    /// Fade screen transition. This can be overriden by another script so use with caution.
    /// </summary>
    /// <param name="durationIn"></param>
    /// <param name="durationOut"></param>
    /// <param name="holdBetween"></param>
    public void Fade(float durationIn, float durationOut, float holdBetween = 0) {
        fadingIn = true;

        transitionTimer = Time.time;
        transitionIn = durationIn;
        transitionOut = durationOut;
        delayBetween = holdBetween;
    }
}
