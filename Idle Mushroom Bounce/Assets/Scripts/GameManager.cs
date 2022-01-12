using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Camera m_mainCam;

    private Vector2 m_mushroomSpawnPoint;
    private Vector2 m_checkOrigin;

    public GameObject m_blueMushm;
    public GameObject m_visualRadius;

    public float m_checkRadius;

    public bool m_spotFound = false;
    public bool m_placingMushroom = false;

    // Start is called before the first frame update
    void Start()
    {
        m_mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // start placing mushroom, choose size and angle
            if (!m_placingMushroom)
            {
                
                m_placingMushroom = true;
            }
            if (!m_spotFound) 
            {
                m_checkOrigin = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
                m_visualRadius.transform.position = m_checkOrigin;
                RaycastHit2D rayHit = Physics2D.CircleCast(m_checkOrigin, m_checkRadius, Vector2.zero, 0, LayerMask.GetMask("Growable"));
                if (rayHit.point != Vector2.zero)
                {
                    m_spotFound = true;
                    m_mushroomSpawnPoint = rayHit.point;
                    Debug.Log(m_mushroomSpawnPoint);
                }
            }
            else
            {
                
            }
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            // place mushroom
            if (m_placingMushroom && m_spotFound)
            {
                m_spotFound = false;
                m_placingMushroom = false;
                Instantiate(m_blueMushm, m_mushroomSpawnPoint, Quaternion.identity);
            }
        }
    }
}
