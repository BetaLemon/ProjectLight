using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class NameDisplay : MonoBehaviour
{
    public static NameDisplay instance;
    public string aName;

    private Text _textComponent;

    void Start()
    {
        _textComponent = GetComponent<Text>();
        _textComponent.text = ""; //Emptying text display just for good measure.
    }

    void Update()
    {
        _textComponent.text = aName;
    }
}

