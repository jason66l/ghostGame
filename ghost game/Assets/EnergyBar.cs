using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxEnergy (float energy) {
        slider.maxValue = energy;
        slider.value = energy; 
    }
    public void SetEnergy(float energy) {
        slider.value = energy;
    }
    // Start is called before the first frame update
    void Start()
    {
        //saiodja
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
