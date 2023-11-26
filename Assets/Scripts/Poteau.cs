using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poteau : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite sprite_middle;
    [SerializeField] private Sprite sprite_in;

    [SerializeField] private bool isBottom;
    [SerializeField] public bool isMiddle;
    [SerializeField] private bool isTop;

    [SerializeField] Element element;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
        element = GetComponent<Element>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject voisinTop = GrilleElementManager.instance.GetObjetAtPosition(element.getXPos(), element.getYPos() + 1);
        GameObject voisinBottom = GrilleElementManager.instance.GetObjetAtPosition(element.getXPos(), element.getYPos() - 1);
        GameObject voisinDroite = GrilleElementManager.instance.GetObjetAtPosition(element.getXPos() + 1, element.getYPos());

        isTop = false;
        isMiddle = false;
        isBottom = false;

        if (voisinTop != null && voisinTop.GetComponent<Element>().type == Element.TypeElement.Poteau)
        {
            if (voisinBottom != null && voisinBottom.GetComponent<Element>().type == Element.TypeElement.Poteau) isMiddle = true;
            else isBottom = true;
        }
        else isTop = true;

        spriteRenderer.flipX = (voisinDroite != null && voisinDroite.GetComponent<Element>().type == Element.TypeElement.Cable);
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (isBottom) spriteRenderer.sprite = sprite_in;
        if (isMiddle) spriteRenderer.sprite = sprite_middle;
        if (isTop) spriteRenderer.sprite = defaultSprite;

        

    }
}
