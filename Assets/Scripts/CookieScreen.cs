using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CookieScreen : MonoBehaviour
{
    // Start is called before the first frame update
    

    public void returnToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void quit()
    {
        Application.Quit();
    }

}
