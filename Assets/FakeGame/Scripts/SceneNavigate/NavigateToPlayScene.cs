using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Fake
{
    public class NavigateToPlayScene : MonoBehaviour
    {

        public void ChangedToScene()
        {
            SceneManager.LoadScene("PlayScene");
        }
    }
}
