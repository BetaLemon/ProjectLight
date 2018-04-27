using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    [System.Serializable]
    public class CharacterSpeech 
    {
        public Sprite characterFace; //Sprite con la imagen que debe mostrarse durante esta conversacion
        public string name; //Nombre del personaje
        public string[] texts; //String con todo el dialogo especifico
    };

    public CharacterSpeech[] sentences; //Array con dialogos distintos
}
