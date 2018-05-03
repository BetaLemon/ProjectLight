using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaIntroducer : MonoBehaviour {

    public string areaName;
    public bool hasShownIntro = false;

    private GameObject introDisplayerRef; //Intro displayer animation reference
    private IngameProgressScript ingameProgressRef; //Progress script reference
    private Text areaTextRef;

	// Use this for initialization
	void Start () {
        introDisplayerRef = GameObject.Find("HUD").transform.GetChild(4).gameObject;
        ingameProgressRef = GameObject.Find("GameState").GetComponent<IngameProgressScript>();
        areaTextRef = introDisplayerRef.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasShownIntro != true)
            {
                areaTextRef.text = areaName;
                //introDisplayerRef.SetActive(true);
            }
        }
    }
}
