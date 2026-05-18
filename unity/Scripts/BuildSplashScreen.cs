using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class BuildSplashScreen : MonoBehaviour
{
    [MenuItem("AR Tools/Build Splash Screen")]
    public static void Build()
    {
        var canvas = GameObject.Find("Canvas");
        if (canvas == null) { Debug.LogError("Canvas not found!"); return; }

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // Remove old splash
        var old = canvas.transform.Find("SplashPanel");
        if (old != null) DestroyImmediate(old.gameObject);

        // Root panel
        var splash = new GameObject("SplashPanel");
        splash.transform.SetParent(canvas.transform, false);
        splash.transform.SetAsFirstSibling();
        var sr = splash.AddComponent<RectTransform>();
        sr.anchorMin = Vector2.zero;
        sr.anchorMax = Vector2.one;
        sr.offsetMin = sr.offsetMax = Vector2.zero;
        splash.AddComponent<Image>().color = new Color(0.04f, 0.04f, 0.04f, 1f);

        // Badge
        var badge = MakeElement(splash.transform, "Badge", 220f, 38f, 0f, 210f);
        badge.AddComponent<Image>().color = new Color(0.1f, 0.45f, 0.2f, 1f);
        AddText(badge.transform, "MIXED REALITY", font, 16, FontStyle.Bold, Color.white);

        // Title white
        var title1 = MakeElement(splash.transform, "Title1", 500f, 80f, 0f, 55f);
        var t1 = AddText(title1.transform, "Place Furniture", font, 60, FontStyle.Bold, Color.white);
        t1.horizontalOverflow = HorizontalWrapMode.Wrap;

        // Title green
        var title2 = MakeElement(splash.transform, "Title2", 500f, 80f, 0f, -55f);
        var t2 = AddText(title2.transform, "In Your Room", font, 60, FontStyle.Bold, new Color(0.2f, 0.8f, 0.35f, 1f));
        t2.horizontalOverflow = HorizontalWrapMode.Wrap;

        // Subtitle
        var sub = MakeElement(splash.transform, "Subtitle", 460f, 70f, 0f, -185f);
        var st = AddText(sub.transform, "Point your camera at the floor, then tap to place and arrange virtual furniture.", font, 20, FontStyle.Normal, new Color(0.65f, 0.65f, 0.65f, 1f));
        st.horizontalOverflow = HorizontalWrapMode.Wrap;
        st.verticalOverflow = VerticalWrapMode.Overflow;

        // Start button
        var btnGO = MakeElement(splash.transform, "StartScanningBtn", 340f, 68f, 0f, -305f);
        btnGO.AddComponent<Image>().color = new Color(0.1f, 0.45f, 0.2f, 1f);
        var btn = btnGO.AddComponent<Button>();
        AddText(btnGO.transform, "Start Scanning", font, 28, FontStyle.Bold, Color.white);

        // Add SplashScreenManager to canvas
        var mgrComponent = canvas.GetComponent("SplashScreenManager") as MonoBehaviour;
        if (mgrComponent == null)
        {
            var mgrType = System.Type.GetType("SplashScreenManager, Assembly-CSharp");
            if (mgrType != null)
                mgrComponent = canvas.AddComponent(mgrType) as MonoBehaviour;
            else
                Debug.LogError("SplashScreenManager type not found - make sure it compiled first!");
        }

        if (mgrComponent != null)
        {
            var so = new SerializedObject(mgrComponent);
            so.FindProperty("splashPanel").objectReferenceValue = splash;
            so.FindProperty("mainCanvas").objectReferenceValue = canvas;
            so.FindProperty("arSession").objectReferenceValue = GameObject.Find("AR Session");
            so.FindProperty("xrOrigin").objectReferenceValue = GameObject.Find("XR Origin");
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(mgrComponent);

            // Wire button
            var bso = new SerializedObject(btn);
            var calls = bso.FindProperty("m_OnClick.m_PersistentCalls.m_Calls");
            if (calls.arraySize == 0) calls.InsertArrayElementAtIndex(0);
            var call = calls.GetArrayElementAtIndex(0);
            call.FindPropertyRelative("m_Target").objectReferenceValue = mgrComponent;
            call.FindPropertyRelative("m_MethodName").stringValue = "OnStartScanning";
            call.FindPropertyRelative("m_Mode").intValue = 1;
            call.FindPropertyRelative("m_CallState").intValue = 2;
            bso.ApplyModifiedProperties();
            EditorUtility.SetDirty(btn);
        }

        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        Debug.Log("Splash screen built!");
    }

    static GameObject MakeElement(Transform parent, string name, float w, float h, float x, float y)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var r = go.AddComponent<RectTransform>();
        r.anchorMin = r.anchorMax = r.pivot = new Vector2(0.5f, 0.5f);
        r.sizeDelta = new Vector2(w, h);
        r.anchoredPosition = new Vector2(x, y);
        return go;
    }

    static Text AddText(Transform parent, string text, Font font, int size, FontStyle style, Color color)
    {
        var go = new GameObject("Text");
        go.transform.SetParent(parent, false);
        var r = go.AddComponent<RectTransform>();
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one;
        r.offsetMin = r.offsetMax = Vector2.zero;
        var t = go.AddComponent<Text>();
        t.text = text; t.font = font; t.fontSize = size;
        t.fontStyle = style; t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        return t;
    }
}
