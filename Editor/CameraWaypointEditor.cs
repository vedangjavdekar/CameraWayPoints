using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CameraWayPoints))]
public class CameraWaypointEditor : Editor
{
    CameraWayPoints wayPoints;
    
    SerializedProperty points;
    ReorderableList list;

    private void OnEnable()
    {
        wayPoints = (CameraWayPoints)target;
        points= serializedObject.FindProperty("points");
        list = new ReorderableList(serializedObject, points, true, true, true, true);
        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
        list.elementHeight = EditorGUIUtility.singleLineHeight * 3.7f;
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PrefixLabel(rect, new GUIContent($"Point{index}"),EditorStyles.boldLabel);
        Color col = element.FindPropertyRelative("color").colorValue;
        if(col.Equals(new Color(0,0,0,0)))
        {
            col = Random.ColorHSV();
            element.FindPropertyRelative("color").colorValue = col;
        }
        EditorGUI.DrawRect(new Rect(rect.x + 80, rect.y + 2.5f, EditorGUIUtility.singleLineHeight-5, EditorGUIUtility.singleLineHeight-5),col );
        if(GUI.Button(new Rect(rect.x + 120, rect.y, 50, EditorGUIUtility.singleLineHeight),new GUIContent("Align")))
        {
            wayPoints.transform.position = element.FindPropertyRelative("position").vector3Value;
            wayPoints.transform.eulerAngles = element.FindPropertyRelative("angles").vector3Value;
        }
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel++;
        EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 10, 100, EditorGUIUtility.singleLineHeight), new GUIContent("position"));
        EditorGUI.PropertyField(
            new Rect(rect.x + 80, rect.y + EditorGUIUtility.singleLineHeight + 10, 150, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("position"),
            GUIContent.none
        );
        EditorGUI.LabelField(new Rect(rect.x, rect.y + 2 * EditorGUIUtility.singleLineHeight + 10, 100, EditorGUIUtility.singleLineHeight), new GUIContent("angles"));
        EditorGUI.PropertyField(
            new Rect(rect.x + 80, rect.y + 2*EditorGUIUtility.singleLineHeight + 10, 150, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("angles"),
            GUIContent.none
        );
        EditorGUI.indentLevel = indent;
    }

    void DrawHeader(Rect rect)
    {
        string name = "Points";
        EditorGUI.LabelField(rect, name,EditorStyles.boldLabel);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.FindProperty("drawGizmos");
        EditorGUILayout.PropertyField(prop);
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);
        if (GUILayout.Button("Add current"))
        {
            wayPoints.points.Add(new WayPoint {position = wayPoints.transform.position,angles = wayPoints.transform.eulerAngles,color = Random.ColorHSV()});
        }
    }


}
