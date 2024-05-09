using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LMCCMenuSpawner : MonoBehaviour
{
    public static LMCCMenuSpawner Main { get; private set; }

    [SerializeField] private Transform menuSpawnLoc;
    [SerializeField] private Transform menuCacheLoc;
    [SerializeField] private List<LMCCMenu> menus;

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    public LMCCMenu ToggleMenu(int menuIndex)
    {
        if (menus[menuIndex].gameObject.transform.parent == menuSpawnLoc)
            return DeactivateMenu(menuIndex);
        else if (menus[menuIndex].gameObject.transform.parent == menuCacheLoc)
            return ActivateMenu(menuIndex);
        else
        {
            Debug.Log("Menu is in an unknown state!");
            return null;
        }
    }

    public LMCCMenu ActivateMenu(int menuIndex)
    {
        if (menus[menuIndex].gameObject.transform.parent == menuSpawnLoc)
        {
            Debug.Log("Menu already active!");
            return null;
        }

        LMCCMenu menu = menus[menuIndex];
        menu.transform.SetParent(menuSpawnLoc);
        menu.transform.position = menuSpawnLoc.position;
        menu.Display(true);
        return menu;
    }

    public LMCCMenu DeactivateMenu(int menuIndex)
    {
        if (menus[menuIndex].gameObject.transform.parent == menuCacheLoc)
        {
            Debug.Log("Menu already inactive!");
            return null;
        }

        LMCCMenu menu = menus[menuIndex];
        menu.Display(false, () =>
        {
            menu.transform.SetParent(menuCacheLoc);
            menu.transform.position = menuCacheLoc.position;
        });
        return menu;
    }
}
