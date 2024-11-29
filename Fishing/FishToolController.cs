using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishToolController : MonoBehaviour
{
    Animator animator;

    public bool isWaitingForFish;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeFishingBool()
    {
        isWaitingForFish = !isWaitingForFish;
        animator.SetBool("isWaitingForFish", isWaitingForFish);
    }
}
