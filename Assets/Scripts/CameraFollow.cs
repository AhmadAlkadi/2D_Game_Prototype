#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.TerrainTools;
#endif
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1,10)]
    public float smoothFactor;
    [HideInInspector]
    public Vector3 minValue, maxValue;
    [Range(0,1)]
    public float moveCamerTriggerValue;

    [HideInInspector]
    public bool setupComplete = false;
    public enum SetupStates { None, Step1, Step2 }
    [HideInInspector]
    public SetupStates ss = SetupStates.None;

    void Start()
    {
        Follow();
    }

    private void FixedUpdate()
    {
        var playerViewPortPos = Camera.main.WorldToViewportPoint(target.transform.position);
        if (playerViewPortPos.x >= moveCamerTriggerValue)
        {
            Follow();
        }
    }

    void Follow()
    {      
        Vector3 targetPosition = target.position + offset;
        //verify if the targetPosition is out of bound or not
        //Limit it to the min and max values
        //clap the value to its max and min
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z));


        Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

        minValue.x = smoothedPosition.x;
    }

    public void ResetValues()
    {
        setupComplete = false;
        minValue = Vector3.zero;
        maxValue = Vector3.zero;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (CameraFollow)target;

        GUILayout.Space(20);

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("-=- Camera Boundaries Settings -=-", titleStyle);

        GUIStyle defultStyle = new GUIStyle();
        defultStyle.fontSize = 12;
        defultStyle.alignment = TextAnchor.MiddleCenter;

        if (script.setupComplete)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Minimum values:", defultStyle);
            GUILayout.Label("Maximum values:", defultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"X = {script.minValue.x}", defultStyle);
            GUILayout.Label($"X = {script.maxValue.x}", defultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Y = {script.minValue.y}", defultStyle);
            GUILayout.Label($"Y = {script.maxValue.y}", defultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("View Minumum"))
            {
                Camera.main.transform.position = script.minValue;
            }
            if (GUILayout.Button("View Maximum"))
            {
                Camera.main.transform.position = script.maxValue;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Focus on Player"))
            {
                Vector3 targetPos = script.target.position + script.offset;
                targetPos.z = script.minValue.z;
                Camera.main.transform.position = targetPos;
            }

            if (GUILayout.Button("Reset Camera Values"))
            {
                script.ResetValues();
            }

        }
        else
        {
            if(script.ss == CameraFollow.SetupStates.None)
            {
                if (GUILayout.Button("Start Setting Camera Values"))
                {
                    script.ss = CameraFollow.SetupStates.Step1;
                }
            }
            if (script.ss == CameraFollow.SetupStates.Step1)
            {
                GUILayout.Label($"1- Select your main Camera", defultStyle);
                GUILayout.Label($"2- Move it to the bottom left bound limit of your level", defultStyle);
                GUILayout.Label($"3- Click the min select button", defultStyle);
                if (GUILayout.Button("min select"))
                {
                    script.minValue = Camera.main.transform.position;

                    script.ss = CameraFollow.SetupStates.Step2;
                }
            }
            if (script.ss == CameraFollow.SetupStates.Step2)
            {
                GUILayout.Label($"1- Select your main Camera", defultStyle);
                GUILayout.Label($"2- Move it to the top right bound limit of your level", defultStyle);
                GUILayout.Label($"3- Click the max select button", defultStyle);
                if (GUILayout.Button("max select"))
                {
                    script.maxValue = Camera.main.transform.position;
                    script.ss = CameraFollow.SetupStates.None;
                    script.setupComplete = true;
                    Vector3 targetPos = script.target.position + script.offset;
                    targetPos.z = script.minValue.z;
                    Camera.main.transform.position = targetPos;
                }
            }
        }
    }
}
#endif