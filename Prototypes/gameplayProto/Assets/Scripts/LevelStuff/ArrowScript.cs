using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour
{
    public float ShushDelay = 2F;
    private Rigidbody physics;
    private bool isSleeping = false;
    public float Movespeed = 5.0f;

    [HideInInspector]
    public ArrowSpawner spawnParent;

    [HideInInspector]
    public Vector3 FlyDirection;

    private Vector3 velocity; //The saved velocity used when awakening the object

    // Use this for initialization
    void Start()
    {
        physics = GetComponent<Rigidbody>();
        physics.velocity = FlyDirection * Movespeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSleeping)
            physics.velocity = FlyDirection * Movespeed;
    }

    void FixedUpdate()
    {
    }

    IEnumerator Rewake()
    {
        Debug.Log("Sleep");
        if (!isSleeping)
            velocity = physics.velocity;
        yield return new WaitForSeconds(ShushDelay);
        Debug.Log("Awake");
        isSleeping = false;
        physics.velocity = velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isSleeping)
        {
            if (collision.gameObject.tag == GlobalNames.TAG.Player)
            {
                collision.gameObject.GetComponent<Player>().Reset();
                spawnParent.ChildDeath(this);
                GameObject.Destroy(transform.gameObject);
            }
            else
            {
                spawnParent.ChildDeath(this);
                GameObject.Destroy(transform.gameObject);

            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == GlobalNames.TAG.ShushTrigger)
        {
            Debug.Log("Freeze");
            StartCoroutine(Rewake());
            isSleeping = true;
            gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}