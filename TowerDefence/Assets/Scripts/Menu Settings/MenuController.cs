using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController menuController;
    //0:loadMenu, 1:saveMenu, 2:controlMenu, 3:graphicsMenu, 4:soundMenu
    [SerializeField] GameObject[] menus;
    [SerializeField] GameObject menuContainer;
    [SerializeField] GameObject startScreen;

    private void Awake()
    {
        if (menuController == null)
            menuController = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            menuContainer.SetActive(!menuContainer.active);
            startScreen.SetActive(!startScreen.active);
        }
    }

    public void OpenMenu(int menuID)
    {
        Debug.Log("Activating menu " + menuID);
        menus[menuID].SetActive(true);
        for (int i = 0; i < menus.Length; i++)
        {
            if (i != menuID)
            {
                Debug.Log("Deactivating menu " + i);
                menus[i].SetActive(false);
            }
        }
    }
}
