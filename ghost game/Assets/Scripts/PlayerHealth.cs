using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullH;
    public Sprite emptyH;
    void Start()
    {
        health = 2;
        numOfHearts = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) 
            {
                hearts[i].sprite = fullH;
            }
            else
            {
                hearts[i].sprite = emptyH;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
