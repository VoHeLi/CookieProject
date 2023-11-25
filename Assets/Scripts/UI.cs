using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] float buttonSize;
    private List<GameObject> gameObjectList;



    // Start is called before the first frame update
    void Start()
    {
        GameObject bottomImage = canvas.transform.GetChild(1).gameObject;
        Texture2D tex = Resources.Load<Texture2D>("Logo_AnimINT");
        createButton(bottomImage, 0, tex);
        createButton(bottomImage, 1, tex);
        createButton(bottomImage, 2, tex);        
    }

    // Create a button for selecting an element
    private void createButton(GameObject bottomImage, int index, Texture2D tex)
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
    }

    // Select the element wanted
    public void chooseItem(int i)
    {
        Debug.Log("Button press" + i);
    }





    // Enable pause menu
    public void pause()
    {
        Debug.Log("pause");
    }

    // Start the game's simulation
    public void startGame()
    {
        Debug.Log("Game Start");
    }
}
