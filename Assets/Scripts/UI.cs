using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("UI Customization")]
    [SerializeField] GameObject bottomImage;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject pausePanel;
    [SerializeField] float buttonSize;
    [SerializeField] float buttonSpacing;

    [Header("Element Placement")]
    [SerializeField] EnergyResolver energyResolver;
    [SerializeField] GrilleElementManager grilleElementManager;
    [SerializeField] GameObject indication;



    // Start is called before the first frame update
    void Start()
    {
        startAgain(); // Desable the pause menu
        indication.SetActive(false);

        //Create a new navigation
        Navigation newNav = new Navigation();
        newNav.mode = Navigation.Mode.Horizontal;

        Texture2D tex = Resources.Load<Texture2D>("Logo_AnimINT");

        for (int i = 1; i < Element.ELEMENT_TO_SELECT_COUNT; i++)
        {
            createButton(i - (float)Element.ELEMENT_TO_SELECT_COUNT / 2f, i, tex, newNav);
        }
    }

    // Create a button for selecting an element
    private void createButton(float place, int index, Texture2D tex, Navigation newNav)
    {
        GameObject buttonBackground = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
        RectTransform rectTrans = buttonBackground.GetComponent<RectTransform>();

        buttonBackground.transform.SetParent(bottomImage.transform); // setting parent
        rectTrans.anchoredPosition = new Vector2(place * (buttonSize + buttonSpacing), 0f); // set position

        var childO = buttonBackground.transform.GetChild(0);

        // To add a custom size (prefab as normally the good size)
        rectTrans.sizeDelta = new Vector2(buttonSize, buttonSize);
        rectTrans.localScale = new Vector2(1, 1);
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
        Debug.Log("Start Game Simulation");
        energyResolver.ResolveLevelPart(grilleElementManager, grilleElementManager.sourcePosition);
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
