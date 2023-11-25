using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batterie : Element
{

    private float energy;

    [SerializeField] private Sprite batterie100;
    [SerializeField] private Sprite batterie75;
    [SerializeField] private Sprite batterie50;
    [SerializeField] private Sprite batterie25;
    [SerializeField] private Sprite batterie0;

    [SerializeField] private float dureeAnimation = 5f;
    [SerializeField] private float animationFPS = 10f;

    // Start is called before the first frame update
    void Start()
    {
        energy = 100f;
        Debug.Log("Energy de depart : " + energy);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Energy = " + energy);

        if (energy <= 100f && energy > 75f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie100;
        }
        if (energy <= 75f && energy > 50f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie75;
        }
        if (energy <= 50f && energy > 25f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie50;
        }
        if (energy <= 25f && energy > 0f)
        {
            GetComponent<SpriteRenderer>().sprite = batterie25;
        }
        if (energy == 0)
        {
            GetComponent<SpriteRenderer>().sprite = batterie0;
        }
    }

    // TODO : Commencer l'animation pas à un click de la souris mais quand on presse le bouton Start animation
    private void OnMouseDown()
    {
        Debug.Log("Click");
        StartCoroutine(startBatterieAnimation());
    }


    // Coroutine qui baisse le niveau de l'énergie en $dureeAnimation secondes
    IEnumerator startBatterieAnimation()
    {
        for (float energyLevel = 100f; energyLevel >= 0; energyLevel -= 100/(animationFPS*dureeAnimation))
        {
            energy = energyLevel;
            yield return new WaitForSeconds(1/animationFPS);
        }
    }
}
