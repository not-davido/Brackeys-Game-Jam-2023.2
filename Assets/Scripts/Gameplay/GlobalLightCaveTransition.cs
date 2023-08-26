using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightCaveTransition : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    [SerializeField] Color CaveColor;

    bool entered;

    private void Update()
    {
        if (entered && ScreenFade.Instance.NormalizedTime >= 1) {
            UpdateColor();
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
        globalLight.color = CaveColor;
    }
}
