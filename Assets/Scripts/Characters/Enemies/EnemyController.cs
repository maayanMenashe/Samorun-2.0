using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyController : MonoBehaviour
{
    // serialized fields
    [SerializeField]
    private GameObject horizontalArrow;
    [SerializeField]
    private GameObject verticalArrow;
    [SerializeField]
    private GameObject SlashPrefab;
    [SerializeField]
    private float shrinkedCameraSize = 6f;
    [SerializeField]
    private float combatTimeScale = 0.1f;
    
    
    // private fields
    private int arrowHorizontalDirection;
    private int arrowDirection;
    private Vector3 arrowOriginalScale;
    private float originalCameraSize;
    private float deathAnimDuration = 1 / 6f;
    private float originalTimeScale;
    private float playerOriginalSpeed;
    private int numOfHitsEnemy = 4;
    private int numOfHitsBoss = 7;
    private int numOfPossibleInputs = 4;
    
    
    
    // components
    private Animator animator;
    private Camera camera;
    private AudioManager audioManager;
    
    private System.Random randomNum = new System.Random();

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initializes some fields
        camera = Camera.main; // sets camera
        originalCameraSize = camera.orthographicSize; // saves camera size
        animator = GetComponent<Animator>(); // gets animator
        arrowOriginalScale = verticalArrow.transform.localScale;// saves arrow's scale
        audioManager = FindAnyObjectByType<AudioManager>();// gets audiomanager
        originalTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // plays the SXF
    void PlaySlashSFX()
    {
        audioManager.PlaySlashSFX();
    }
    
    // plays the SXF
    void PlayFastSlashSFX()
    {
        audioManager.PlayFastSlashSFX();
    }
    
    // plays the SXF
    void PlayKillSFX()
    {
        audioManager.PlayKillEnemySFX();
    }
    
    // destroys an instance of an enemy (used at the end of death anim)
    void KillEnemy()
    {
        Destroy(gameObject);
    }

    /*
    KeyCode[] RandomButtonSequence(int numOfHits)
    {
        KeyCode[] newArray = new KeyCode[numOfHits];
        
        for (int i = 0; i < numOfHits; i++)
        {
            int num = randomNum.Next(1,5);

        }

        switch (num)
        {
            case 1:
                break;
            
            case 2:
                break;
        }
    }
    */
    
    // makes arrow appear correctly
    private void FlipArrow(KeyCode input)
    {
        // makes only horizontal arrow appear
        if (input == KeyCode.LeftArrow || input == KeyCode.RightArrow)
        {
            verticalArrow.SetActive(false);
            horizontalArrow.SetActive(true);
        }
        
        // makes only vertical arrow appear
        if (input == KeyCode.UpArrow || input == KeyCode.DownArrow)
        {
            verticalArrow.SetActive(true);
            horizontalArrow.SetActive(false);
        }
        
        //arrowHorizontalDirection = 1;
        arrowDirection = 1;
        
        // flips it if right
        if (input == KeyCode.RightArrow)
        {
            arrowDirection = -1;
        }
        
        // flips it if down
        if (input == KeyCode.DownArrow)
        {
            arrowDirection = -1;
        }
        
        //flips both
        verticalArrow.transform.localScale = new Vector3(
            arrowOriginalScale.x,
            arrowDirection * arrowOriginalScale.y,
            arrowOriginalScale.z);

        horizontalArrow.transform.localScale = verticalArrow.transform.localScale;
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { 
            StartCoroutine(Fight(other));// starts fight routine if player enters collider
        }
    }
    
    //the fight routine that slows down time, zooms camera and checks for inputs
    IEnumerator Fight(Collider2D other)
    {
        playerOriginalSpeed = other.GetComponent<CharacterMovementController>().speed;
        camera.orthographicSize = shrinkedCameraSize;// zooms
        Time.timeScale = combatTimeScale;// slows time
        
        // this is an input array example, that will be replaced by a function that creates a random one
        KeyCode[] inputs = new KeyCode[4];
        inputs[0] = KeyCode.LeftArrow;
        inputs[1] = KeyCode.RightArrow;
        inputs[2] = KeyCode.LeftArrow;
        inputs[3] = KeyCode.RightArrow;

        // loops one time for each input in the array
        for (int i = 0; i < inputs.Length; i++)
        {
            animator.SetInteger("AnimationNum", i);// sets the animation trigger
            camera.orthographicSize = shrinkedCameraSize - i;//shrinks the camera more and more
            
            // checks if the right input was pressed
            while (!Input.GetKeyDown(inputs[i]))
            {
                FlipArrow(inputs[i]);// positions the arrow correctly accurding to the expected input
                yield return null;
            }
            Instantiate(SlashPrefab, transform.position, Quaternion.identity);
        }
        
        // stops both characters and plays dead anim
        verticalArrow.SetActive(false);
        horizontalArrow.SetActive(false);
        other.GetComponent<CharacterMovementController>().speed = 0;
        GetComponent<CharacterMovementController>().speed = 0;
        animator.SetTrigger("Dead"); 
        
        yield return new WaitForSeconds(deathAnimDuration); // waits for the death animation to end
        
        // reverts everything back to normal
        Time.timeScale = originalTimeScale;
        other.GetComponent<CharacterMovementController>().speed = playerOriginalSpeed;
        camera.orthographicSize = originalCameraSize;
    }
    
}



