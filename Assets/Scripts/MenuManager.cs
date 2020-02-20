using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   
    public void RestartLevel()
    {
        NewManager.instance.Save();
        SceneManager.LoadScene(0);
    }
    public void ActivateSubMenu(GameObject subMenu)
    {
      subMenu.SetActive(!subMenu.activeSelf);
    }
}
