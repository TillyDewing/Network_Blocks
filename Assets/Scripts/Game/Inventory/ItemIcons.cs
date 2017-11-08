using UnityEngine;
using System.Collections;

public class ItemIcons : MonoBehaviour
{
    private static Sprite[] icons;
    private static Sprite nullIcon;

    public Sprite[] itemIcons;
    public Sprite nullicon;


    void Awake()
    {
        icons = itemIcons;
        nullIcon = nullicon;
    }

    public static Sprite GetIcon(int iconId)
    {
        if (iconId < icons.Length && iconId >= 0)
        {
            return icons[iconId];
        }
        else
        {
            Debug.LogWarning("Icon with id: " + iconId + " does not exist");
            return nullIcon;
            
        }
    }


}
