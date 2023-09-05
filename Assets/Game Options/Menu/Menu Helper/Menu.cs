using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MenuInstanceManager))]
public class Menu : MonoBehaviour
{
    MenuInstanceManager menuManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        menuManager = GetComponent<MenuInstanceManager>();
        menuManager.Open(MenuType.Main);
    }

    /// <summary>
    /// Checks if <paramref name="currentMenu"/> is active to open <paramref name="menuEscapingTo"/>. If not, this will return <see langword="false"/>
    /// </summary>
    /// <param name="currentMenu">The current menu.</param>
    /// <param name="menuEscapingTo">The next menu to open.</param>
    /// <param name="action">Optional action to apply.</param>
    /// <returns>Returns <see langword="true"/> if the <paramref name="currentMenu"/> is active; otherwise <see langword="false"/>.</returns>
    protected bool OnMenuEscape(MenuType currentMenu, MenuType menuEscapingTo, Action action = null) {
        if (menuManager.IsMenuOpen(currentMenu)) {
            action?.Invoke();
            menuManager.Open(menuEscapingTo);
            return true;
        }

        return false;
    }
}
