using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Menu;
    public GameObject HelpMenu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit() {
        Application.Quit();
    }

    public void Back() {
        HelpMenu.SetActive(false);
        Menu.SetActive(true);
    }

    public void Help() {
        Menu.SetActive(false);
        HelpMenu.SetActive(true);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }
}
