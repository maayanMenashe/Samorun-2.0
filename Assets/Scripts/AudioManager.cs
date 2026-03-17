using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource SFX;

    [SerializeField] private AudioClip music;

    [SerializeField] private AudioClip slash;
    [SerializeField] private AudioClip fastSlash;
    [SerializeField] private AudioClip enemyDeath1;
    [SerializeField] private AudioClip enemyDeath2;
    [SerializeField] private AudioClip playerDeath;


    private System.Random randomNum = new System.Random();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void PlaySlashSFX()
    {
        SFX.PlayOneShot(slash);
    }
    
    public void PlayFastSlashSFX()
    {
        SFX.PlayOneShot(fastSlash);
    }
    
    public void PlayKillEnemySFX()
    {
        int num = randomNum.Next(1,3);

        switch (num)
        {
            case 1:
                SFX.PlayOneShot(enemyDeath1);
                break;
            
            case 2:
                SFX.PlayOneShot(enemyDeath1);
                break;
        }
        
    }
    
    public void PlayPlayerDeathSFX()
    {
        SFX.PlayOneShot(playerDeath);
    }
    
    
    
    
}
