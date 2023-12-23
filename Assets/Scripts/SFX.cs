using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    // Start is called before the first frame update
    static bool exist = false;
    void Start()
    {
        if (SFX.exist) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        SFX.exist = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
