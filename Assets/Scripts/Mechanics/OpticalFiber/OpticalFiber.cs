﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalFiber : MonoBehaviour {

    private Transform[] nodes;
    private GameObject[] cables;
    public GameObject cablePrefab;
    private GameObject connectionsContainer;

    private float propagationRate = 0.2f;
    private float dissipateRate = 1.0f;

    public bool reversed;
    private bool wasReversed;

	// Use this for initialization
	void Start () {
        OpticalSetup();
        reversed = false;
        wasReversed = false;
	}
	
	// Update is called once per frame
	void Update () {
        ChargePropagation();
        if(wasReversed != reversed) {
            ReverseNodes();
            ReverseCables();
            wasReversed = reversed;
        }
        ChargeDissipation();
	}

    void OpticalSetup()
    {
        nodes = GetComponentsInChildren<Transform>();  // gets all children objects.
        //print(nodes.Length);
        
        // Filters unwanted objects:
        List<Transform> newNodes = new List<Transform>();
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].name[0] == 'n' || nodes[i].name[0] == 'N')
            {
                newNodes.Add(nodes[i]);
            }
            else if(nodes[i].name == "Connections")
            {
                connectionsContainer = nodes[i].gameObject;
            }
        }

        nodes = new Transform[newNodes.Count];
        for(int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = newNodes[i];
        }

        // Setup all connection cables:
        cables = new GameObject[nodes.Length - 1];
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            GameObject cable = Instantiate(cablePrefab) as GameObject;
            cable.GetComponent<CableSetup>().start = nodes[i].gameObject;
            cable.GetComponent<CableSetup>().end = nodes[i+1].gameObject;
            cable.GetComponent<CableSetup>().Setup();
            cable.transform.parent = connectionsContainer.transform;
            cables[i] = cable;
        }
    }

    void ChargePropagation()
    {
        OpticalFiber_Node n = nodes[0].GetComponent<OpticalFiber_Node>();
        CableSetup c = cables[0].GetComponent<CableSetup>();

        c.charge += (n.charge - c.charge) * propagationRate;

        for (int i = 1; i < nodes.Length; i++)
        {
            c = cables[i-1].GetComponent<CableSetup>();
            n = nodes[i].GetComponent<OpticalFiber_Node>();
            
            n.charge += (c.charge - n.charge) * propagationRate;

            if(i < cables.Length)
            {
                c = cables[i].GetComponent<CableSetup>();
                c.charge += (n.charge - c.charge) * propagationRate;
            }
        }


        // PROPAGATION WITHOUT CABLES:
        //for(int i = 0; i < nodes.Length - 1; i++)
        //{
        //    OpticalFiber_Node current = nodes[i].gameObject.GetComponent<OpticalFiber_Node>();
        //    OpticalFiber_Node next = nodes[i+1].gameObject.GetComponent<OpticalFiber_Node>();

        //    next.charge += (current.charge - next.charge) * 0.2f;
        //}
    }

    void ChargeDissipation()
    {
        OpticalFiber_Node node = nodes[0].GetComponent<OpticalFiber_Node>();
        if (!node.isReceivingLight())
        {
            node.charge -= dissipateRate * propagationRate;
            if (node.charge < 0) { node.charge = 0; }
        }
    }

    // Automatically controlls reversibility:
    public void SetClosestNode(Transform player)
    {
        float distance1;
        float distance2;

        if (!reversed)
        {
            distance1 = Vector3.Distance(nodes[0].transform.position, player.position);
            distance2 = Vector3.Distance(nodes[nodes.Length - 1].transform.position, player.position);
        }
        else
        {
            distance2 = Vector3.Distance(nodes[0].transform.position, player.position);
            distance1 = Vector3.Distance(nodes[nodes.Length - 1].transform.position, player.position);
        }

        if(distance1 < distance2) { reversed = false; }
        else { reversed = true; }
    }

    void ReverseNodes()
    {
        Transform[] rev = new Transform[nodes.Length];
        int index = nodes.Length-1;
        for(int i = 0; i < nodes.Length; i++)
        {
            rev[i] = nodes[index];
            index--;
            if(index < 0) { break; }
        }
        nodes = rev;
    }

    void ReverseCables()
    {
        GameObject[] rev = new GameObject[cables.Length];
        int index = cables.Length - 1;
        for (int i = 0; i < cables.Length; i++)
        {
            rev[i] = cables[index];
            index--;
            if(index < 0) { break; }
        }
        cables = rev;
    }
}
