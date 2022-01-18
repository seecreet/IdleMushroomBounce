using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MushroomGenerator : MonoBehaviour
{
    public GameObject m_mushroomTip;
    public GameObject m_pointsToRender;
    public Mesh m_mushMesh;

    //public SpriteShapeController spriteShapeController;
    //public Spline spline;

    private Camera m_mainCam;
    private MeshFilter m_mushFilter;
    private LineRenderer m_mushLine;

    public List<Vector2> m_mushroomKeyPoints;
    public List<Vector2> m_leftPoints;
    public List<Vector2> m_rightPoints;

    public Vector3[] m_meshPoints;
    public Vector3[] m_flippedBitch;
    public int[] m_meshTriangles;

    public float m_interpolationValue;
    public float m_incrementBetweenPoints;
    public float m_mushroomBaseCoefficient;
    public float m_baseWidthCoefficient;

    public int m_pointCounter;
    public int m_basePoint;
    public int m_meshIndices = 0;

    public GameObject m_blueMushroomCap;
    public GameObject m_currentMushroomCap;
    public bool m_isCapInstantiated;


    // Start is called before the first frame update
    void Start()
    {
        m_isCapInstantiated = false;
        m_mainCam = Camera.main;
        m_mushLine = GetComponent<LineRenderer>();
        m_mushFilter = GetComponent<MeshFilter>();
        m_mushMesh = new Mesh();
        m_mushFilter.mesh = m_mushMesh;

        m_pointCounter = 0;
        m_mushroomKeyPoints = new List<Vector2>();
        m_leftPoints = new List<Vector2>();
        m_rightPoints = new List<Vector2>();
        //spline = spriteShapeController.spline;

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            if (!m_isCapInstantiated)
            {
                m_isCapInstantiated = true;
                m_mushroomTip.transform.position = (Vector2)m_mainCam.ScreenToWorldPoint(Input.mousePosition);
                m_currentMushroomCap = Instantiate(m_blueMushroomCap, m_mushroomTip.transform.position, Quaternion.identity);
            }
          
            m_mushroomTip.transform.position = Vector2.Lerp(m_mushroomTip.transform.position, (Vector2)m_mushroomTip.transform.position + ((Vector2)m_mainCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)m_mushroomTip.transform.position).normalized, m_interpolationValue*Time.deltaTime);

            m_currentMushroomCap.transform.position = m_mushroomTip.transform.position - new Vector3(0,0,m_pointCounter+1);
            if(m_mushroomKeyPoints.Count > 1)
            {
                m_currentMushroomCap.transform.up = (m_mushroomKeyPoints[m_mushroomKeyPoints.Count -1] - m_mushroomKeyPoints[m_mushroomKeyPoints.Count - 2]).normalized;
                m_currentMushroomCap.transform.localScale = Vector2.Lerp(m_currentMushroomCap.transform.localScale, new Vector2((float)m_pointCounter/20, (float)m_pointCounter /20),0.1f);
            }
           
            if (m_pointCounter == 0 || ((Vector2)m_mushroomTip.transform.position - m_mushroomKeyPoints[m_mushroomKeyPoints.Count - 1]).magnitude > m_incrementBetweenPoints)
            {
                m_mushroomKeyPoints.Add(m_mushroomTip.transform.position);
                Instantiate(m_pointsToRender, m_mushroomKeyPoints[m_pointCounter], Quaternion.identity);

                m_mushroomBaseCoefficient *= 1 / (((float)m_pointCounter + 1000)/1000);
                m_basePoint = (int)(GetMushroomLength() * m_mushroomBaseCoefficient) + 1;

                m_leftPoints.Clear();
                m_rightPoints.Clear();
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
                        m_leftPoints.Add(m_mushroomKeyPoints[i] + vertexNormal * factor);
                        m_rightPoints.Add(m_mushroomKeyPoints[i] - vertexNormal * factor);
                    }
                    catch (Exception e)
                    {

                    }
                }

                Array.Resize(ref m_meshPoints, m_mushroomKeyPoints.Count * 2);
                for(int i = 0; i < m_leftPoints.Count; i++)
                {
                    m_meshPoints[i] = (Vector3)m_leftPoints[i] + Vector3.back * i;
                }
                for(int i = 0; i < m_rightPoints.Count; i++)
                {
                    m_meshPoints[m_rightPoints.Count + i] = (Vector3)m_rightPoints[i] + Vector3.back * i;
                }
                Array.Resize(ref m_flippedBitch, m_meshPoints.Length + 1);
                for(int i = 0; i < m_flippedBitch.Length - 1; i++)
                {
                    if(i < m_meshPoints.Length / 2)
                    {
                        Debug.Log("FLIP ONE FUCKER 111111 ;;2;: " + i);
                        m_flippedBitch[i] = m_meshPoints[i] + Vector3.back * 0.1f;
                    }
                    else
                    {
                        Debug.Log("TOWO2O222OWWWO22O2O22O2WOWWWWOWOO" + i);
                        m_flippedBitch[i] = m_meshPoints[m_meshPoints.Length - (i - m_meshPoints.Length / 2 + 1)] + Vector3.back * 0.1f;
                    }
                }
                m_flippedBitch[m_flippedBitch.Length - 1] = m_flippedBitch[0];
                m_mushLine.positionCount = m_flippedBitch.Length;
                m_mushLine.SetPositions(m_flippedBitch);
                
                Array.Resize(ref m_meshTriangles, (m_meshPoints.Length - 2)*3);
                m_meshIndices = 0;
                for(int i = 0; i < m_mushroomKeyPoints.Count; i++)
                {
                    if(i != 0)
                    {
                        m_meshTriangles[m_meshIndices] = i;
                        m_meshIndices++;
                        m_meshTriangles[m_meshIndices] = i + m_mushroomKeyPoints.Count;
                        m_meshIndices++;
                        m_meshTriangles[m_meshIndices] = i + m_mushroomKeyPoints.Count - 1;
                        m_meshIndices++;
                    }
                    Debug.Log(m_meshIndices);
                    if(i != m_mushroomKeyPoints.Count - 1)
                    {
                        m_meshTriangles[m_meshIndices] = i + 1;
                        m_meshIndices++;
                        m_meshTriangles[m_meshIndices] = i + m_mushroomKeyPoints.Count;
                        m_meshIndices++;
                        m_meshTriangles[m_meshIndices] = i;
                        m_meshIndices++;
                    }
                    
                }
                m_mushMesh.vertices = m_meshPoints;
                m_mushMesh.triangles = m_meshTriangles;


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
