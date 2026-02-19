using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 10f;
    
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            dir += new  Vector3(0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += new  Vector3(0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += new  Vector3(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += new  Vector3(1, 0);
        }
        
        transform.position += (Time.deltaTime * moveSpeed) * dir;

        float camDir = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            camDir = 1f;
        }
        
        if (Input.GetKey(KeyCode.R))
        {
            camDir = -1f;
        }
        
        cam.orthographicSize +=  camDir * Time.deltaTime * moveSpeed;
    }
}
