using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLaunch : MonoBehaviour
{
    private bool nowInteract = false;
    public ConversationDisplayer textCanvasTextRef;
    public Cinemachine.CinemachineVirtualCamera npcCamRef;
    private GameObject playerRef;
    private Animator animator;
    public int priorityOnEnter = 10;

    private void Start()
    {
        // THIS. THIS IS JUST UGLY:
        //textCanvasTextRef = GameObject.Find("SimpleHud").transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.GetComponent<ConversationDisplayer>();
        // This does the same, and actually is much more reliable:
        textCanvasTextRef = FindObjectOfType<ConversationDisplayer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other) //Función que da la oportunidad de hablar con un npc
    {

        if (other.gameObject.tag == "Player") //Si el objeto que ha causado la colision es el mago
        {
            playerRef = Player.instance.gameObject;
            nowInteract = true; //Ahora si podemos pulsar enter para activar el dialogo desde el update
        }
    }

    private void OnTriggerExit(Collider other) //Salimos de la colision, por lo tanto no se puede pulsar enter para activar el dialogo
    {
        nowInteract = false;
        Panel.instance.StopConversation();
    }

    private void Update()
    {
        if (nowInteract &&
            Input.GetButtonDown("BaseInteraction") &&
            !Panel.instance.ConversationInProgress()) //Comprobar si se ha pulsado enter mientras estabamos dentro de la colision
        {
            Conversation conversation = GetComponentInChildren<Conversation>(); //Recoge el elemento de conversacion del npc al qual va adjudicado este script
            Panel.instance.PlayConversation(conversation); //Ejecuta la funcion de reproducir conversacion en el Panel con la conversacion que hemos recogido

            playerRef.GetComponent<PlayerController>().StopMovement();
            playerRef.GetComponent<PlayerLight>().StopLightUsage();

            if (npcCamRef != null)
            {
                npcCamRef.LookAt = transform;
                npcCamRef.Priority = priorityOnEnter;
            }
        }
        if (Panel.instance.ConversationInProgress()) { animator.SetBool("Talking", true); }
        else { animator.SetBool("Talking", false); }
    }

    public void StartConv()
    {
        Conversation conversation = GetComponentInChildren<Conversation>(); //Recoge el elemento de conversacion del npc al qual va adjudicado este script
        Panel.instance.PlayConversation(conversation); //Ejecuta la funcion de reproducir conversacion en el Panel con la conversacion que hemos recogido
    }
}
