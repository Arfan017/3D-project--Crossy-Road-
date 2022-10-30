using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // static membuat variabel ini shared pada semua Tree
    public static List<Vector3> AllPositions = new List<Vector3>();

    public void OnEnable(){
        AllPositions.Add(this.transform.position);
        Debug.Log(AllPositions.Count);
    }

    public void OnDisable(){
        AllPositions.Remove(this.transform.position);
    }
}
