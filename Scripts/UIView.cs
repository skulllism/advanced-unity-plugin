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

    private Graphic[] graphics;
    
    private static List<UIView> views = new List<UIView>();

    public Image pannel;

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
        UI.AnimationEventManager.Push(UIAnimationEventManager.GetFade(GetAllGraphics(), 0, 0),
            () =>
            {
                onStart?.Invoke();
                OnStartHideAnimationEvent();
            },
              () =>
              {
                  onFinish?.Invoke();
                  OnFinishHideAnimationEvent();
              });
    }

    public void ShowImmediately(Action onStart, Action onFinish)
    {
        UI.AnimationEventManager.Push(UIAnimationEventManager.GetFade(GetAllGraphics(), 1, 0),
            ()=>
            {
                onStart?.Invoke();
                OnStartShowAnimationEvent();
            },
         () =>
         {
             onFinish?.Invoke();
             OnFinishShowAnimationEvent();
         });     
    }

    public void Show(Action onStart, Action onFinish)
    {
        //Transparent Zero Setting
        Sequence sequence = DOTween.Sequence();

        foreach (var graphic in GetAllGraphics().ToList())
        {
            sequence.Insert(0, graphic.DOFade(0, 0));
        }

        sequence.OnComplete(() =>
        {
            //Push Show Animation
            UI.AnimationEventManager.Push(GetShowAnimationEvent(),
           () =>
           {
               onStart?.Invoke();
               OnStartShowAnimationEvent();
           },
           ()=>
           {
               onFinish?.Invoke();
               OnFinishShowAnimationEvent();
           });
        });
    }

    public void Hide(Action onStart, Action onFinish)
    {
        if (useTimeScale)
        {
            Time.timeScale = 1f;
        }

        UI.AnimationEventManager.Push(GetHideAnimationEvent(),
            ()=>
            {
                onStart?.Invoke();
                OnStartHideAnimationEvent();
            },
            ()=>
            {
                onFinish?.Invoke();
                OnFinishHideAnimationEvent();
            });
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

        if(useTimeScale)
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

    protected virtual UIAnimationEventManager.EventParams[] GetHideAnimationEvent()
    {
        if(pannel != null)
        {
            return UIAnimationEventManager.GetUsePannelFadeOut(GetAllGraphics(), pannel);
        }

        return UIAnimationEventManager.GetFade(GetAllGraphics(),0f,0.5f);
    }

    protected virtual UIAnimationEventManager.EventParams[] GetShowAnimationEvent()
    {
        if (pannel != null)
        {
            return UIAnimationEventManager.GetUsePannelFadeIn(GetAllGraphics(), pannel);
        }

        return UIAnimationEventManager.GetFade(GetAllGraphics(), 1f, 0.5f);
    }

    private void OnDestroy()
	{
        views.Remove(this);
	}

    protected virtual void Awake()
    {
        transform.localPosition = Vector3.zero;
        views.Add(this);
        graphics = GetComponentsInChildren<Graphic>();
        gameObject.SetActive(false);
    }

    public Graphic[] GetAllGraphics()
    {
        return GetComponentsInChildren<Graphic>();
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

    public virtual void OnGameOver(InventoryList reconsitutionItems, string KillerSpawnPointID)
    {
    }
}
