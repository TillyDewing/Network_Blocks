using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInfo : MonoBehaviour
{
    public static Item selectedItem = ItemDatabase.nullItem;
    

    public Text title;
    public Text description;
    public Text stats;
    public Text value;

    void Update()
    {
        //transform.position = Input.mousePosition + new Vector3(.01f, .01f, 0);
    }

    public void SetItem(Item item)
    {
        selectedItem = item;
        title.text = selectedItem.Title;
        description.text = selectedItem.Description;
        stats.text = "Power: " + selectedItem.Power + "\nDefence: " + selectedItem.Defence;
        value.text = selectedItem.Value.ToString();

        
    }
}
