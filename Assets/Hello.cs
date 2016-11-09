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
        for (int y = -10; y <= 10; y+=5)
        {
            for (int x = -10; x <= 10; x+=5)
            {
                var text = new GUIContent(string.Format("{0}, {1}", x.ToString(), y.ToString()));
                var screenPosition = camera.WorldToScreenPoint(new Vector3(x, y, 0));
                screenPosition = new Vector3(screenPosition.x, -screenPosition.y + view.position.height -20);
                var size = label.CalcSize(text);


                GUI.Label(new Rect(screenPosition, size), text);
            }
        }

        UnityEditor.Handles.EndGUI();
    }
}
