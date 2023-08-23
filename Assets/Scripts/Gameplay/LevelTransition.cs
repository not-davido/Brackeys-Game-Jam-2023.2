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

    private void Update()
    {
        var fadeNormalizedTime = ScreenFade.Instance.NormalizedTime;

        if (levelIsTransitioningIn && fadeNormalizedTime >= 1) {

            GameManager.Instance.SetPlayerActive(false);

            levelIsTransitioningIn = false;
            levelIsTransitioningOut = true;
        }

        if (levelIsTransitioningOut && fadeNormalizedTime < 1) {
            var player = GameManager.Instance.GetPlayer();

            player.gameObject.SetActive(true);
            player.ResetVelocity();
            player.ResetMove();
            player.transform.position = StartingPoint ? NextLevel.StartingPosition : NextLevel.EndingPosition;

            LevelManager.Instance.UpdateLevel();

            levelIsTransitioningOut = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out KinematicPlayerController2D _)) {
            LevelManager.Instance.NextLevel(NextLevel);

            ScreenFade.Instance.Fade(0.5f, 0.5f, 0.5f);

            levelIsTransitioningIn = true;
            levelIsTransitioningOut = false;
            //GameTransitionEvent evt = Events.GameTransitionEvent;
            //evt.isTransitioning = true;
            //EventManager.Broadcast(evt);
        }
    }
}
