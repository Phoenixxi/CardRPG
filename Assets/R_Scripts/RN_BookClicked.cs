using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RN_BookClicked : MonoBehaviour
{
    private Animator animator;
    private bool animating = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        //Debug.Log(gameObject.name);
        //Check if the ONLY world 1 is unlocked
        if(MapManager.Instance == null)
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
            animator.SetTrigger("BookClicked");
            StartCoroutine(WaitForAnimationEnd("Map2", "World1"));
            return;
        }

        if(worldID == 2 && gameObject.name == "BookAnimationW3" && !animating)
        {
            animating = true;
            animator.SetTrigger("BookClicked");
            StartCoroutine(WaitForAnimationEnd("Map3"));
            return;
        }
    }

    IEnumerator WaitForAnimationEnd(string scene)
    {
        yield return new WaitForSeconds(5);
        animating = false;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        if(scene == "Map2")
        {

            GameObject tempParent = new GameObject("TemporarySceneObjects");
            foreach(var obj in allObjects)
            {
                if(obj.name == "World1")
                {
                    foreach(Transform child in obj.transform)
                    {
                        child.SetParent(tempParent.transform);
                    }
                    Destroy(obj);
                }
                
            }
        }

        if(scene == "Map3")
        {
            GameObject tempParent = new GameObject("TemporarySceneObjects");
            foreach(var obj in allObjects)
            {
                if(obj.name == "World2")
                {
                    foreach(Transform child in obj.transform)
                    {
                        child.SetParent(tempParent.transform);
                    }
                    Destroy(obj);
                }
                
            }
        }
        SceneManager.LoadScene(scene);
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
        animating = false;

        // Load the new scene
        SceneManager.LoadScene(scene);

        // Now, destroy the world object after a delay
        // yield return new WaitForSeconds(1); // Optional delay to ensure scene load happens first
    }

}
