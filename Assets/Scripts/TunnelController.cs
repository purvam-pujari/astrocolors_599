﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelController : MonoBehaviour {
    public float damping = 30;
    float desiredRot;
    int curVertex = 0;
    private float rotationTime;
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    Vector2 currentSwipe;
    public GameObject gm;
    private GameMaster gmScript;

    private void Start()
    {
        gmScript = gm.GetComponent<GameMaster>();
    }

    // Use this for initialization
    private void OnEnable()
    {
        desiredRot = curVertex * 60;
    }

    // Update is called once per frame
    private void Update()
    {
        if( !gmScript.IsGameOver() )
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                desiredRot = curVertex * 60 - 60f;
                curVertex--;
                rotationTime = 0;
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                desiredRot = curVertex * 60 + 60f;
                curVertex++;
                rotationTime = 0;
            }

            // Touch
            if(Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if(t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point
                    fingerDown = new Vector2(t.position.x, t.position.y);
                }
                if(t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point
                    fingerUp = new Vector2(t.position.x, t.position.y);             
                    if(currentSwipe.x < 0 && (currentSwipe.y > -0.5f || currentSwipe.y < 0.5f))
                    {
                        // Debug.Log("left swipe");
                        desiredRot = curVertex * 60 - 60f;
                        curVertex--;
                        rotationTime = 0;
                    }
                    //swipe right
                    if(currentSwipe.x > 0 && (currentSwipe.y > -0.5f || currentSwipe.y < 0.5f))
                    {
                        // Debug.Log("right swipe");
                        desiredRot = curVertex * 60 + 60f;
                        curVertex++;
                        rotationTime = 0;
                    }
                }
            }

            // Mouse swipe
            // if(Input.GetMouseButtonDown(0)){
            //     fingerDown = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // }

            // if(Input.GetMouseButtonUp(0)){
            //     fingerUp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //     currentSwipe = new Vector2(fingerUp.x - fingerDown.x, fingerUp.y - fingerDown.y);
            //     //swipe left
            //     if(currentSwipe.x < 0 && (currentSwipe.y > -0.5f || currentSwipe.y < 0.5f))
            //     {
            //         // Debug.Log("left swipe");
            //         desiredRot = curVertex * 60 - 60f;
            //         curVertex--;
            //         rotationTime = 0;
            //     }
            //     //swipe right
            //     if(currentSwipe.x > 0 && (currentSwipe.y > -0.5f || currentSwipe.y < 0.5f))
            //     {
            //         // Debug.Log("right swipe");
            //         desiredRot = curVertex * 60 + 60f;
            //         curVertex++;
            //         rotationTime = 0;
            //     }
            // }

            curVertex = curVertex % 6;
            Quaternion desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, desiredRot);

            rotationTime += Time.deltaTime * damping;
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotQ, rotationTime);
        }
    }
}
