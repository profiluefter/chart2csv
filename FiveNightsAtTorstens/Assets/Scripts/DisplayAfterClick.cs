using System;
using UnityEngine;

public class DisplayAfterClick : MonoBehaviour
{
    public GameObject target;
    public GameObject after;

    private void Awake()
    {
        target.SetActive(false);
        after.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        if(target.activeSelf)
            after.SetActive(target.activeSelf);
        target.SetActive(!target.activeSelf);
    }
}
