using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RN_BookClicked : MonoBehaviour
{
    private Animator animator;
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
            if(gameObject.name == "BookAnimationW1")
            {
                animator.SetTrigger("BookClicked");
                StartCoroutine(WaitForAnimationEnd("Jared"));
            }
            return;
        }

        NodeController activeNode = MapManager.Instance.nodes[0].sendCurrentNode();
        int worldID = activeNode.thisWorld;

        if(worldID == 1 && gameObject.name == "BookAnimationW2")
        {
            animator.SetTrigger("BookClicked");
            StartCoroutine(WaitForAnimationEnd("Map2"));
            return;
        }

        if(worldID == 2 && gameObject.name == "BookAnimationW3")
        {
            animator.SetTrigger("BookClicked");
            StartCoroutine(WaitForAnimationEnd("Map3"));
            return;
        }
    }

    IEnumerator WaitForAnimationEnd(string scene)
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(scene);
    }
}
