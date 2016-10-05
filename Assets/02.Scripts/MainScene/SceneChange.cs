using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Fake
{
    public class SceneChange : MonoBehaviour
    {

        public void ChangedToScene()
        {
            SceneManager.LoadScene("PlayScene");
        }
    }
}
