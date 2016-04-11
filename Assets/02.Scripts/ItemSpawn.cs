using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour {
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

    public static ItemSpawn instance = null;

    void Awake()
    {
        instance = this;
    }


    public void SpawnItem(Transform point, ItemType item)
    {
        //@ switch

        // 오류가 나는부분 질문.
        /*      switch (item)
              {
                  case ItemType.PowerItem:
                      GameObject Item = GameObject.Instantiate(PowerItem);
                      break;

                  case ItemType.ScoreItem:
                      GameObject Item = GameObject.Instantiate(ScoreItem);
                      break;
              }

              Transform ItemTransform = Item.transform;
              SetPosition(point, ItemTransform); */

        GameObject Item = null;
        if (item == ItemType.PowerItem)
        {
            Item = GameObject.Instantiate(PowerItem);
        } 

        else if (item == ItemType.ScoreItem)
        {
            Item = GameObject.Instantiate(ScoreItem);
        }

        Transform ItemTransform = Item.transform;
        SetPosition(point, ItemTransform);
    }

    void SetPosition(Transform point , Transform ItemTransform)
    {
        ItemTransform.localPosition = point.transform.localPosition;
        ItemTransform.localRotation = Quaternion.identity;
        ItemTransform.localScale = Vector3.one;
    }

}
