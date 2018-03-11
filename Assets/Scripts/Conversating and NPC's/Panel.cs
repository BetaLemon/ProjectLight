using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Panel : MonoBehaviour
{
    public static Panel instance;

    Conversation currentConversation;
    ConversationDisplayer conversationDisplayer;

    int sentencesAmmount;
    int currentSentence;


    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false); //Asegura que no se muestra el panel primero
        conversationDisplayer = GetComponentInChildren<ConversationDisplayer>(); //Asigna el dialogo
    }

    public void PlayConversation(Conversation conversation) //Esta funcion se ejecuta desde el launch
    {
//        Debug.Log("Movimiento: false");
        //PlayerController.instance.startStopMovement(false); //HERE WE MUST CONSIDER IF WE WANT THE PLAYER TO BE BLOCKED FROM MOVING

        conversationDisplayer.SetConversationAndStart(conversation);
        PanelActivation(true); //Muestra el panel

        currentConversation = conversation; //Guarda la conversacion que habiamos recogido en dialoguelaunch como conversacion actual para el panel.
    }

    public void StopConversation()
    {
        conversationDisplayer.StopConversation();
        currentConversation = null;
    }

    public void PanelActivation(bool what) //Activar o desactivar panel
    {
        gameObject.SetActive(what);
    }

    public bool ConversationInProgress()
    {
        return currentConversation != null;
    }

}
