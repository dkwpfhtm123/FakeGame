using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange : MonoBehaviour {

    public void ChangedToScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
