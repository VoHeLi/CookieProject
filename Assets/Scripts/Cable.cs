using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : Element
{

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite cableLeft;
    [SerializeField] private Sprite cableRight;
    [SerializeField] private Sprite cableLine;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    // Update is called once per frame
    void Update()
    {

        // Get des voisins
        Element.TypeElement voisinGauche = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() - 1, GetComponent<Element>().getYPos());
        Element.TypeElement voisinDroite = GrilleElementManager.instance.GetElementTypeAtPosition(GetComponent<Element>().getXPos() + 1, GetComponent<Element>().getYPos());
        

        // Change de sprite voisins pour changer de sprites (connecté à droite, à gauche, aux deux cotés)
        // TODO : vérifier que la case en dessous est différente d'une grille
        if ( (voisinDroite == TypeElement.None) && (voisinGauche == TypeElement.None))
        {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
        if ( !(voisinDroite == TypeElement.None) && (voisinGauche == TypeElement.None))
        {
            GetComponent<SpriteRenderer>().sprite = cableRight;
        }
        if ((voisinDroite == TypeElement.None) && !(voisinGauche == TypeElement.None))
        {
            GetComponent<SpriteRenderer>().sprite = cableLeft;
        }
        if ( !(voisinDroite == TypeElement.None) && !(voisinGauche == TypeElement.None))
        {
            GetComponent<SpriteRenderer>().sprite = cableLine;
        }

    }
}
