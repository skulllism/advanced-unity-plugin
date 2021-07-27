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
    public List<VaporWorldGraphic> graphics = new List<VaporWorldGraphic>();

    private static List<UIView> views = new List<UIView>();

    public Image pannel;
    public VaporWorldGraphic Pannel { private set; get; }
    private UIAnimationEvent[] animationEvents;

    protected UIManager UI;

    public UnityEvent onCancel;
    public UnityEvent onSubmit;

    public IEventHandler EventHandler { set; get; }

    public virtual void OnCancel()
    {
        onCancel?.Invoke();
    }
    public void HideImmediately(Action onStart, Action onFinish)
    {
        List<Sequence> list = new List<Sequence>();

        foreach (var animationEvent in animationEvents)
        {
            list.Add(animationEvent.HideSequences(0));
        }

        Hide(list, onStart, onFinish);
    }

    public void ShowImmediately(Action onStart, Action onFinish)
    {
        List<Sequence> list = new List<Sequence>();

        foreach (var animationEvent in animationEvents)
        {
            list.Add(animationEvent.ShowSequences(0));
        }

        Show(list, onStart, onFinish);
    }

    public void Show(Action onStart, Action onFinish)
    {
        List<Sequence> list = new List<Sequence>();

        foreach (var animationEvent in animationEvents)
        {
            list.Add(animationEvent.ShowSequences());
        }

        Show(list, onStart, onFinish);
    }

    private void Show(List<Sequence> list, Action onStart, Action onFinish)
    {
        UI.AnimationEventManager.Push(new UIAnimationEventManager.SequenceStream(list,
            () =>
            {
                onStart?.Invoke();
                OnStartShowAnimationEvent();
            },
         () =>
         {
             onFinish?.Invoke();
             OnFinishShowAnimationEvent();
         }));
    }
    private void Hide(List<Sequence> list, Action onStart, Action onFinish)
    {
        UI.AnimationEventManager.Push(new UIAnimationEventManager.SequenceStream(list,
         () =>
         {
             onStart?.Invoke();
             OnStartHideAnimationEvent();
         },
           () =>
           {
               onFinish?.Invoke();
               OnFinishHideAnimationEvent();
           }));
    }

    public void Hide(Action onStart, Action onFinish)
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

        Hide(list, onStart, onFinish);
    }

    protected virtual void OnStartShowAnimationEvent()
    {
        gameObject.SetActive(true);

        if (firstSelect != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
            return;
        }

        EventHandler?.OnStartShowAnimationEvent(this);
    }

    protected virtual void OnFinishShowAnimationEvent()
    {
        EventHandler?.OnFinishShowAnimationEvent(this);

        if (useTimeScale)
        {
            Time.timeScale = 0;
        }
    }

    protected virtual void OnStartHideAnimationEvent()
    {
        EventHandler?.OnStartHideAnimationEvent(this);
    }

    protected virtual void OnFinishHideAnimationEvent()
    {
        EventHandler?.OnFinishHideAnimationEvent(this);
        EventHandler = null;
        gameObject.SetActive(false);
    }

    protected virtual void Awake()
    {
        transform.localPosition = Vector3.zero;
        views.Add(this);
        if (pannel != null)
        {
            Pannel = new VaporWorldGraphic(pannel);
        }
        Graphic[] temp = GetComponentsInChildren<Graphic>();
        foreach (var graphic in temp)
        {
            graphics.Add(new VaporWorldGraphic(graphic));
        }
        animationEvents = GetComponentsInChildren<UIAnimationEvent>();
    }

    private void OnDestroy()
    {
        views.Remove(this);
    }

    public VaporWorldGraphic[] GetAllGraphics()
    {
        return graphics.ToArray();
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

        Debug.Log("Not found page by type: " + typeof(T));
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
        UI = ingameScene.UI;
        HideImmediately(null, null);
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
