using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform parent; // Référence au GameObject parent
    public Transform enfant; // Référence au GameObject enfant
    void Start()
    {
        if (parent != null && enfant != null)
        {
            // Changer le parent de l'enfant
            enfant.SetParent(parent);
        }
        else
        {
            Debug.LogError("Veuillez attribuer les références parent et enfant dans l'inspecteur Unity.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
