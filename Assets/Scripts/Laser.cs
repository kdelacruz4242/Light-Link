using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public Transform startPoint;
    public float maxDistance = 20f;
    public int maxBounces = 5;
    public AudioSource mirrorBounceSound;
    private LineRenderer lr;
    private Collider2D lastMirrorHit;

    void Awake()
    {
        // the laser itselt is a LineRenderer
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (startPoint == null) return;


        // initialize laser starting position and direction
        Vector2 currentPos = startPoint.position;
        Vector2 direction = Vector2.right;

        // initialize LineRenderer with the starting point
        lr.positionCount = 1;
        lr.SetPosition(0, currentPos);

        GoalCrystal[] goals = FindObjectsOfType<GoalCrystal>();

        // reset all goals to inactive at the start of each frame
        foreach (GoalCrystal goal in goals)
        {
            goal.Deactivate();
        }

        for (int i = 0; i < maxBounces; i++)
        {
             // how the laser reflects off the mirror in realistic way 
            RaycastHit2D hit = Physics2D.Raycast(currentPos, direction, maxDistance);

            if (hit.collider != null)
            {
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, hit.point);

                // if player hits that laser, the player dies
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PlayerMovement>().Die();
                    return;
                }

                GoalCrystal goal = hit.collider.GetComponent<GoalCrystal>();
                if (goal != null)
                {
                    goal.Activate();
                    break;
                }

                // when the laser reaches a mirror the line will bounce off of it 
                if (hit.collider.CompareTag("Mirror"))
                {
                    if (mirrorBounceSound != null && hit.collider != lastMirrorHit)
                    {
                        mirrorBounceSound.Play();
                    }

                    lastMirrorHit = hit.collider;

                    direction = Vector2.Reflect(direction, hit.normal).normalized;
                    currentPos = hit.point + direction * 0.01f;
                }
                else
                {
                    lastMirrorHit = null;
                    break;
                }
            }
            else
            {
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, currentPos + direction * maxDistance);
                lastMirrorHit = null;
                break;
            }
        }
    }
}