using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public float wait;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public AudioSource musicSource;
    public AudioClip clip1;
    public AudioClip clip2;

    private float nextFire;

    private Rigidbody rb;

    public GameObject playerExplosion;
    public Material material;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            // GameObject clone = 
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation); // as GameObject;
            musicSource.clip = clip1;
            musicSource.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Green")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            speed = 20;
            material.color = Color.green;
            musicSource.clip = clip2;
            musicSource.Play();
            Destroy(other.gameObject);
            StartCoroutine(Wait());          
        }

        if (other.tag == "Red")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            fireRate = 0.1f;
            material.color = Color.red;
            musicSource.clip = clip2;
            musicSource.Play();
            Destroy(other.gameObject);
            StartCoroutine(Wait2());
        }
    }

    IEnumerator Wait()
    {
            yield return new WaitForSeconds(3.0f);
            speed = 10;
            material.color = Color.white;
            StopCoroutine(Wait());
    }

    IEnumerator Wait2()
    {
        yield return new WaitForSeconds(3.0f);
        fireRate = 0.25f;
        material.color = Color.white;
        StopCoroutine(Wait2());
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
             Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
             0.0f,
             Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}