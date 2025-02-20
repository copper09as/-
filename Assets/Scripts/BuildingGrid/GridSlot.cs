using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GridSlot : MonoBehaviour
{
    [SerializeField] int gridNumeber = 52;
    [SerializeField] private int witdh;
    [SerializeField] private int height;
    [SerializeField] List<Grid> grids;
    [SerializeField] private float interval;
    void Start()
    {
        for(int i = 0;i<witdh * height;i++)
        {

            GameObject grid;
            grid = Instantiate(transform.GetChild(0).gameObject, transform);
            //grid.AddComponent<Grid>();
            grids.Add(grid.GetComponent<Grid>());
            //grid.GetComponent<Collider2D>().enabled = true;
            Color currentColor = grid.GetComponent<SpriteRenderer>().color;
            currentColor.a = 0.19f;
            grid.GetComponent<SpriteRenderer>().color = currentColor;
            grid.transform.position = new Vector2(
                transform.position.x + interval * (i % witdh)
                , transform.position.y - interval *(i % height)
                );
            
            grid.name = "grid";
        }
        Destroy(transform.GetChild(0).gameObject);

        /*placePosition.Add(grids[0].transform.position);
        placePosition.Add(grids[witdh - 1].transform.position);
        placePosition.Add(grids[witdh * (height - 1)].transform.position);
        placePosition.Add(grids[grids.Count - 1].transform.position);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
