using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//public class DropdownRefreshRate : Dropdown
//{
//    protected override void InitializeOptions() {
//        int i = 0;
//        int selected = 0;

//        foreach (var res in Screen.resolutions) {
//            if (res.width == GameOption.Get<GraphicOption>().Width &&
//                res.height == GameOption.Get<GraphicOption>().Height) {

//                if (!dropdown.options.Any(o => o.text == res.refreshRate.ToString()))
//                    dropdown.options.Add(new TMP_Dropdown.OptionData(res.refreshRate.ToString()));

//                if (GameOption.Get<GraphicOption>().RefreshRate == res.refreshRate)
//                    selected = i;

//                i++;
//            }
//        }

//        dropdown.SetValueWithoutNotify(selected);
//        dropdown.RefreshShownValue();
//    }

//    protected override void UpdateOptions(int value) {
//        GameOption.Get<GraphicOption>().RefreshRate = int.Parse(dropdown.options[value].text);
//    }

//    public void Init() => InitializeOptions();
//}
