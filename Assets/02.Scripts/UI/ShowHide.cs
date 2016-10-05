using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Fake
{
    public class ShowHide : MonoBehaviour
    {
        public Image[] Object = new Image[5];
        
        public void Reset()
        {
            for (int i = 0; i < 5; i++)
            {
                Object[i].enabled = false;
            }
        }

        public void Show(int num)
        {
            for(int i=0; i< num; i++)
            {
                Object[i].enabled = true;
            }
        }

    }
}
