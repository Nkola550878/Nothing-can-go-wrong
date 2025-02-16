using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject player;

    void Start()
    {
        Instantiate(player, transform.position + offset, Quaternion.identity);
    }
}
