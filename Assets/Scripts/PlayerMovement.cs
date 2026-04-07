using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float interactRange = 1.5f;
    public float mirrorRotateSpeed = 100f;
    public AudioSource deathSound;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer sr;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        GameObject nearestMirror = FindNearestMirror();

        if (Input.GetKey(KeyCode.E) && nearestMirror != null)
        {
            nearestMirror.transform.Rotate(0f, 0f, mirrorRotateSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q) && nearestMirror != null)
        {
            nearestMirror.transform.Rotate(0f, 0f, -mirrorRotateSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement.normalized * speed;
    }

    GameObject FindNearestMirror()
    {
        GameObject[] mirrors = GameObject.FindGameObjectsWithTag("Mirror");

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject mirror in mirrors)
        {
            float dist = Vector2.Distance(transform.position, mirror.transform.position);

            if (dist < interactRange && dist < minDist)
            {
                minDist = dist;
                closest = mirror;
            }
        }

        return closest;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        sr.color = Color.red;
        rb.linearVelocity = Vector2.zero;

        if (deathSound != null)
        {
            deathSound.Play();
        }

        Invoke("RestartLevel", 1.5f);
    }

    void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}