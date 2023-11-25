using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject bottomImage;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject pausePanel;
    [SerializeField] float buttonSize;



    // Start is called before the first frame update
    void Start()
    {
        startAgain(); // Desable the pause menu

        //Create a new navigation
        Navigation newNav = new Navigation();
        newNav.mode = Navigation.Mode.Horizontal;

        Texture2D tex = Resources.Load<Texture2D>("Logo_AnimINT");

        createButton(0, tex, newNav);
        createButton(1, tex, newNav);
        createButton(2, tex, newNav);        
    }

    // Create a button for selecting an element
    private void createButton(int index, Texture2D tex, Navigation newNav)
    {
        GameObject buttonBackground = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
        RectTransform rectTrans = buttonBackground.GetComponent<RectTransform>();

        buttonBackground.transform.SetParent(bottomImage.transform); // setting parent
        rectTrans.anchoredPosition = new Vector2(index * buttonSize * 1.2f, 0f); // set position

        var childO = buttonBackground.transform.GetChild(0);

        // To add a custom size (prefab as normally the good size)
        rectTrans.sizeDelta = new Vector2(buttonSize, buttonSize);
        RectTransform rectTransButton = childO.gameObject.GetComponent<RectTransform>();
        rectTransButton.sizeDelta = new Vector2(buttonSize, buttonSize);

        // Add image to the button
        Image buttonImage = childO.gameObject.GetComponent<Image>();
        buttonImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        // Add function to the button
        Button button = childO.GetComponent<Button>();
        button.onClick.AddListener(delegate { chooseItem(index); });

        //Assign the new navigation to the button
        button.navigation = newNav;
    }

    // Select the element wanted
    public void chooseItem(int i)
    {
        Debug.Log("Button press" + i);
    }

    // Start the game's simulation
    public void startSimulation()
    {
        Debug.Log("Game Start");
    }





    // Enable pause menu
    public void pause()
    {
        pausePanel.SetActive(true);
    }

    // Disable pause menu
    public void startAgain()
    {
        pausePanel.SetActive(false);
    }

    // Quit the game
    public void quit()
    {
        Application.Quit();
    }
}
