using System;
using System.Collections;
using System.Collections.Generic;
using BoxFall;
using UnityEngine;

public class InteractiveVRItem : MonoBehaviour
{
    public float durationToDestroy = 2f;

    public bool isOver { get; private set; }

    private float time = 0f;

    // Use this for initialization
    private void Start()
    {
        isOver = false;
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnOver()
    {
        this.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.23f, 0.42f, 0.5f, 1.0f));
    }

    private void OnOut()
    {
        this.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 0f, 0f, 1.0f));
    }

    public IEnumerator StartCheckingViewPoint(Camera _camera)
    {
        RaycastHit hit;
        time = 0f;
        isOver = true;
        OnOver();

        while (time < durationToDestroy &&
               Physics.Raycast(new Ray(_camera.transform.position, _camera.transform.forward), out hit))
        {
            if (hit.transform.gameObject != this.gameObject)
            {
                break;
            }

            yield return null;
        }

        if (time >= durationToDestroy)
        {
            Destroy(this.gameObject);
            Data.Instance.Decrease(Constants.CountBoxes, (int)1);
        }

        isOver = false;
        OnOut();
    }
}
