using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    private Animator animator;
    private bool open;
    void Start()
    {
        animator = GetComponent<Animator>();
        open = animator.GetBool("Open"); //estado de un parámetro
    }
    public void Operate() 
    {
        open = !open;
        animator.SetBool("Open", open);
        Debug.Log("ZZ");
    }

    public void Deactivate()
    {
        if (open) Operate();
        Debug.Log("cierrate sesamo");
    }
    public void Activate()
    {
        if (!open) Operate();
        Debug.Log("abrete sesamo");

    }
}