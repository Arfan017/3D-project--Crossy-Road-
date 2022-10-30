using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    //inisialisasi durasi perpindahan dan tinggi lompatan
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.5f;
    [SerializeField] AudioSource JumpAudio;
    [SerializeField] AudioSource CrashAudio;
    [SerializeField] AudioSource Bubble;


    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    //baru
    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel;}
    [SerializeField] public int currentTravel;
    public int CurrentTravel { get => currentTravel;}
    public bool IsDie {get => this.enabled == false;}
    public void SetUp(int minZPos, int extent){
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    //menerima inputan 
    void Update()
    {
        var moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0, 1);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0, -1);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1, 0, 0);
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
        }

        if (moveDir != Vector3.zero && IsJumping() == false)
        {
            Jump(moveDir);
        }
    }

    //lompat
    private void Jump(Vector3 targetDirection)
    {
        JumpAudio.Play();
        //atur posisi
        var targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);
        
        //loncat ke atas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

        if  (targetPosition.z <= backBoundary ||
            targetPosition.x <= leftBoundary ||
            targetPosition.x >= rightBoundary)
            return;

        if(Tree.AllPositions.Contains(targetPosition))
        {
            return;
        }
        
        //gerak maju/mundur/samping
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform
            .DOMoveZ(targetPosition.z, moveDuration)
            .OnComplete(UpdateTravel);
    }

    //baru
    private void UpdateTravel()
    {
        currentTravel = (int) this.transform.position.z;

        if(currentTravel > maxTravel)
            maxTravel = currentTravel;

        stepText.text = "STEP: " + maxTravel.ToString();      
    }

    //mengecek lompat
    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other){
        if (this.enabled == false)
        {
            return;
        }

        if (other.tag == "Car")
        {
            CrashAudio.Play();
            AnimateCrash();
        }
    }

    private void AnimateCrash()
    {
        //gepeng
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(1, 0.2f);
        transform.DOScaleZ(1, 0.2f);
        this.enabled = false;
        dieParticles.Play();
        Bubble.Play();
    }

    private void OnTriggerStay(Collider other){
        //di execute setiap frame selama masih nempel
    }

    private void OnTriggerExit(Collider other){
        //di execute sekali pada frame ketika tidak nempel
    }
}
