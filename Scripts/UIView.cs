using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VaporWorld;

public class UIView : MonoBehaviour, UIManager.ICommand, IngameScene.IEventHandler
{
    public bool isOverlay;

    public Graphic firstSelect;

    private Graphic[] graphics;
    
    private static List<UIView> views = new List<UIView>();

    public Image panel;

    private UIManager UI;

    public virtual void OnCancel()
    {
        UI.Pop();
    }
    public virtual void Hide()
    {
        List<UIAnimationEventManager.FadeParams> list = new List<UIAnimationEventManager.FadeParams>();

        Graphic[] graphics = GetAllGraphics();

        foreach (var graphic in graphics)
        {
            if (panel != null && graphic == panel)
            {
                list.Add(new UIAnimationEventManager.FadeParams(panel, 0.0f, 0.5f, 0.25f));
                continue;
            }

            list.Add(new UIAnimationEventManager.FadeParams(graphic, 0.0f, 0.5f));
        }

        UI.EventManager.Queue.Enqueue(new UIAnimationEventManager.UIAnimationEvent(list.ToArray(),
            () =>
            {
                Time.timeScale = 1f;
            },
             () =>
             {
                 HideImmediately();
             }));
    }
    public virtual void Show()
    {
        List<UIAnimationEventManager.FadeParams> list = new List<UIAnimationEventManager.FadeParams>();

        SetAllAlpha(0);

        Graphic[] graphics = GetAllGraphics();

        foreach (var graphic in graphics)
        {
            if (panel != null && graphic == panel)
            {
                list.Add(new UIAnimationEventManager.FadeParams(panel, 0.5f, 0.5f, 0f));
                continue;
            }

            list.Add(new UIAnimationEventManager.FadeParams(graphic, 1f, 0.5f, 0.25f));
        }

        UI.EventManager.Queue.Enqueue(new UIAnimationEventManager.UIAnimationEvent(list.ToArray(),
            () =>
            {
                ShowImmediately();
            },
            () =>
            {
                Time.timeScale = 0f;
            }));
    }

    private void OnDestroy()
	{
        views.Remove(this);
	}

    private void Awake()
    {
        transform.localPosition = Vector3.zero;
        views.Add(this);
        graphics = GetComponentsInChildren<Graphic>();
        HideImmediately();
    }
    public Graphic[] GetAllGraphics()
    {
        return graphics;
    }

    public static UIView Get(string pageName)
    {
        foreach (var view in views)
        {
            if(view.name == pageName)
            {
                return view;
            }
        }

        Debug.Log("Not found page by name: " + pageName);
        return null;
    }

    public static T Get<T>() where T : UIView
    {
        foreach (var view in views)
        {
            if (view is T)
            {
                return view as T;
            }
        }

        Debug.Log("Not found page by type: " + typeof(T));
        return null;
    }

    public static List<T> GetList<T>() where T : UIView
    {
        List<T> results = new List<T>();

        foreach (var view in views)
        {
            if(view is T)
            {
                results.Add(view as T);
            }
        }

        if (results.Count > 0)
        {
            return results;
        }

        Debug.LogError("Can't Found UIView List");
        return null;
    }

    public bool IsShowing()
    {
        return gameObject.activeSelf;
    }

    public void ShowImmediately()
    {
        gameObject.SetActive(true);

        if(firstSelect != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
            return;
        }
    }

    public void SetAllAlpha(float value)
    {
        foreach (var graphic in graphics)
        {
            graphic.DOFade(value, 0);
        }
    }

    public void HideImmediately()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnSubmit()
    {
    }

    public virtual void OnLeftBumper()
    {
    }

    public virtual void OnRightBumper()
    {
    }

    public virtual void OnTab()
    {
    }

    public virtual void OnSceneInitialized(IngameScene ingameScene)
    {
        UI = ingameScene.UI;
    }

    public virtual void OnPlayerInitialzed(Player player)
    {
    }

    public virtual void OnGameReset()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnLateUpdate()
    {
    }

    public virtual void OnResister(params object[] args)
    {
    }

    public virtual void OnUnresister()
    {
    }
}
