using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Vector3 offset;
    Vector3 lastAnimalPos;

    void Start()
    {
        offset = this.transform.position - player.transform.position;
    }

    void Update()
    {
        if (player.IsDie || lastAnimalPos == player.transform.position)
        {
            return;
        }

        var targerAnimalPos = new Vector3(
            player.transform.position.x, 
            0, 
            player.transform.position.z);

        transform.position = targerAnimalPos + offset;
        lastAnimalPos = player.transform.position;
    }
}
