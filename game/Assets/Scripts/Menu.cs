using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Canvas menu;
    public Button newGameButton;
    public Button resumeButton;
    public Button exitButton;

    void Start()
    {
        //Pobieramy obiekt menu
        menu = (Canvas)GetComponent<Canvas>();

        //Pobieramy wszystkie przyciski
        newGameButton = GameObject.Find("newGameButton").GetComponent<UnityEngine.UI.Button>();
        resumeButton = GameObject.Find("resumeGameButton").GetComponent<UnityEngine.UI.Button>();
        exitButton = GameObject.Find("exitButton").GetComponent<UnityEngine.UI.Button>();

        //Ukrywamy menu na starcie i chowamy kursor
        menu.enabled = !menu.enabled;
        Cursor.visible = menu.enabled;
    }

    void Update()
    {
        // Jeśli przyciśniemy escape pokazujemy / ukrywamy menu
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //Pokazujemy/ukrywamy menu i kursor
            menu.enabled = !menu.enabled; 
            Cursor.visible = menu.enabled;

            if (menu.enabled)
            {
                Cursor.lockState = CursorLockMode.Confined;//Odblokowujemy kursor myszy.
                Cursor.visible = true;//Pokazujemy kursor.

                newGameButton.enabled = true; 
                resumeButton.enabled = true;
                exitButton.enabled = true;

                Time.timeScale = 0; // Zatrzymujemy czas
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; //Blokujemy kursor myszy.
                Cursor.visible = false; //Ukrywamy kursor.

                Time.timeScale = 1; // Wznawiamy czas
            }

        }
    }

    // Po naciśnięciu Exit wychodzimy z aplikacji
    public void Exit()
    {
        Application.Quit();
    }

    // Po naciśnięciu start zaczynamy grę od początku
    public void StartGame()
    {
        Application.LoadLevel (0); 
        menu.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
    }

    // Po naciśnięciu Resume Game ukrywamy menu
    public void ResumeGame()
    {
        menu.enabled = !menu.enabled;
        Cursor.visible = menu.enabled;

        Time.timeScale = 1;
    }



}
