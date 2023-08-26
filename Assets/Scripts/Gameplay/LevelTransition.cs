using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] Level NextLevel;
    [Tooltip("If enabled, will position the player at the starting point of the level; otherwise the ending position. If ending position does not exist, will use starting position instead.")]
    [SerializeField] bool StartingPoint = true;

    bool levelIsTransitioningIn;
    bool levelIsTransitioningOut;
    bool enteredNextLevel;

    private void Update()
    {
        var fadeNormalizedTime = ScreenFade.Instance.NormalizedTime;

        if (levelIsTransitioningIn && fadeNormalizedTime >= 1) {

            levelIsTransitioningIn = false;
            levelIsTransitioningOut = true;

            GameManager.Instance.SetPlayerActive(false);
        }

        if (levelIsTransitioningOut && fadeNormalizedTime < 1) {

            levelIsTransitioningOut = false;

            LevelManager.Instance.UpdateLevel();

            GameManager.Instance.SetPlayerActive(true);

            LevelTransitionEvent evt = Events.LevelTransitionEvent;
            evt.isTransitioningOut = true;
            evt.newPosition = StartingPoint ? NextLevel.StartingPosition : NextLevel.EndingPosition;

            EventManager.Broadcast(evt);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enteredNextLevel) return;

        if (collision.TryGetComponent(out Player player)) {
            if (player.TookDamage) return;

            LevelManager.Instance.NextLevel(NextLevel);

            ScreenFade.Instance.FadeInAndOut(0.5f, 0.5f, 0.5f);

            levelIsTransitioningIn = true;
            levelIsTransitioningOut = false;
            enteredNextLevel = true;
        }
    }

    private void OnDisable()
    {
        enteredNextLevel = false;
    }
}
