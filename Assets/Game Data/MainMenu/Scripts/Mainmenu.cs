using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] private Tween mainMenu;
    [SerializeField] private Tween creditMenu;

    private void Start()
    {
        mainMenu.BeginTween();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void CreditTween(bool Open)
    {
        if(Open)
        {
            creditMenu.BeginTween();
            mainMenu.ReturnTween();
        }
        else
        {
            mainMenu.BeginTween();
            creditMenu.ReturnTween();
        }
    }
}
