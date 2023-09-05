using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInstance : MonoBehaviour
{
    [field:SerializeField]
    public string MenuName { get; private set; }

    [field:SerializeField]
    public MenuType MenuType { get; private set; }

    public bool IsOpen => gameObject.activeSelf;

    public void Open() {
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
