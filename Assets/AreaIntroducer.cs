using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaIntroducer : MonoBehaviour {

    public string areaName;
    public int areaIndex; //Used for notifying the Ingame Progress Script

    public GameObject introDisplayerRef;
    private Animator introDisplayerAnimationRef; //Intro displayer animation reference
    private Text areaTextRef;
    private IngameProgressScript ingameProgressRef; //Progress script reference


	// Use this for initialization
	void Start () {
        introDisplayerAnimationRef = introDisplayerRef.GetComponent<Animator>();
        areaTextRef = introDisplayerRef.transform.GetChild(2).GetComponent<Text>();

        ingameProgressRef = GameObject.Find("GameState").GetComponent<IngameProgressScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ingameProgressRef.getAreaVisited(areaIndex) != true)
        {
            areaTextRef.text = areaName;
            //introDisplayerAnimationRef.Play();
            ingameProgressRef.setAreaVisited(true, areaIndex); //notifying the Ingame Progress Script, we visited this area according to index
        }
    }
}
