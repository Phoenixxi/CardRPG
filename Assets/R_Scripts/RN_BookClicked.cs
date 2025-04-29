using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class RN_BookClicked : MonoBehaviour
{
    private Animator animator;
    private bool animating = false;

    public delegate void BookClicked(string sceneName);
    public event BookClicked OnBookClicked;
    [SerializeField]private GameObject BookParticles;
    private int w = 0;

    void Awake()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        PlaySparkleAnimation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        //Debug.Log(gameObject.name);
        //Check if the ONLY world 1 is unlocked
        if(MapManager.Instance == null && w == 0)
        {
            if(gameObject.name == "BookAnimationW1" && !animating)
            {
                animating = true;
                animator.SetTrigger("BookClicked");
                StartCoroutine(WaitForAnimationEnd("Jared"));
            }
            return;
        }

        int worldID = MapManager.Instance.GetCurrentWorld();

        if(worldID == 1 && gameObject.name == "BookAnimationW2" && !animating)
        {
            animating = true;
            w = 1;
            animator.SetTrigger("BookClicked");
            StartCoroutine(WaitForAnimationEnd("Map2", "World1"));
            return;
        }

        if(worldID == 2 && gameObject.name == "BookAnimationW3" && !animating)
        {
            animating = true;
            w = 2;
            animator.SetTrigger("BookClicked");
            StartCoroutine(WaitForAnimationEnd("Map3", "World2"));
            return;
        }
    }

    IEnumerator WaitForAnimationEnd(string scene)
    {
        yield return new WaitForSeconds(5);
        Destroy(BookParticles);
        OnBookClicked?.Invoke(scene);
    }
        IEnumerator WaitForAnimationEnd(string scene, string worldName)
    {
        // Destroy MapManager instance
        if (MapManager.Instance != null)
        {
            // Deactivate and destroy the instance, if necessary
            Destroy(MapManager.Instance.gameObject);  // Assuming MapManager has a GameObject associated with it
            // MapManager.Instance = null;  // Optionally null out the instance reference
        }

        // Wait for the animation to finish
        yield return new WaitForSeconds(5);
        Destroy(BookParticles);
        OnBookClicked?.Invoke(scene);

        // Now, destroy the world object after a delay
        // yield return new WaitForSeconds(1); // Optional delay to ensure scene load happens first
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        animating = false;
    }

    private void PlaySparkleAnimation()
    {
        if(!checkWorld())
        {
            return;
        }

        BookParticles.gameObject.SetActive(true);
    }

    private bool checkWorld()
    {
        int worldID = 0;

        if(MapManager.Instance == null)
        {
            worldID = 0;
        }
        else
        {
            worldID = MapManager.Instance.GetCurrentWorld();
        }

        switch(worldID)
        {
            case 0:
                return gameObject.name == "BookAnimationW1";
            
            case 1:
                return gameObject.name == "BookAnimationW2";

            case 2:
                return gameObject.name == "BookAnimationW3";
        }
        //should never reach this point
        return false;
    }

}
