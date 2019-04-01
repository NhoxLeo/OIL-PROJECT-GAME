using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{

    public float destroyTime = 3f;
    public GameObject floatingTextPrefab;

    public void AddObject()
    {
        GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        floatingText.transform.parent = gameObject.transform;
    }

    public void DestroyObject()
    {
        Destroy(gameObject, destroyTime);
    }
}
