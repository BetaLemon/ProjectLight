using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Sprite))]

public class ImageDisplay : MonoBehaviour
{
    public static ImageDisplay instance;
    public Sprite anImage;

    void Start()
    {
    }

    void Update()
    {
        anImage = ConversationDisplayer.displayerInstance.getImageWhoTalks();
        gameObject.GetComponent<SpriteRenderer>().sprite = anImage;
    }


}
