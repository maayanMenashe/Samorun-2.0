using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField]
    public float speed = 1.0f;
    
    [SerializeField]
    private bool isEnemy;

    private float sign = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemy)
        {
            sign = 1;
        }
        Vector2 lerp2d = Vector2.Lerp(transform.position, transform.position + sign * Vector3.left, speed * Time.deltaTime);
        transform.position = new Vector3(lerp2d.x, lerp2d.y, transform.position.z); 
    }
}
