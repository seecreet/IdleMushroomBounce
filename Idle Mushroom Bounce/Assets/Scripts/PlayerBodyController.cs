using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyController : MonoBehaviour
{
    [SerializeField] private GameObject m_playerHead;

    // Update is called once per frame
    void Update()
    {
        // Making the body's neck face towards the player's head.
        Vector3 moveDirection = m_playerHead.transform.position - transform.position;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (transform.parent == null)
        {
            transform.SetParent(m_playerHead.transform);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }   
               
    }
}
