using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour
{
    public GameObject PowerItem;
    public GameObject ScoreItem;
    public GameObject BoomItem;
    public GameObject LifeItem;

    public enum ItemType
    {
        PowerItem,
        ScoreItem,
        BoomItem,
        LifeItem
    }

    public static ItemSpawn Instance = null;

    void Awake()
    {
        Instance = this;
    }


    public void SpawnItem(Transform point, ItemType item)
    {
        GameObject Item = null;
        switch (item)
        {
            case ItemType.PowerItem:
                Item = GameObject.Instantiate(PowerItem);
                break;

            case ItemType.ScoreItem:
                Item = GameObject.Instantiate(ScoreItem);
                break;
        }


        Transform itemTransform = Item.transform;
        SetPosition(point, itemTransform);

    }

    private void SetPosition(Transform point, Transform ItemTransform)
    {
        ItemTransform.localPosition = point.transform.localPosition;
        ItemTransform.localRotation = Quaternion.identity;
        ItemTransform.localScale = Vector3.one;
    }
}
