using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownQuality : Dropdown
{
    protected override void InitializeOptions()
    {
        foreach (var quality in QualitySettings.names) {
            dropdown.options.Add(new TMP_Dropdown.OptionData(quality));
        }

        dropdown.SetValueWithoutNotify(GameOption.Get<GraphicOption>().Quality);
    }

    protected override void UpdateOptions(int value)
    {
        GameOption.Get<GraphicOption>().Quality = value;
    }
}
