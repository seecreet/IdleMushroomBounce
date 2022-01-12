using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MushroomGenerator : MonoBehaviour
{
    public GameObject m_mushroomTip;
    public GameObject m_pointsToRender;
    public SpriteShapeController spriteShapeController;
    public Spline spline;

    private Camera m_mainCam;

    public List<Vector2> m_mushroomKeyPoints;
    public List<Vector2> m_leftPoints;
    public List<Vector2> m_rightPoints;

    public float m_interpolationValue;
    public float m_incrementBetweenPoints;
    public float m_mushroomBaseCoefficient;
    public float m_baseWidthCoefficient;

    public int m_pointCounter;
    public int m_basePoint;
    

    // Start is called before the first frame update
    void Start()
    {
        m_mainCam = Camera.main;
        m_pointCounter = 0;
        m_mushroomKeyPoints = new List<Vector2>();
        m_leftPoints = new List<Vector2>();
        m_rightPoints = new List<Vector2>();
        spline = spriteShapeController.spline;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            m_mushroomTip.transform.position = Vector2.Lerp(m_mushroomTip.transform.position, (Vector2)m_mushroomTip.transform.position + ((Vector2)m_mainCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)m_mushroomTip.transform.position).normalized, m_interpolationValue);

            if(m_pointCounter == 0 || ((Vector2)m_mushroomTip.transform.position - m_mushroomKeyPoints[m_mushroomKeyPoints.Count - 1]).magnitude > m_incrementBetweenPoints)
            {
                m_mushroomKeyPoints.Add(m_mushroomTip.transform.position);
                Instantiate(m_pointsToRender, m_mushroomKeyPoints[m_pointCounter], Quaternion.identity);

                m_mushroomBaseCoefficient *= 1 / (((float)m_pointCounter + 1000)/1000);
                m_basePoint = (int)(GetMushroomLength() * m_mushroomBaseCoefficient) + 1;

                spline.Clear();
                for (int i = 0; i < 2 * m_mushroomKeyPoints.Count - 1; i++) spline.InsertPointAt(i, (Vector3)m_mushroomKeyPoints[0] + new Vector3(0,0,i));
                for(int i = 0; i < m_mushroomKeyPoints.Count; i++)
                {
                    Vector2 vertexNormal = GetVertexNormal(i);
                    float j = i + 1;
                    try
                    {
                        float factor;
                        if(j <= m_basePoint)
                        {
                            factor = (j / (float)m_basePoint) * m_baseWidthCoefficient * (float)Math.Sqrt(m_pointCounter)/3;
                        }
                        else
                        {
                            factor = (((float)m_mushroomKeyPoints.Count - j) / ((float)m_mushroomKeyPoints.Count - (float)m_basePoint)) * m_baseWidthCoefficient * (float)Math.Sqrt(m_pointCounter)/3;
                        }
                        Debug.Log(factor);
                        spline.SetPosition(i, m_mushroomKeyPoints[i] + vertexNormal * factor);
                        spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                        spline.SetPosition(2 * m_mushroomKeyPoints.Count - 2 - i, m_mushroomKeyPoints[i] - vertexNormal * factor);
                        spline.SetTangentMode(2 * m_mushroomKeyPoints.Count - 2 - i, ShapeTangentMode.Continuous);
                    }
                    catch (Exception e)
                    {

                    }
                }
                
                m_pointCounter++;
            }
        }
    }

    private float GetMushroomLength()
    {
        return (m_pointCounter - 1) * m_incrementBetweenPoints;
    }

    private Vector2 GetVertexNormal(int vertexIndex)
    {
        try
        {
            if (vertexIndex == 0)
            {
                if (m_mushroomKeyPoints[vertexIndex + 1] != null)
                {
                    Vector2 vectorTwo = m_mushroomKeyPoints[vertexIndex + 1] - m_mushroomKeyPoints[vertexIndex];
                    return Vector2.Perpendicular(vectorTwo).normalized;
                }
                else return Vector2.zero;
            }
            else if (vertexIndex == m_mushroomKeyPoints.Count - 1)
            {
                Vector2 vectorOne = m_mushroomKeyPoints[vertexIndex] - m_mushroomKeyPoints[vertexIndex - 1];
                return Vector2.Perpendicular(vectorOne).normalized;
            }
            else
            {
                Vector2 vectorOne = m_mushroomKeyPoints[vertexIndex] - m_mushroomKeyPoints[vertexIndex - 1];
                Vector2 vectorTwo = m_mushroomKeyPoints[vertexIndex + 1] - m_mushroomKeyPoints[vertexIndex];
                return Vector2.Perpendicular(vectorOne + vectorTwo).normalized;
            }
        }
        catch (Exception e)
        {
            return Vector2.zero;
        }
    }
}
