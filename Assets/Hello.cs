using UnityEngine;
using System.Collections;

public class Hello : MonoBehaviour
{
    void OnDrawGizmos()
    {
        UnityEditor.Handles.BeginGUI();
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        var camera = view.camera;
        var label = GUI.skin.label;
        for (int y = -10; y <= 10; y++)
        {
            for (int x = -10; x <= 10; x++)
            {
                var text = new GUIContent(string.Format("{0}, {1}", x.ToString(), y.ToString()));
                var screenPosition = camera.WorldToScreenPoint(new Vector3(x, y, 0));
                var size = label.CalcSize(text);

                GUI.Label(new Rect(screenPosition, size), text);
            }
        }

        UnityEditor.Handles.EndGUI();
    }
}
