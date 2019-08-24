using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGame : MonoBehaviour
{

    public Vector2 grid;
    public Object prefab;
    public RectTransform holder;
    // Update is called once per frame

    private void Start()
    {
        Draw();
    }
    void Update()
    {
        
    }

    private void Draw()
    {
        Vector3 startPosition = new Vector3(-holder.rect.width /2,holder.rect.height /2);
                    var canvas = FindObjectOfType<Canvas>();
        for (int i = 0; i< grid.x; i++)
        {
            for (int j = 0; j< grid.y; j++)
            {
                if (i == 0 && i%2 == 0)
                {
                    GameObject go = (GameObject)GameObject.Instantiate(prefab, holder.transform);
                    go.transform.localPosition = startPosition + new Vector3(i * 50,j * -50 -50);
                }
                else {
                    GameObject go = (GameObject)GameObject.Instantiate(prefab, holder.transform);
                    go.transform.localPosition = startPosition + new Vector3(i * 50, j * -50);

                }
            }
        }
    }
}
