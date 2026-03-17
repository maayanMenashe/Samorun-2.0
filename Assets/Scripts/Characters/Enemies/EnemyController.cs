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
    private float shrinkedCameraSize = 6f;
    [SerializeField]
    private float combatTimeScale = 0.1f;
    [SerializeField]
    private int numOfHits = 4;
    [SerializeField]
    private GameObject upSlashPrefab;
    [SerializeField]
    private GameObject downSlashPrefab;
    [SerializeField]
    private GameObject rightSlashPrefab;
    [SerializeField]
    private GameObject leftSlashPrefab;

    
    // private fields
    private int arrowHorizontalDirection;
    private int arrowDirection;
    private Vector3 arrowOriginalScale;
    private float originalCameraSize;
    private float deathAnimDuration = 1 / 6f;
    private float originalTimeScale;
    private float playerOriginalSpeed;
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
    
    KeyCode[] RandomButtonSequence()
    {
        KeyCode[] newArray = new KeyCode[numOfHits];
        
        for (int i = 0; i < numOfHits; i++)
        {
            int num = randomNum.Next(1,numOfPossibleInputs + 1);

            switch (num)
            {
                case 1:
                    newArray[i] = KeyCode.LeftArrow;
                    break;
            
                case 2:
                    newArray[i] = KeyCode.RightArrow;
                    break;     
                
                case 3:
                    newArray[i] = KeyCode.UpArrow;
                    break;             
                
                case 4:
                    newArray[i] = KeyCode.DownArrow;
                    break;
            }
        }
        return newArray;
    }
    
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

    GameObject FindSlashPrefab(KeyCode input)
    {
        GameObject correctPrefab;
        switch (input)
        {
            case KeyCode.LeftArrow:
                correctPrefab = leftSlashPrefab;
                break;
            
            case KeyCode.RightArrow:
                correctPrefab = rightSlashPrefab;
                break;
            
            case KeyCode.UpArrow:
                correctPrefab = upSlashPrefab;
                break;
            
            default:
                correctPrefab = downSlashPrefab;
                break;    
        }
        return correctPrefab;
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
        
        // makes a new random seqence of inputs
        KeyCode[] inputs = RandomButtonSequence();


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
            Instantiate(FindSlashPrefab(inputs[i]), transform.position, Quaternion.identity);
            yield return null;
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



