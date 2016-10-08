using UnityEngine;
using System.Collections;

namespace Fake
{
    public enum ItemType
    {
        ScoreItem,
        PowerItem,
        LifeItem,
        BoomItem,
    }

    public class ItemTypeCode : MonoBehaviour
    {
        public ItemType ItemTypeCheck;
    }
}