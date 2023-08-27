using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightCaveTransition : MonoBehaviour
{
    [SerializeField] Light2D GlobalLight;
    [SerializeField] Color CaveColor;
    [SerializeField] AudioClip CaveAmbience;

    bool entered;

    private void Update()
    {
        if (entered && ScreenFade.Instance.NormalizedTime >= 1) {
            UpdateColor();

            var caveAmbience = new GameObject("Cave Ambience");
            var audio = caveAmbience.AddComponent<AudioSource>();
            audio.clip = CaveAmbience;
            audio.volume = 0.3f;
            audio.loop = true;
            audio.Play();

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (entered) return;

        if (collision.TryGetComponent(out Player _)) {
            entered = true;
        }
    }

    public void UpdateColor() {
        GlobalLight.color = CaveColor;
    }
}
