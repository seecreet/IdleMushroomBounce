using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public string m_mushroomType;
    public float m_growthSpeed;
    public float m_bounceFactor;
    public float m_sporeAmount;

    // Start is called before the first frame update
    void Start()
    {
        switch (m_mushroomType)
        {
            case "Blue":
                break;
            case "Green":
                break;
            case "Yellow":
                break;
            case "Purple":
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
