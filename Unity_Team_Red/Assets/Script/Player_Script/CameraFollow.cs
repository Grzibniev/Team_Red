#if UNITY_EDITOR
using UnityEditor;
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


   [HideInInspector]
    public bool setupComplete = false;
    public enum setupState { None,Step1,Step2}
    [HideInInspector]
    public setupState ss = setupState.None;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        //definiuje minimalne i maksymalne x,y,z 

        Vector3 targetPosition = target.position + offset;
        //sprawdza czy targetPosition jest za granicami czy nie
        //limituje do minimalnych i maksymalnych wartosci
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z));

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
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

        //przypisaæ monobehaviour do docelowego skryptu
        var script = (CameraFollow)target;
        //sprawdzanie czy wartosci s¹ wpisane czy nie

        //spacja miedzy kolumnami
        GUILayout.Space(20);

        GUIStyle defaultStyle = new GUIStyle();
        defaultStyle.fontSize = 12;
        defaultStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 14;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("-=- Camera Boundries Settings -=-", titleStyle);
        //jak s¹ wpisane to wyœwietla min i max wartosci razem z guzikiem do zobaczenia ich
        //i fo tego guzik z resetem
        if (script.setupComplete)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("Minimum Values:", defaultStyle);
            GUILayout.Label("Maximum Values:", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"X = {script.minValue.x}", defaultStyle);
            GUILayout.Label($"X = {script.maxValue.x}", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Y = {script.minValue.y}", defaultStyle);
            GUILayout.Label($"Y = {script.maxValue.y}", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("View Minimum"))
            {
                //przemieœæ kamere na minimalne wartosci
                Camera.main.transform.position = script.minValue;
            }
            if (GUILayout.Button("View Maximum"))
            {
                //przemieœæ kamere na maxymalne wartosci
                Camera.main.transform.position = script.maxValue;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Focus On Target"))
            {
                //przemieœæ kamere na postaæ gracza
                Vector3 targetPos = script.target.position + script.offset;
                targetPos.z = script.minValue.z;
                Camera.main.transform.position = targetPos;
            }

            if (GUILayout.Button("Reset Camera Values"))
            {
                //resetuje setupcomplete boolean
                //resetuje min, max, vec3 values
                script.ResetValues();
            }


        }
        //jak watrosci nie sa przypisane to wyswietlasie guzik z przypisaniem ich
        else
        {
            //krok 0: zresetuj kamere na postac geracza
            if (script.ss == CameraFollow.setupState.None)
            {
                if (GUILayout.Button("Start Setting Camera Values"))
                {
                    //zmienia stan na krok 1
                    script.ss = CameraFollow.setupState.Step1;
                }
            }
            //krok 1: przypisz lewy dolny róg mapy (min wartosci)
            else if (script.ss == CameraFollow.setupState.Step1)
            {
                //instrukcje co zrobiæ
                GUILayout.Label($"1- Select your main camera", defaultStyle);
                GUILayout.Label($"2- Move it to the bottom left bound limit of your level", defaultStyle);
                GUILayout.Label($"3- click the 'Set Minimum Values' Button", defaultStyle);
                //guzik do ustawienia min wartosci
                if (GUILayout.Button("Set Minimum Values"))
                {
                    //ustawia minimalne wartosci dla camery
                    script.minValue = Camera.main.transform.position;
                    //zmiana na krok 2
                    script.ss = CameraFollow.setupState.Step2;
                }
            }
            //krok 2: przypisz prawy górny róg mapy (max wartosci)
            else if (script.ss == CameraFollow.setupState.Step2)
            {
                //instrukcje co zrobiæ
                GUILayout.Label($"1- Select your main camera", defaultStyle);
                GUILayout.Label($"2- Move it to the top right bound limit of your level", defaultStyle);
                GUILayout.Label($"3- click the 'Set Maximum Values' Button", defaultStyle);
                //guzik do ustawienia max wartosci
                if (GUILayout.Button("Set Maximum Values"))
                {
                    //ustawia minimalne wartosci dla camery
                    script.maxValue = Camera.main.transform.position;
                    //zmiana na krok 0
                    script.ss = CameraFollow.setupState.None;
                    //Wy³¹czamy setupcomplete boolean
                    script.setupComplete = true;
                    //resetuje kamere na postac gracza
                    Vector3 targetPos = script.target.position + script.offset;
                    targetPos.z = script.minValue.z;
                    Camera.main.transform.position = targetPos;
                }
            }
        }
    }
}
#endif