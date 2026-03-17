using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    
    private AudioManager audioManager;

    private bool isPaused;

    private float currentTimeScale = 1f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            OpenOrClosePauseMenu();
        }
    }

    public void OpenOrClosePauseMenu()
    {
        
        pauseMenu.SetActive(!isPaused);
        isPaused = !isPaused;
        if (isPaused)
        {
            currentTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            audioManager.backgroundMusic.volume = 0.1f;
        }
        else
        {
            Time.timeScale = currentTimeScale;
            audioManager.backgroundMusic.volume = 0.3f;
        }
        
        
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    
    
    
}



