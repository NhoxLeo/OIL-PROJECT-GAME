using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexGrid : MonoBehaviour {
    public GameObject container;
    [SerializeField]
    public int elements;

	void Start () {
		
	}

	void Update () {
        //float width = container.GetComponent<RectTransform>().rect.width;
        //Vector2 newSize = new Vector2(width / elements, width / 50);
        //container.GetComponent<GridLayoutGroup>().cellSize = newSize;
	}
}
