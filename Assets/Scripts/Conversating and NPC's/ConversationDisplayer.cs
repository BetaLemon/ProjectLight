using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class ConversationDisplayer : MonoBehaviour
{
    
    public static ConversationDisplayer displayerInstance;

    private Text _textComponent; //We save the text we want to write to the text component here

    public AudioSource TextSound;

    public string[] DialogueStrings; //All possible dialogues we wish to display
    Conversation currentConversation;

    int idxSentence;
    int idxText;
    int idxChar;

    enum State
    {
        RevealingText,
        WaitingForInput,
        Finished,
    };

    State currentState = State.RevealingText;

    public float SecondsBetweenCharacters = 0.03f; //Delay between character's being shown on the text display.

    //public KeyCode DialogueInput = KeyCode.Z;
    bool startTheDialogue = false;

    public GameObject ContinueIcon;

    public GameObject Name;
    private NameDisplay access;

    public GameObject Sprite;
    private ImageDisplay access2;

    //INICIAIZATION
    void Start()
    {
        displayerInstance = this;

        access = Name.GetComponent<NameDisplay>();
        //access2 = Sprite.GetComponent<ImageDisplay>();

        _textComponent = GetComponent<Text>();
        _textComponent.text = ""; //Emptying text display just for good measure.

        ContinueIcon.SetActive(false);
    }

    public void SetConversationAndStart(Conversation newConversation)
    {
        currentConversation = newConversation;

        currentState = State.RevealingText;
        idxSentence = 0;
        idxText = 0;
        idxChar = 0;

        gameObject.SetActive(true);
    }

    public void StopConversation()
    {
        currentConversation = null;
    }


    //UPDATE ONCE PER FRAME
    float timeLeftToRevealNextChar = 0f;
    void Update()
    {
        //Actualizando nombre de quien habla:
        //nameDisplayer.updateName("AAAAA");
        access.aName = currentConversation.sentences[idxSentence].name;
//        Debug.Log("Nombre en uso: " +currentConversation.sentences[idxSentence].name);

        //Actualizando imagen de quien habla:
        //access2.anImage = currentConversation.sentences[idxSentence].characterFace;
//        Debug.Log("Sprite en uso: " +currentConversation.sentences[idxSentence].characterFace);

        switch (currentState)
        {
            case State.RevealingText:
                timeLeftToRevealNextChar -= Time.deltaTime;
                if (timeLeftToRevealNextChar < 0f)
                {
                    TextSound.Play();
                    _textComponent.text += currentConversation.sentences[idxSentence].texts[idxText][idxChar];
                    idxChar++;

                    if (idxChar >= currentConversation.sentences[idxSentence].texts[idxText].Length)
                    {
                        ContinueIcon.SetActive(true);
                        currentState = State.WaitingForInput;
                    }
                    else {
                        ContinueIcon.SetActive(false);
                    }

                    timeLeftToRevealNextChar = SecondsBetweenCharacters;
                }
                break;

            case State.WaitingForInput:
                if (Input.GetButtonDown("BaseInteraction"))
                {
                    idxChar = 0;
                    _textComponent.text = "";
                    idxText++;

                    if (idxText >= currentConversation.sentences[idxSentence].texts.Length)
                    {
                        idxSentence++;

                        if (idxSentence >= currentConversation.sentences.Length)
                        {
                            KillTheFuckingBox();
                        }
                        else
                        {
                            idxText = 0;
                            _textComponent.text = "";
                            currentState = State.RevealingText;
                            ContinueIcon.SetActive(false);
                        }
                    }
                    else
                    {
                        currentState = State.RevealingText;
                    }
                }
                break;
        }
    }

    private void KillTheFuckingBox() //Hace desaparecer el panel y termina la conversacion
    {
//        Debug.Log("Movimiento: true");
        //PlayerController.instance.startStopMovement(true); //REACTIVATE MOVEMENT?
        Panel.instance.PanelActivation(false);
    }

    public void StartTheDialogue()
    {
        startTheDialogue = true;
    }

    public Sprite getImageWhoTalks()
    {
        Sprite theSprite = currentConversation.sentences[idxSentence].characterFace;
        return theSprite;
    }
}
