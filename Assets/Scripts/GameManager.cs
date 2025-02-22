using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Referencia al tablero
    public Board gameBoard;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private LevelMenuManager LevelMenuManager;
    
    void Start()
    {
        // Inicialización si es necesario
    }

    void Update()
    {
        // Actualización por frame si es necesario
    }
    
    public void Win()
    {
        LevelMenuManager.ShowWinPanel();
        Debug.Log("Has ganooo!");
    }
    
    // Método para manejar el final del juego
    public void GameOver()
    {
        LevelMenuManager.DisableEscapeMenu();
        LevelMenuManager.ShowGameOverPanel();
        Debug.Log("Game Over!");
        
        // Aquí puedes añadir lógica adicional para manejar el final del juego
    }
    
    
    
    
    
    
    
    
    
    
}
