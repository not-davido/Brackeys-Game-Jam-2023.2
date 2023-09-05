using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that determines if a button belongs in a certain platform.
/// Example: A quit game button shouldn't be on a WebGL build.
/// </summary>
public class PlatformDependentButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE
        gameObject.SetActive(true);
#elif UNITY_WEBGL
        gameObject.SetActive(false);
#endif
    }
}
