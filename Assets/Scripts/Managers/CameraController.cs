using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float speed = 1.0f;
    
    [SerializeField]
    GameObject target;

    private Camera Camera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lerp2d = Vector2.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position = new Vector3(lerp2d.x, lerp2d.y, transform.position.z);    
    }

    /*
    void Zoom()
    {
        speed = 20;
        Vector2 lerp2d = Vector2.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position = new Vector3(lerp2d.x, lerp2d.y, transform.position.z);  
    }
    */
    
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CombatArea"))
        {
            Camera.orthographicSize = 5;
        }
    }
    */
}
