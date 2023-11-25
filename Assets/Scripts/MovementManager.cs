using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [Header("Movement settings")]

    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private float border = 50f;

    [Space]
    [Header("Scroll settings")]
    [SerializeField] private float maxZoomDistance = 15f;
    [SerializeField] private float minZoomDistance = 1f;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3((GlobalGrid.nbCaseX * GlobalGrid.caseSize / 2), (GlobalGrid.nbCaseY * GlobalGrid.caseSize / 2), 0f);
    }

    // Update is called once per frame
    void Update()
    {

        float cameraHalfHeight = GetComponent<Camera>().orthographicSize;
        float cameraHalfWidth = GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect;

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
            
            // D�placement de la cam�ra par la souris si elle sort de l'�cran
            if (Input.mousePosition.x > Screen.width - border && (transform.position.x < GlobalGrid.nbCaseX * GlobalGrid.caseSize - cameraHalfWidth - 0.5f))
            {
                transform.position = transform.position + Vector3.right * cameraSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.x < border && (transform.position.x > cameraHalfWidth + 0.5f))
            {
                transform.position = transform.position + Vector3.left * cameraSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y > Screen.height - border && (transform.position.y < GlobalGrid.nbCaseY * GlobalGrid.caseSize - cameraHalfHeight - 0.5f))
            {
                transform.position = transform.position + Vector3.up * cameraSpeed * Time.deltaTime;
            }
            if (Input.mousePosition.y < border && (transform.position.y > cameraHalfHeight + 0.5f))
            {
                transform.position = transform.position + Vector3.down * cameraSpeed * Time.deltaTime;
            }

            // Zoom et dezoom de la cam�ra
            GetComponent<Camera>().orthographicSize = Mathf.Clamp(cameraHalfHeight - Input.mouseScrollDelta.y, minZoomDistance, (GlobalGrid.nbCaseX * GlobalGrid.caseSize / 2)/ GetComponent<Camera>().aspect);

            // Recadrage de la cam�ra si en se d�pla�ant ou dezoomant on sort des limites du niveau
            if (transform.position.y > GlobalGrid.nbCaseY * GlobalGrid.caseSize - cameraHalfHeight)
            {
                transform.position = new Vector3(transform.position.x, GlobalGrid.nbCaseY * GlobalGrid.caseSize - cameraHalfHeight, 0f);
            }
            if (transform.position.y < cameraHalfHeight)
            {
                transform.position = new Vector3(transform.position.x, cameraHalfHeight, 0f);
            }
            if (transform.position.x > GlobalGrid.nbCaseX * GlobalGrid.caseSize - cameraHalfWidth)
            {
                transform.position = new Vector3(GlobalGrid.nbCaseX * GlobalGrid.caseSize - cameraHalfWidth, transform.position.y, 0f) ;
            }
            if (transform.position.x < cameraHalfWidth)
            {
                transform.position = new Vector3(cameraHalfWidth, transform.position.y, 0f);
            }
        }


    }
}
