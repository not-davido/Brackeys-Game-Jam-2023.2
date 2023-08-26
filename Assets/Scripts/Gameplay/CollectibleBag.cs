using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBag : MonoBehaviour
{
    public int CollectibleCount { get; private set; }

    private void OnEnable()
    {
        EventManager.AddListener<CollectiblePickUpEvent>(OnPickUp);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<CollectiblePickUpEvent>(OnPickUp);

    }

    void OnPickUp(CollectiblePickUpEvent evt) {
        CollectibleCount++;
    }
}
