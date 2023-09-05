using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInstanceManager : MonoBehaviour
{
    [SerializeField] MenuInstance[] menus;

    // For editor
    public void Open(MenuInstance menu) {
        foreach (var currentMenu in menus) {
            if (currentMenu == menu)
                currentMenu.Open();
            else
                currentMenu.Close();
        }
    }

    public void Open(string menuName) {
        foreach (var currentMenu in menus) {
            if (currentMenu.MenuName == menuName)
                currentMenu.Open();
            else
                currentMenu.Close();
        }
    }

    public void Open(MenuType menuType) {    
        foreach (var currentMenu in menus) {
            if (currentMenu.MenuType == menuType)
                currentMenu.Open();
            else
                currentMenu.Close();
        }
    }

    public bool IsMenuOpen(MenuType menuType) {
        foreach (var currentMenu in menus) {
            if (currentMenu.MenuType == menuType) {
                if (currentMenu.IsOpen) {
                    return true;
                }
            }   
        }

        return false;
    }
}

public enum MenuType {
    None, Main, Settings, Plot, Credits, Instruction, MessageBeforeQuiting
}
