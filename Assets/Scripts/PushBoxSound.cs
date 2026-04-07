using UnityEngine;

public class PushBoxSound : MonoBehaviour
{
    public AudioSource pushSound;
    private bool canPlay = true;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && pushSound != null && canPlay)
        {
            pushSound.PlayOneShot(pushSound.clip);
            canPlay = false;
            Invoke(nameof(ResetSound), 0.2f);
        }
    }

    void ResetSound()
    {
        canPlay = true;
    }
}