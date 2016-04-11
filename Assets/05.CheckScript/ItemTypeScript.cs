using UnityEngine;
using System.Collections;

public class ItemTypeScript : MonoBehaviour {

    public enum ItemTy
    {
        ScoreItem,
        PowerItem,
        LifeItem,
        BoomItem,
    }

    public ItemTy ItemTypeCheck;
}
