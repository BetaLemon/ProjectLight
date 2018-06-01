using UnityEngine;

public class GemPickup : MonoBehaviour
{
    //private Rigidbody rigidbody;
    public Transform model;

    //private Vector3 _startPosition; //Serves as a reference for base sinoidal motion animation
    private GameObject player; //PlayerReference
    public float getDraggedRadius = 5.0f; //Radius from which the collectable will start getting absorbed
    public float dragSpeed = 0.2f; //Speed at which the collectable is dragged towards the player on proximity

    public float despawnTime = 25f;
    private float lifeTime = 0.0f;

    //int raycastLayerMask = 1 << 8;
    public float hoverHeight = 1f;

    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();

        //_startPosition = transform.position;
        player = GameObject.FindWithTag("Player");
        if(model == null) { model = transform.GetChild(0); }
        if(model == null) { Debug.LogError("GamePickup is missing model."); }
    }

    void Update()
    {
        if (lifeTime >= despawnTime && despawnTime != -1) //Despawn system
        {
            Destroy(gameObject);
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
            Debug.DrawRay(transform.position, -Vector3.up * hoverHeight, Color.red);
            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, hoverHeight))
            {
                if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("ManaCharge") && !hit.collider.CompareTag("SmallGemstone"))
                {
                    //We apply an ascending force at the hover point position, if the collision hit was detected close the ascending force will be higher:
                    //rigidbody.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), transform.position);
                    //rigidbody.AddForce(Vector3.up * hoverForce);
                    transform.position = new Vector3(hit.point.x, hit.point.y + hoverHeight, hit.point.z);

                    //Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, -1, 0)) * hit.distance, Color.yellow);
                    //Debug.Log("Did Hit");
                }
            }

            model.transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * 4) / 10, transform.position.z); //Sinoidal motion for position (Up and down)


        }
    }
}