using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fake
{
    public class SceneNavigator : MonoBehaviour
    {
        public void NavigateToPlayScene()
        {
            SceneManager.LoadScene("PlayScene");
        }
    }
}
