using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField]
    private float cameraSpeed = 5f;
    [SerializeField]
    private float border = 50f;

    [Space]
    [Header("Scroll settings")]
    [SerializeField]
    private float maxZoomDistance = 15f;
    [SerializeField]
    private float minZoomDistance = 1f;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            cameraSpeed = 10f;
        } else
        {
            cameraSpeed = 5f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
        
        if (!isPaused)
        {
            
            // Déplacement de la caméra par la souris si elle sort de l'écran
            if (Input.mousePosition.x > Screen.width - border)
            {
                transform.position = transform.position + Vector3.right * cameraSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.x < border)
            {
                transform.position = transform.position + Vector3.left * cameraSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y > Screen.height - border)
            {
                transform.position = transform.position + Vector3.up * cameraSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y < border)
            {
                transform.position = transform.position + Vector3.down * cameraSpeed * Time.deltaTime;
            }

            // Zoom et dezoom de la caméra
            GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize - Input.mouseScrollDelta.y, minZoomDistance, maxZoomDistance);

        }


    }
}
