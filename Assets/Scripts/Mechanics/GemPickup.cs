using UnityEngine;

public class GemPickup : MonoBehaviour
{
    private Rigidbody rigidbody;

    //private Vector3 _startPosition; //Serves as a reference for base sinoidal motion animation
    private GameObject player; //PlayerReference
    public float getDraggedRadius = 5.0f; //Radius from which the collectable will start getting absorbed
    public float dragSpeed = 0.2f; //Speed at which the collectable is dragged towards the player on proximity

    private float despawnTime;
    private float lifeTime = 0.0f;

    //int raycastLayerMask = 1 << 8;
    float hoverHeight = 2.5f;
    float hoverForce = 10.0f;

    void Start()
    {
        despawnTime = Time.deltaTime * 100;

        rigidbody = GetComponent<Rigidbody>();

        //_startPosition = transform.position;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (lifeTime >= despawnTime) //Despawn system
        {
            Destroy(this);
        }

        //Check if collectable tag is ManaCharge or SmallGemstone
        if (tag == "ManaCharge" || tag == "SmallGemstone")
        {
            lifeTime += Time.deltaTime;

            float currentDistance = Vector3.Distance(transform.position, Player.instance.transform.position);
            if (currentDistance < getDraggedRadius)
            {
                // The thing moves towards the player:
                transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position, dragSpeed / currentDistance);
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, hoverHeight/*, raycastLayerMask*/))
            {
                //We apply an ascending force at the hover point position, if the collision hit was detected close the ascending force will be higher:
                rigidbody.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), transform.position);

                Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, -1, 0)) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }

            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * 4) / 10, transform.position.z); //Sinoidal motion for position (Up and down)


        }
    }
}