using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDisplayerScript : MonoBehaviour {

    public void ResetDisplayAnimation()
    {
        GetComponent<Animator>().SetBool("ShowAreaDisplayer",false);
    }

}
