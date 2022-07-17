using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject main_menu_canvas, options_canvas;
    private int options_index = 0;
    private int scene_target = 1;

    public void PlayGame()
    {
        SceneManager.LoadScene(scene_target);
    }
    
    public void Options()
    {
        if(options_index == 1)
        {
            main_menu_canvas.SetActive(true);
            options_canvas.SetActive(false);
            options_index = 0;
            return;
        }
        main_menu_canvas.SetActive(false);
        options_canvas.SetActive(true);
        options_index = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
