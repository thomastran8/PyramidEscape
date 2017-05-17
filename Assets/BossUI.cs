using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour {
    Slider slider;
    Mummy boss;
    public Image fill;
    // Use this for initialization
    void Start () {
        slider = GetComponent<Slider>();
        boss = GameObject.Find("Boss Mummy").GetComponent<Mummy>();
        slider.maxValue = boss.health;
        slider.value = boss.health;
    }
	
	// Update is called once per frame
	void Update () {
        slider.value = boss.health;
        if (slider.value <= slider.maxValue / 4) {
            fill.color = Color.red;
        }
        else if (slider.value < slider.maxValue/2) {
            fill.color = Color.yellow;
        }
        else {
            fill.color = Color.green;
        }
	}
}
