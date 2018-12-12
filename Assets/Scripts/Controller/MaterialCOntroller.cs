using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCOntroller : MonoBehaviour {

    public Material[] material;
    Renderer rend;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rend.sharedMaterial = material[1];
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Q))
       {
            rend.sharedMaterial = material[0];
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            rend.sharedMaterial = material[2];
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            rend.sharedMaterial = material[0];
        }
    }
}
