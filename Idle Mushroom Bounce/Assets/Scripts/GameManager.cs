using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_mushroomTip;

    private Camera m_mainCam;

    private Vector2 m_mushroomSpawnPoint;
    private Vector2 m_checkOrigin;

    public GameObject m_blueMushm;
    public GameObject m_visualRadius;

    public float m_checkRadius;
    public bool m_placingMushroom;

    public MushroomGenerator m_mushGen;
    public bool m_isNewMushroom;


    // Start is called before the first frame update
    void Start()
    {
        m_isNewMushroom = true;
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
                m_checkOrigin = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
                m_visualRadius.transform.position = m_checkOrigin;
                RaycastHit2D rayHit = Physics2D.CircleCast(m_checkOrigin, m_checkRadius, Vector2.zero, 0, LayerMask.GetMask("Growable"));
                if (rayHit.point != Vector2.zero)
                {
                    m_placingMushroom = true;
                    m_mushroomTip.transform.position = rayHit.point;
                    Debug.Log(m_mushroomSpawnPoint);
                }
            }
            else
            {
                m_mushGen.GenerateMushroom(m_isNewMushroom);
                m_isNewMushroom = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_isNewMushroom = true;
            m_placingMushroom = false;
        }
    }
}
