using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VaporWorld;

public class VaporWorldGraphic
{
    public Graphic graphic;
    public float maxTransparent;
    public VaporWorldGraphic(Graphic graphic)
    {
        this.graphic = graphic;
        maxTransparent = graphic.color.a;
    }
}
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
    public bool useTimeScale;

    public Graphic firstSelect;
    private static List<UIView> views = new List<UIView>();

    private UIAnimationEvent[] animationEvents;

    public UnityEvent onCancel;
    public UnityEvent onSubmit;

    public IEventHandler EventHandler { set; get; }

    public virtual void OnCancel()
    {
        onCancel?.Invoke();
    }

    public List<Sequence> Show()
    {
        if (useTimeScale)
        {
            Time.timeScale = 1f;
        }

        List<Sequence> list = new List<Sequence>();

        foreach (var animationEvent in animationEvents)
        {
            list.Add(animationEvent.ShowSequences());
        }

        return list;
    }

    public List<Sequence> Show(float insertTime, float duration)
    {
        if (useTimeScale)
        {
            Time.timeScale = 1f;
        }

        List<Sequence> list = new List<Sequence>();

        foreach (var animationEvent in animationEvents)
        {
            list.Add(animationEvent.ShowSequences(insertTime, duration));
        }

        return list;
    }

    public List<Sequence> Hide()
    {
        if (useTimeScale)
        {
            Time.timeScale = 1f;
        }

        List<Sequence> list = new List<Sequence>();

        foreach (var animationEvent in animationEvents)
        {
            list.Add(animationEvent.HideSequences());

        }

        return list;
    }

    public List<Sequence> Hide(float insertTime, float duration)
    {
        if (useTimeScale)
        {
            Time.timeScale = 1f;
        }

        List<Sequence> list = new List<Sequence>();

        foreach (var animationEvent in animationEvents)
        {
            list.Add(animationEvent.HideSequences(insertTime,duration));
        }

        return list;
    }

    public virtual void OnStartShowAnimationEvent()
    {
        gameObject.SetActive(true);

        if (firstSelect != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
            return;
        }

        EventHandler?.OnStartShowAnimationEvent(this);
    }

    public virtual void OnFinishShowAnimationEvent()
    {
        EventHandler?.OnFinishShowAnimationEvent(this);

        if (useTimeScale)
        {
            Time.timeScale = 0;
        }
    }

    public virtual void OnStartHideAnimationEvent()
    {
        EventHandler?.OnStartHideAnimationEvent(this);
    }

    public virtual void OnFinishHideAnimationEvent()
    {
        EventHandler?.OnFinishHideAnimationEvent(this);
        EventHandler = null;
        gameObject.SetActive(false);
    }

    protected virtual void Awake()
    {
        transform.localPosition = Vector3.zero;
        views.Add(this);

        animationEvents = GetComponentsInChildren<UIAnimationEvent>();
        if(animationEvents == null)
        {
            animationEvents = new UIAnimationEvent[0];
        }
    }

    private void OnDestroy()
    {
        views.Remove(this);
    }

    public static UIView Get(string pageName)
    {
        foreach (var view in views)
        {
            if (view.name == pageName)
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

        Debug.Log("Not found view by type: " + typeof(T));
        return null;
    }

    public static List<T> GetList<T>() where T : UIView
    {
        List<T> results = new List<T>();

        foreach (var view in views)
        {
            if (view is T)
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

    public virtual void OnSubmit()
    {
        onSubmit?.Invoke();
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
        Initialize(ingameScene);
    }

    private void Initialize(IngameScene ingameScene)
    {
        ingameScene.UIManager.PushEvent(new UIEventHide(true, this));
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

    public virtual void OnGameOver(InventoryList reconsitutionItems, string KillerSpawnPointID)
    {
    }
}
