using UnityEngine;
using System.Collections;

public enum ItemType
{
    ScoreItem,
    PowerItem,
    LifeItem,
    BoomItem,
}

public class ItemTypeScript : MonoBehaviour {
    public ItemType ItemTypeCheck;
}
