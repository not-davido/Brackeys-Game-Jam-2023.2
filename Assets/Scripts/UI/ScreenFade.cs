using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : Singleton<ScreenFade>
{
    [SerializeField] CanvasGroup FadeCanvas;

    float transitionIn;
    float delayBetween;
    float transitionOut;
    bool fadingIn;
    bool fadeInFinished;
    bool fadingOut;
    bool fadeInAndOut;

    float transitionTimer;
    float delayTimer;

    public float NormalizedTime { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (fadeInAndOut) {
            if (fadingIn) {
                NormalizedTime = (Time.time - transitionTimer) / transitionIn;
                FadeCanvas.alpha = NormalizedTime;

                if (NormalizedTime >= 1) {
                    fadeInFinished = true;
                    fadingIn = false;
                    delayTimer = Time.time;
                }
            }

            if (fadeInFinished && Time.time > delayTimer + delayBetween) {
                fadingOut = true;
                fadeInFinished = false;
                transitionTimer = Time.time;
            }

            if (fadingOut) {
                NormalizedTime = 1 - (Time.time - transitionTimer) / transitionOut;
                FadeCanvas.alpha = NormalizedTime;

                if (NormalizedTime <= 0) {
                    fadingOut = false;
                    fadeInAndOut = false;
                }
            }

            return;
        }

        if (fadingIn) {
            NormalizedTime = (Time.time - transitionTimer) / transitionIn;
            FadeCanvas.alpha = NormalizedTime;

            if (NormalizedTime >= 1) {
                fadingIn = false;
            }
        }

        if (fadingOut) {
            NormalizedTime = 1 - (Time.time - transitionTimer) / transitionOut;
            FadeCanvas.alpha = NormalizedTime;

            if (NormalizedTime <= 0) {
                fadingOut = false;
            }
        }

    }

    /// <summary>
    /// Fade screen transition. This can be overriden by another script so use with caution.
    /// </summary>
    /// <param name="durationIn"></param>
    /// <param name="durationOut"></param>
    /// <param name="holdBetween"></param>
    public void FadeInAndOut(float durationIn, float durationOut, float holdBetween = 0) {
        fadeInAndOut = true;
        // Reset values
        fadingIn = true;
        fadeInFinished = false;
        fadingOut = false;

        transitionTimer = Time.time;
        transitionIn = durationIn;
        transitionOut = durationOut;
        delayBetween = holdBetween;
    }

    public void FadeIn(float duration, float delayBeforeFade = 0) {
        fadeInAndOut = false;
        fadingIn = true;
        fadingOut = false;
        fadeInFinished = false;

        transitionTimer = Time.time + delayBeforeFade;

        transitionIn = duration;
    }

    public void FadeOut(float duration) {
        fadeInAndOut = false;
        fadingOut = true;
        fadingIn = false;
        fadeInFinished = false;

        transitionTimer = Time.time;

        transitionOut = duration;
    }
}
