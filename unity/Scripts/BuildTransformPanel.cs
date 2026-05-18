using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class BuildTransformPanel : MonoBehaviour
{
    [MenuItem("AR Tools/Build Transform Panel")]
    public static void Build()
    {
        var canvas = GameObject.Find("Canvas");
        var builtinFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // Remove old panel if exists
        var old = GameObject.Find("TransformPanel");
        if (old != null) DestroyImmediate(old);

        // Panel
        var panel = new GameObject("TransformPanel");
        panel.transform.SetParent(canvas.transform, false);
        var panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0f);
        panelRect.anchorMax = new Vector2(0.5f, 0f);
        panelRect.pivot = new Vector2(0.5f, 0f);
        panelRect.anchoredPosition = new Vector2(0, 200f);
        panelRect.sizeDelta = new Vector2(320f, 110f);
        var panelImg = panel.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.6f);

        MakeLabel("Scale",  panel.transform, new Vector2(-80f, 38f), builtinFont);
        MakeLabel("Rotate", panel.transform, new Vector2( 80f, 38f), builtinFont);

        var btnScaleDown = MakeBtn("-",  panel.transform, new Vector2(-105f, 5f), builtinFont);
        var btnScaleUp   = MakeBtn("+",  panel.transform, new Vector2(-55f,  5f), builtinFont);
        var btnRotL      = MakeBtn("<",  panel.transform, new Vector2( 55f,  5f), builtinFont);
        var btnRotR      = MakeBtn(">",  panel.transform, new Vector2(105f,  5f), builtinFont);

        var div = new GameObject("Divider");
        div.transform.SetParent(panel.transform, false);
        var dr = div.AddComponent<RectTransform>();
        dr.sizeDelta = new Vector2(1f, 80f);
        dr.anchoredPosition = new Vector2(0f, 5f);
        div.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);

        panel.SetActive(false);

        // Wire manager
        var xrOrigin = GameObject.Find("XR Origin");
        var mgr = xrOrigin.GetComponent("FurniturePlacementManager") as MonoBehaviour;

        var so = new SerializedObject(mgr);
        so.FindProperty("transformPanel").objectReferenceValue = panel;
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(mgr);

        WireBtn(btnScaleDown, mgr, "ScaleDown");
        WireBtn(btnScaleUp,   mgr, "ScaleUp");
        WireBtn(btnRotL,      mgr, "RotateLeft");
        WireBtn(btnRotR,      mgr, "RotateRight");

        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        Debug.Log("TransformPanel built successfully!");
    }

    static void MakeLabel(string text, Transform parent, Vector2 pos, Font font)
    {
        var go = new GameObject("Label_" + text);
        go.transform.SetParent(parent, false);
        var r = go.AddComponent<RectTransform>();
        r.sizeDelta = new Vector2(160f, 22f);
        r.anchoredPosition = pos;
        var t = go.AddComponent<Text>();
        t.text = text; t.font = font; t.fontSize = 13;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = new Color(1f, 1f, 1f, 0.7f);
    }

    static Button MakeBtn(string label, Transform parent, Vector2 pos, Font font)
    {
        var g = new GameObject("Btn_" + label);
        g.transform.SetParent(parent, false);
        var r = g.AddComponent<RectTransform>();
        r.sizeDelta = new Vector2(70f, 45f);
        r.anchoredPosition = pos;
        g.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.15f);
        var btn = g.AddComponent<Button>();

        var tg = new GameObject("Text");
        tg.transform.SetParent(g.transform, false);
        var tr = tg.AddComponent<RectTransform>();
        tr.anchorMin = Vector2.zero; tr.anchorMax = Vector2.one;
        tr.offsetMin = tr.offsetMax = Vector2.zero;
        var tx = tg.AddComponent<Text>();
        tx.text = label; tx.font = font; tx.fontSize = 24;
        tx.fontStyle = FontStyle.Bold;
        tx.alignment = TextAnchor.MiddleCenter;
        tx.color = Color.white;
        return btn;
    }

    static void WireBtn(Button btn, MonoBehaviour target, string method)
    {
        var bso = new SerializedObject(btn);
        var calls = bso.FindProperty("m_OnClick.m_PersistentCalls.m_Calls");
        calls.InsertArrayElementAtIndex(0);
        var call = calls.GetArrayElementAtIndex(0);
        call.FindPropertyRelative("m_Target").objectReferenceValue = target;
        call.FindPropertyRelative("m_MethodName").stringValue = method;
        call.FindPropertyRelative("m_Mode").intValue = 1;
        call.FindPropertyRelative("m_CallState").intValue = 2;
        bso.ApplyModifiedProperties();
        EditorUtility.SetDirty(btn);
    }
}
