using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoalCrystal : MonoBehaviour{
    public bool isActivated = false;
    public AudioSource goalSound;
    public float nextLevelDelay = 1f;

    private SpriteRenderer sr;
    private bool soundPlayed = false;
    private bool levelLoading = false;

    void Start(){
        sr = GetComponent<SpriteRenderer>();
    }

    public void Activate(){
        isActivated = true;
        sr.color = Color.purple;

        // audio clip will play when the goal activates
        if (!soundPlayed && goalSound != null){
            goalSound.Play();
            soundPlayed = true;
        }

        // next level will play 
        if (!levelLoading){
            levelLoading = true;
            StartCoroutine(LoadNextLevel());
        }
    }

    public void Deactivate(){
        if (levelLoading) return;
        
        isActivated = false;
        sr.color = Color.orange;
    }

  IEnumerator LoadNextLevel(){
    yield return new WaitForSeconds(nextLevelDelay);

    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    // if next scene exists → go to it
    if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings){
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    else{
        // otherwise go back to Main Menu (index 0)
        SceneManager.LoadScene(0);
    }
    }
}