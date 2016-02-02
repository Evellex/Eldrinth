using UnityEngine;
using System.Collections;

public class TestGun : MonoBehaviour
{

    public GameObject prefab;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefab, transform.position, transform.rotation);
        }
    }
}