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
    public interface IEventHandler
    {
        void OnStartShowAnimationEvent(UIView view);
        void OnFinishShowAnimationEvent(UIView view);
        void OnStartHideAnimationEvent(UIView view);
        void OnFinishHideAnimationEvent(UIView view);
    }

    public bool isOverlay;

    public Graphic firstSelect;

    private Graphic[] graphics;
    
    private static List<UIView> views = new List<UIView>();

    public Image pannel;

    private UIManager UI;

    public IEventHandler EventHandler { set; get; }

    public virtual void OnCancel()
    {
        UI.Escape();
    }

    public void Show()
    {
        UI.EventManager.Push(GetShowAnimationEvent(),
            OnStartShowAnimationEvent, OnFinishShowAnimationEvent);
    }

    public void Hide()
    {
        UI.EventManager.Push(GetHideAnimationEvent(),
            OnStartHideAnimationEvent, OnFinishHideAnimationEvent);
    }

    protected virtual void OnStartShowAnimationEvent()
    {
        EventHandler?.OnStartShowAnimationEvent(this);
    }

    protected virtual void OnFinishShowAnimationEvent()
    {
        EventHandler?.OnFinishShowAnimationEvent(this);
    }

    protected virtual void OnStartHideAnimationEvent()
    {
        EventHandler?.OnStartHideAnimationEvent(this);
    }

    protected virtual void OnFinishHideAnimationEvent()
    {
        EventHandler?.OnFinishHideAnimationEvent(this);
    }

    protected virtual UIAnimationEventManager.EventParams[] GetHideAnimationEvent()
    {
        return new UIAnimationEventManager.SetActiveEvent[]
            { new UIAnimationEventManager.SetActiveEvent(gameObject,false) };
    }

    protected virtual UIAnimationEventManager.EventParams[] GetShowAnimationEvent()
    {
        return new UIAnimationEventManager.SetActiveEvent[]
            { new UIAnimationEventManager.SetActiveEvent(gameObject,true) };
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
        gameObject.SetActive(false);
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
