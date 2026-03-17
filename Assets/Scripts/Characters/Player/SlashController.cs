using UnityEngine;

public class SlashController : MonoBehaviour
{
    private AudioManager audioManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void DestroySlash()
    {
        Destroy(gameObject);
    }

    void PlaySFX()
    {
        audioManager.PlaySlashSFX();
    }
    
}
