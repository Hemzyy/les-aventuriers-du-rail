using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestI : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField i;
    void Start()
    {
         i = GetComponent<InputField>();

        if (i != null)
        {
            Debug.Log("InputField trouvé !");
        }
        else
        {
            Debug.LogError("InputField non trouvé !");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
