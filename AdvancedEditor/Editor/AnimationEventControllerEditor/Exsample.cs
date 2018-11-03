using UnityEngine;
using UnityEditor;

public class Exsample : EditorWindow 
{
    protected GameObject go;
    protected AnimationClip animationClip;
    protected float time = 0.0f;
    protected bool lockSelection = false;
    protected bool animationMode = false;

    [MenuItem("Examples/AnimationMode demo", false, 2000)]
    public static void DoWindow()
    {
        var window = GetWindowWithRect<Exsample>(new Rect(0, 0, 300, 80));
        window.Show();
    }

    // Has a GameObject been selection?
    public void OnSelectionChange()
    {
        //for (int i = 0; i < Selection.objects.Length; i++)
        //    Debug.Log(Selection.objects[i].name);
        
        //Debug.Log(Selection.activeGameObject.name);
        if (!lockSelection)
        {
            go = Selection.activeGameObject;
            Repaint();
        }
    }


    // Main editor window
    public void OnGUI()
    {
        // Wait for user to select a GameObject
        if (go == null)
        {
            EditorGUILayout.HelpBox("Please select a GameObject", MessageType.Info);
            return;
        }

        // Animate and Lock buttons.  Check if Animate has changed
        GUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        GUILayout.Toggle(AnimationMode.InAnimationMode(), "Animate");
        if (EditorGUI.EndChangeCheck())
            ToggleAnimationMode();

        GUILayout.FlexibleSpace();
        lockSelection = GUILayout.Toggle(lockSelection, "Lock");
        GUILayout.EndHorizontal();

        // Slider to use when Animate has been ticked
        EditorGUILayout.BeginVertical();
        animationClip = EditorGUILayout.ObjectField(animationClip, typeof(AnimationClip), false) as AnimationClip;
        if (animationClip != null)
        {
            float startTime = 0.0f;
            float stopTime = animationClip.length;
            time = EditorGUILayout.Slider(time, startTime, stopTime);
        }
        else if (AnimationMode.InAnimationMode())
            AnimationMode.StopAnimationMode();
        EditorGUILayout.EndVertical();
    }

    void Update()
    {
        if (go == null)
            return;

        if (animationClip == null)
            return;

        // There is a bug in AnimationMode.SampleAnimationClip which crashes
        // Unity if there is no valid controller attached
        Animator animator = go.GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController == null)
            return;

        // Animate the GameObject
        if (!EditorApplication.isPlaying && AnimationMode.InAnimationMode())
        {
            AnimationMode.BeginSampling();
            AnimationMode.SampleAnimationClip(go, animationClip, time);
            AnimationMode.EndSampling();

            SceneView.RepaintAll();
        }
    }

    void ToggleAnimationMode()
    {
        if (AnimationMode.InAnimationMode())
            AnimationMode.StopAnimationMode();
        else
            AnimationMode.StartAnimationMode();
    }
}
