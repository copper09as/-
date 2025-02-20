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

    void Start()
    {
        for(int i = 0;i<witdh * height-1;i++)
        {

            GameObject grid;
            grid = Instantiate(transform.GetChild(0).gameObject, transform);
            grid.AddComponent<Grid>();
            grids.Add(grid.GetComponent<Grid>());
            Color currentColor = grid.GetComponent<SpriteRenderer>().color;
            currentColor.a = 0.19f;
            grid.GetComponent<SpriteRenderer>().color = currentColor;
            grid.transform.position = new Vector2(
                transform.position.x + grid.GetComponent<SpriteRenderer>().sprite.border.x * i / height
                , transform.position.y + grid.GetComponent<SpriteRenderer>().sprite.border.y % witdh
                );
            grid.name = "grid";
        }

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
