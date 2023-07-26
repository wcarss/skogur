using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaker : MonoBehaviour
{
    List<GameObject> AllCubes;
    List<Vector2> CubeLocations;
    int t = 0;
    public TextAsset myTextAsset;

    // Start is called before the first frame update
    void Start()
    {
        AllCubes = new List<GameObject>();
        CubeLocations = new List<Vector2>();
        Color[] colors = { Color.green, Color.yellow, Color.blue };

        // Split the text into an array of strings, cutting wherever there's a tab.
        string[] fileLines = myTextAsset.text.Split('\n');
        string[] dimensionsStrings = fileLines[0].Split(',');
        Vector2Int dimensions = new Vector2Int(
            int.Parse(dimensionsStrings[0]),
            int.Parse(dimensionsStrings[1])
        );

        Debug.Log(dimensions.x + "," + dimensions.y);
        // Prepare a float array of the same size.
        float[] heightMap = new float[dimensions.x * dimensions.y];
        //List<List<float>> heightMap = new List<List<float>>((int)dimensions.y);
        for (int i = 0; i < dimensions.y; i++)
        {
            string[] heightStrings = fileLines[i+1].Split(',');
            for (int j = 0; j < dimensions.x; j++)
            {
                Debug.Log(i + "," + j + "," + heightStrings[j]);
                float parsedValue = float.Parse(heightStrings[j]);
                heightMap[i * dimensions.x + j] = parsedValue;
            }
        }
        Debug.Log("----");
        float CUBE_SIZE = 1.0f;
        for (var i = 0; i < dimensions.y; i++)
        {
            for (var j = 0; j < dimensions.x; j++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Debug.Log(i + "," + j + "," + (i*dimensions.x+j));
                cube.transform.position = new Vector3(j * CUBE_SIZE - dimensions.x/2, heightMap[i*dimensions.x+j], i * CUBE_SIZE - dimensions.y/2);
                cube.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Length)];
                AllCubes.Add(cube);
                CubeLocations.Add(new Vector2(i, j));
            }
        }

        InvokeRepeating("rotateCamera", 2, 0.02f);
    }

    void rotateCamera()
    {
        GameObject camera = GameObject.Find("Main Camera");
        camera.transform.Rotate(0, 0.3f, 0);
    }

    void setColors()
    {
        t += 1;
        if (t > 100)
        {
            t = 0;
        }

        for (var i = 0; i < AllCubes.Count; i++)
        {
            Vector2 cubeLocation = CubeLocations[i];
            Color color = new Color(t/100.0f, (cubeLocation.x + 5) * 0.1f, (cubeLocation.y + 5) * 0.1f);
            AllCubes[i].GetComponent<Renderer>().material.color = color;
        }
    }

    void Update()
    {
    }
}
