using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DropdownResolution : Dropdown
{
    protected override void InitializeOptions() {
        int i = 0;
        int selected = 0;

        foreach (var res in Screen.resolutions.OrderByDescending(o => o.width)) {
            string option = $"{res.width}x{res.height}";

            if (!dropdown.options.Any(o => o.text == option)) {
                dropdown.options.Add(new TMP_Dropdown.OptionData(option));

                var graphicOption = GameOption.Get<GraphicOption>();
                var graphicResolution = new Resolution() {
                    width = graphicOption.FullscreenWidth,
                    height = graphicOption.FullscreenHeight
                };

                if (GraphicOption.IsResolutionEqual(res, graphicResolution)) {
                    selected = i;
                }
            }

            i++;
        }

        dropdown.SetValueWithoutNotify(selected);
    }

    protected override void UpdateOptions(int value) {
        string option = dropdown.options[value].text;
        string[] res = option.Split('x');

        GameOption.Get<GraphicOption>().FullscreenWidth = int.Parse(res[0]);
        GameOption.Get<GraphicOption>().FullscreenHeight = int.Parse(res[1]);
    }
}
