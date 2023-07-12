using System.Collections.Generic;
using UnityEngine;
using SensorEmulatorLib;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ArrowGenerator : MonoBehaviour
{
    //public Vector3 startPoint; // Start point of the arrow
    //public Vector3 endPoint; // End point of the arrow
    public float arrowHeadLength = 0.25f; // Length of the arrowhead

    public GameObject arrowPrefab; // Prefab of the arrow object


    float _maxXPos;
    float _maxYPos;
    float _maxZPos;

    public Terrain terrain;
    public float stemLength;
    public float stemWidth;
    public float tipLength;
    public float tipWidth;

    [System.NonSerialized]
    public List<Vector3> verticesList;
    [System.NonSerialized]
    public List<int> trianglesList;

    Mesh mesh;

    Sensor sensor=new Sensor();
    void Start()
    {
        //make sure Mesh Renderer has a material
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
        this.stemLength = 5;
        this.stemWidth = 2;
        this.tipLength = 4;
        this.tipWidth = 5;
        sensor.ResetPosition(-0.05520356f,0.7006978f,-0.4371091f, -0.05520356f, 0.7006978f, -0.4371091f);
        GenerateArrow();

        _maxXPos= 2;
        _maxYPos = 4;
        _maxZPos = 2;

        arrowPrefab.GetComponent<Renderer>().material.color = new Color(0, 204, 0);
    }

    void Update()
    {
        
        //transform.rotation = Random.rotation;
        ChangePositionOnBasisofSensor();
    }
    void ChangePositionOnBasisofSensor()
    {


        float PositionX, PositionY, PositionZ, RotationX, RotationY, RotationZ;
        int ForceR, ForceG,ForceB;
        sensor.GetSensorsData(out PositionX,out PositionY,out PositionZ,out RotationX,out RotationY,out RotationZ,out ForceR,out ForceG,out ForceB);
        if (PositionX >= _maxXPos || PositionY >= _maxYPos || PositionZ >= _maxZPos)
        {
            sensor.ResetPosition(-0.05520356f, 0.7006978f, -0.4371091f, -0.05520356f, 0.7006978f, -0.4371091f);
            sensor.GetSensorsData(out PositionX, out PositionY, out PositionZ, out RotationX, out RotationY, out RotationZ, out ForceR, out ForceG, out ForceB);

        }
            Vector3 startPoint = new Vector3(PositionX, PositionY, PositionZ);
            transform.position = startPoint;
            //var Rotation = Random.rotation;
            //var StringMessage = System.Environment.NewLine + "X:" + PositionX.ToString() + " Y:" + PositionY.ToString() + " Z:" + PositionZ.ToString() + " RoationX:" + RotationX.ToString() + " RoationY:" + RotationY.ToString() + " RoationZ:" + RotationZ.ToString();
            //System.IO.File.AppendAllText("C://test//unity.txt", StringMessage);
            transform.rotation = Quaternion.Euler(RotationX, RotationY, RotationZ);//Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
            arrowPrefab.GetComponent<Renderer>().material.color = new Color(ForceR, ForceG, ForceB);
        

    }
    //arrow is generated starting at Vector3.zero
    //arrow is generated facing right, towards radian 0.
    void GenerateArrow()
    {
        //setup
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();

        //stem setup
        Vector3 stemOrigin = Vector3.zero;
        float stemHalfWidth = stemWidth / 2f;
        //Stem points
        verticesList.Add(stemOrigin + (stemHalfWidth * Vector3.down));
        verticesList.Add(stemOrigin + (stemHalfWidth * Vector3.up));
        verticesList.Add(verticesList[0] + (stemLength * Vector3.right));
        verticesList.Add(verticesList[1] + (stemLength * Vector3.right));

        //Stem triangles
        trianglesList.Add(0);
        trianglesList.Add(1);
        trianglesList.Add(3);

        trianglesList.Add(0);
        trianglesList.Add(3);
        trianglesList.Add(2);

        //tip setup
        Vector3 tipOrigin = stemLength * Vector3.right;
        float tipHalfWidth = tipWidth / 2;

        //tip points
        verticesList.Add(tipOrigin + (tipHalfWidth * Vector3.up));
        verticesList.Add(tipOrigin + (tipHalfWidth * Vector3.down));
        verticesList.Add(tipOrigin + (tipLength * Vector3.right));

        //tip triangle
        trianglesList.Add(4);
        trianglesList.Add(6);
        trianglesList.Add(5);

        //assign lists to mesh.
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
    }
}