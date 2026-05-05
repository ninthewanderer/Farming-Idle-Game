using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator animator;
    public AnimationClip clipToPlay;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play(clipToPlay.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            gameObject.SetActive(false);
        }
    }
}
