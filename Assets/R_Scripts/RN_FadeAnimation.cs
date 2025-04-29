using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RN_FadeAnimation : MonoBehaviour
{
    private Animator animator;
    private string sceneName;
    private GameObject BookAnimationW1;
    private GameObject BookAnimationW2;
    private GameObject BookAnimationW3;
    
    void Awake()
    {
        BookAnimationW1 = GameObject.Find("BookAnimationW1");
        BookAnimationW2 = GameObject.Find("BookAnimationW2");
        BookAnimationW3 = GameObject.Find("BookAnimationW3");
        InitializeEvents();
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeEvents()
    {
        BookAnimationW1.TryGetComponent<RN_BookClicked>(out RN_BookClicked T);
        BookAnimationW2.TryGetComponent<RN_BookClicked>(out RN_BookClicked R);
        BookAnimationW3.TryGetComponent<RN_BookClicked>(out RN_BookClicked C);
        try
        {
            T.OnBookClicked -= FadeToLevel;
            T.OnBookClicked += FadeToLevel;
        }
        catch
        {

        }

        try
        {
            R.OnBookClicked -= FadeToLevel;
            R.OnBookClicked += FadeToLevel;
        }
        catch
        {

        }

        try
        {
            C.OnBookClicked -= FadeToLevel;
            C.OnBookClicked += FadeToLevel;
        }
        catch
        {
            
        }
    }

    public void FadeToLevel(string scene)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("FadeOut");
        sceneName = scene;
    }

    public void OnFadeComplete()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return null;

        SceneManager.LoadScene(sceneName);
    }
}
