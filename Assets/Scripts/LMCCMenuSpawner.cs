using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LMCCMenuSpawner : MonoBehaviour
{
    public static LMCCMenuSpawner Main { get; private set; }

    public List<LMCCMenuBox> MenuBoxes { get { return menuBoxes; } }
    public List<LMCCMenu> Menus { get { return menus; } }

    [SerializeField] private Transform menuSpawnLoc;
    [SerializeField] private Transform menuCacheLoc;
    [SerializeField] private List<LMCCMenuBox> menuBoxes;
    [SerializeField] private List<LMCCMenu> menus;

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Hide all menu boxes on start
        foreach (LMCCMenuBox box in menuBoxes)
        {
            box.Image.CrossFadeAlpha(0f, 0f, true);
        }
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

    public void DisplayMenuBoxes(bool display)
    {
        foreach (LMCCMenuBox box in menuBoxes)
        {
            if (display)
                box.Image.CrossFadeAlpha(1f, 0.1f, true);
            else
                box.Image.CrossFadeAlpha(0f, 0.1f, true);
        }
    }
}
