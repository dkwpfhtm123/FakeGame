using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour
{
    public GameObject PowerItem;
    public GameObject ScoreItem;
    public GameObject BoomItem;
    public GameObject LifeItem;

    public enum ItemTypeObject
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

    public void SpawnItem(Transform point, ItemTypeObject itemType)
    {
        GameObject item = null;
        switch (itemType)
        {
            case ItemTypeObject.PowerItem:
                item = GameObject.Instantiate(PowerItem);
                break;

            case ItemTypeObject.ScoreItem:
                item = GameObject.Instantiate(ScoreItem);
                break;
        }

        Transform itemTransform = item.transform;
        SetPosition(point, itemTransform);
    }

    private void SetPosition(Transform point, Transform itemTransform)
    {
        itemTransform.localPosition = point.transform.localPosition;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }
}
