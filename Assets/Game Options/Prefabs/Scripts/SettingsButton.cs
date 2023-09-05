using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ButtonText;
    [SerializeField] GameObject Panel;

    Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PanelInteraction);
        ButtonText.text = Panel.activeSelf ? "Close Settings" : "Open Settings";
    }

    private void OnDisable() {
        button.onClick.RemoveListener(PanelInteraction);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Panel.activeSelf) {
            GameOption.Get<GraphicOption>().Apply();
            PanelInteraction();
        }
    }

    void PanelInteraction() {
        Panel.SetActive(!Panel.activeSelf);
        ButtonText.text = Panel.activeSelf ? "Close Settings" : "Open Settings";
    }
}
