using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject player_canvas, main_menu_canvas, options_canvas;
    private int options_index = 0;

    public void PlayGame()
    {
        main_menu_canvas.SetActive(false);
        player_canvas.SetActive(true);
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
