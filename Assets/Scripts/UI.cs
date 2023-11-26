using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("UI Customization")]
    [SerializeField] GameObject bottomImage;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject playButton;
    [SerializeField] float buttonSize;
    [SerializeField] float buttonBackgroundSize;
    [SerializeField] float buttonSpacing;

    [Header("Element Placement")]
    [SerializeField] EnergyResolver energyResolver;
    [SerializeField] GrilleElementManager grilleElementManager;
    [SerializeField] GameObject indication;

    private bool isRunning = false;



    // Start is called before the first frame update
    void Start()
    {
        startAgain(); // Desable the pause menu
        indication.SetActive(false);

        //Create a new navigation
        Navigation newNav = new Navigation();
        newNav.mode = Navigation.Mode.Horizontal;

        Texture2D tex = Resources.Load<Texture2D>("Logo_AnimINT");
        grilleElementManager.completeDictionnaries();

        for (int i = 1; i < Element.ELEMENT_TO_SELECT_COUNT; i++)
        {
            createButton(i - (float)Element.ELEMENT_TO_SELECT_COUNT / 2f, i, newNav);
        }
    }

    // Create a button for selecting an element
    private void createButton(float place, int index, Navigation newNav)
    {
        GameObject buttonBackground = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
        RectTransform rectTrans = buttonBackground.GetComponent<RectTransform>();

        buttonBackground.transform.SetParent(bottomImage.transform); // setting parent
        rectTrans.anchoredPosition = new Vector2(place * (buttonSize + buttonSpacing), 0f); // set position

        var childO = buttonBackground.transform.GetChild(0);

        // To add a custom size (prefab as normally the good size)
        rectTrans.sizeDelta = new Vector2(buttonBackgroundSize, buttonBackgroundSize);
        rectTrans.localScale = new Vector2(1, 1);
        RectTransform rectTransButton = childO.gameObject.GetComponent<RectTransform>();
        rectTransButton.sizeDelta = new Vector2(buttonSize, buttonSize);

        // Add image to the button
        if (!grilleElementManager.isDictionnariesCompleted) 
        {
            Debug.Log("Dictionnaires toujours pas complets");
        }

        Texture2D tex = Resources.Load<Texture2D>(((Element.TypeElement)grilleElementManager.getRelationElementSelection()[index][0]).ToString());

        Debug.Log(((Element.TypeElement)grilleElementManager.getRelationElementSelection()[index][0]).ToString());

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
        if (grilleElementManager.getRelationElementSelection()[i].Length > 1)
        {
            indication.SetActive(true);
        }
        else
        {
            indication.SetActive(false);
        }


        Debug.Log("Button press " + i);
        grilleElementManager.setCurrentPlacingElement(i);
    }


    // Start the game's simulation
    public void startSimulation()
    {
        Image playButtonImage = playButton.GetComponent<Image>();
        
        if (isRunning == false)
        {
            //Debug.Log("Start Game Simulation");
            // Change texture to reload button
            Texture2D retryTex =  Resources.Load<Texture2D>("Retry");
            playButtonImage.sprite = Sprite.Create(retryTex, new Rect(0, 0, retryTex.width, retryTex.height), new Vector2(0.5f, 0.5f));

            // Start simulation
            isRunning = true;
            energyResolver.ResolveLevelPart(grilleElementManager, grilleElementManager.sourcePosition);
        }
        else if (isRunning == true)
        {
            Texture2D playTex = Resources.Load<Texture2D>("Play");
            playButtonImage.sprite = Sprite.Create(playTex, new Rect(0, 0, playTex.width, playTex.height), new Vector2(0.5f, 0.5f));

            // End simulation
            isRunning = false;
            energyResolver.ClearEnergyObjects();
        }

        
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

    // Return to the Title Screen
    public void returnToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    // Quit the game
    public void quit()
    {
        Application.Quit();
    }
}
