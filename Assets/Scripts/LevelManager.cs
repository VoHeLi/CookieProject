using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] string[] lvlList;

    private List<bool> isFinished;


    // Start is called before the first frame update
    void Start()
    {
        isFinished = new List<bool>(lvlList.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
