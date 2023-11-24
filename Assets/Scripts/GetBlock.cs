using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetBlock : MonoBehaviour
{
    private Plane _plane;

    [SerializeField] 
    private Tilemap _tilemap;
    // Start is called before the first frame update
    void Start()
    {
        _plane = new Plane(new Vector3(0, 0, 1), new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            float enter = 0.0f;

            if (_plane.Raycast(ray, out enter))
            {
                
                Vector2 hitPoint = ray.GetPoint(enter);
                int xtile = Mathf.FloorToInt(hitPoint.x);
                int ytile = Mathf.FloorToInt(hitPoint.y);
                Vector3Int f = new Vector3Int(xtile, ytile, 0);
                Debug.Log(_tilemap.HasTile(f));
            }

        }
    }
}
