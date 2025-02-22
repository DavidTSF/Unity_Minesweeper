using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject EscapeMenu;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject WinPanel;
    
    [SerializeField] private TextMeshProUGUI textTime; // Asignar desde el Inspector
    [SerializeField] private TextMeshProUGUI textFlags; // Asignar desde el Inspector
    
    [SerializeField] private Board board;
    private float elapsedTime = 0.0f;
    private bool isGameOver = false;





    void Start()
    {

        // Inicializar el tiempo
        elapsedTime = 0.0f;
        UpdateTimeText();

        textFlags.text = board.flags.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOverPanel.activeInHierarchy)
        {
            EscapeMenu.SetActive(!EscapeMenu.activeSelf);
        }
        
        if (!isGameOver)
        {
            // Actualizar el tiempo
            elapsedTime += Time.deltaTime;
            UpdateTimeText();

        }


        textFlags.text = board.flags.ToString();

    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void DisableEscapeMenu()
    {
        EscapeMenu.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
        isGameOver = true;
    }

    public void ShowWinPanel()
    {
        WinPanel.SetActive(true);
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        textTime.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }




}
