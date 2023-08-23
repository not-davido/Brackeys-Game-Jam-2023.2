using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTiling : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If camera is null, will try to locate Main Camera instead.")]
    private Camera cam;

    [SerializeField]
    private Vector2 parallaxEffect = new(0.25f, 0.25f);

    private float backgroundLength;
    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start() {
        backgroundLength = GetComponent<SpriteRenderer>().bounds.size.x;

        startPosition = transform.position;

        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate() {
        // How is this wrong tho?
        //transform.position = Vector2.Scale(cam.transform.position + (Vector3)startPosition, parallaxEffect);

        transform.position = Vector2.Scale(cam.transform.position, parallaxEffect);
        transform.position += (Vector3)startPosition;

        float temp = cam.transform.position.x * (1 - parallaxEffect.x);

        if (temp > startPosition.x + backgroundLength)
            startPosition.x += backgroundLength;
        else if (temp < startPosition.x - backgroundLength)
            startPosition.x -= backgroundLength;
    }
}
