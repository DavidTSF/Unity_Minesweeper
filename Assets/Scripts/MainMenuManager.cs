
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool isGameMenu = false;

    [SerializeField] private Button buttonExit;

    [SerializeField] private Button buttonLevel1;
    [SerializeField] private Button buttonLevel2;
    [SerializeField] private Button buttonLevel3;
    [SerializeField] private Button buttonCustomLevel;
    [SerializeField] private GameObject customMenuLevel;

    [SerializeField] private Button buttonSaveCustomLevel;
    [SerializeField] private TMP_InputField inputWidth;
    [SerializeField] private TMP_InputField inputHeight;
    [SerializeField] private TMP_InputField inputBombs;


    void Awake()
    {

        buttonExit.onClick.AddListener(delegate { Application.Quit(); });
        buttonLevel1.onClick.AddListener(() => SceneManager.LoadScene("Level1", LoadSceneMode.Single));
        buttonLevel2.onClick.AddListener(() => SceneManager.LoadScene("Level2", LoadSceneMode.Single));
        buttonLevel3.onClick.AddListener(() => SceneManager.LoadScene("Level3", LoadSceneMode.Single));
        buttonCustomLevel.onClick.AddListener(() => customMenuLevel.SetActive(true));
        buttonSaveCustomLevel.onClick.AddListener(SaveCustomLevel);
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customMenuLevel.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {

        buttonCustomLevel.onClick.AddListener(() => customMenuLevel.SetActive(true));

    }
    public void SaveCustomLevel()
    {
        int width = int.Parse(inputWidth.text);
        int height = int.Parse(inputHeight.text);
        int bombs = int.Parse(inputBombs.text);
        

        // Aquí puedes guardar los valores en PlayerPrefs, un archivo, o pasarlos a otra escena
        PlayerPrefs.SetInt("CustomWidth", width);
        PlayerPrefs.SetInt("CustomHeight", height);
        PlayerPrefs.SetInt("CustomBombs", bombs);
        PlayerPrefs.SetInt("LoadCustom", 1);

        Debug.Log("Custom Level Saved: Width=" + width + ", Height=" + height + ", Bombs=" + bombs);

        // Cerrar el panel de configuración personalizada
        customMenuLevel.SetActive(false);
        SceneManager.LoadScene("CustomLevel", LoadSceneMode.Single);
    }
    
}
