using System.Collections;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    
    [SerializeField]
    private GameObject mainMenu;
    
    [SerializeField]
    private GameObject exposition;

    [SerializeField]
    private GameObject tutorial;
    
    [SerializeField]
    private GameObject winScreen;
    
    private AudioManager audioManager;

    private bool isPaused;
    
    private bool pausable;

    private float currentTimeScale = 1f;

    private float originalVolume;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        Time.timeScale = 0f;
        originalVolume = audioManager.backgroundMusic.volume;
        audioManager.backgroundMusic.volume = originalVolume/3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && pausable)
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
            audioManager.backgroundMusic.volume = originalVolume/3;
        }
        else
        {
            Time.timeScale = currentTimeScale;
            audioManager.backgroundMusic.volume = originalVolume;
        }
        
        
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    
    
    public void StartGame()
    {
        pausable = true;
        mainMenu.SetActive(false);
        Time.timeScale = currentTimeScale;
        audioManager.backgroundMusic.volume = originalVolume;
        StartCoroutine(PlayTwoPopups(exposition, tutorial));
    }
    
    




    IEnumerator PlayTwoPopups(GameObject popup1, GameObject popup2)
    {
        pausable = false;
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        audioManager.backgroundMusic.volume = originalVolume/3;
        popup1.SetActive(true);
        
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        
        popup1.SetActive(false);
        yield return new WaitForEndOfFrame();
        popup2.SetActive(true);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        
        popup2.SetActive(false);
        Time.timeScale = currentTimeScale;
        audioManager.backgroundMusic.volume = originalVolume;
        pausable = true;
    }

    void Victory()
    {
        Time.timeScale = 0f;
        audioManager.backgroundMusic.volume = originalVolume/3;
        winScreen.SetActive(true);
        pausable = false;
    }
    
}



