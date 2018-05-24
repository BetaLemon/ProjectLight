using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class ConversationDisplayer : MonoBehaviour
{

    //Some references for external interaction
    public GameStateScript gameStateDataScriptRef;
    public GameObject playerRef;
    public Cinemachine.CinemachineVirtualCamera npcCamRef;
    public Cinemachine.CinemachineBrain brainCamRef;
    public int priorityOnExit = 1;

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

    public static float SecondsBetweenCharacters = 0.01f; //Delay between character's being shown on the text display.

    bool startTheDialogue = false;

    public GameObject ContinueIcon;

    public GameObject Name;
    private NameDisplay access;

    public GameObject Sprite;
    private ImageDisplay access2;

    //INICIAIZATION
    void Start()
    {
        gameStateDataScriptRef = GameObject.Find("GameState").GetComponent<GameStateScript>();
        playerRef = GameObject.FindWithTag("Player");

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
        // Debug.Log("Movimiento: true");
        playerRef.GetComponent<PlayerController>().AllowMovement(); //REACTIVATE MOVEMENT
        playerRef.GetComponent<PlayerLight>().AllowLightUsage();
        Panel.instance.PanelActivation(false);
        brainCamRef.m_DefaultBlend.m_Time = 0;
        if (npcCamRef != null) npcCamRef.Priority = priorityOnExit;
        gameStateDataScriptRef.cameraCoroutine(0.1f);
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
