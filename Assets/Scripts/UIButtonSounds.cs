using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            hoverSound.PlayOneShot(hoverSound.clip);    // audio sound effect will play when mouse is hovered over buttons
        }
    }
}