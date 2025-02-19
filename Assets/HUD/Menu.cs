using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string nomeCena;

    public void Jogar ()
    {
        SceneManager.LoadScene(nomeCena); 
    }

    public void Sair ()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
}
