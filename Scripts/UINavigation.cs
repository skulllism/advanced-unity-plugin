using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UINavigation : UIView.IEventHandler
{
    public interface IEventHandler
    {
        void OnStartShow(UIView view);
        void OnFinishShow(UIView view);
        void OnStartHide(UIView view);
        void OnFinishHide(UIView view);
        void OnChangeView(UIView view);

        void OnStartShowAnimationEvent(UIView view);
        void OnFinishShowAnimationEvent(UIView view);
        void OnStartHideAnimationEvent(UIView view);
        void OnFinishHideAnimationEvent(UIView view);
    }

    public IEventHandler EventHandler { set; get; }

    public readonly Stack<UIView> history = new Stack<UIView>();

    public UIView Current
    {
        set
        {
            current = value;
            if(current != null)
            {
                current.EventHandler = this;
            }
            EventHandler?.OnChangeView(current);
        }
        get
        {
            return current;
        }
    }

    private UIView current;

    public List<UIView> GetCurrentShowing()
    {
        List<UIView> list = new List<UIView>();

        foreach (var view in history)
        {
            if(view.gameObject.activeSelf)
            {
                list.Add(view);
            }
        }

        return list;
    }

    public UIView Push(UIView view)
    {
        Current = view;
        history.Push(view);
        //Debug.Log("Push / Current = " + current.name + " / History Count = " + history.Count);
        return current;
    }

    public UIView Push(string pageName)
    {
        UIView page = UIView.Get(pageName);
        return Push(page);
    }

    public UIView Pop()
    {
        if(Current != null)
        {
            history.Pop();
        }

        if (history.Count == 0)
        {
            Current = null;
        }
        else
        {
            Current = history.Peek();
        }

        //Debug.Log("Pop / Current = " + current?.name + " / History Count = " + history.Count);

        return Current;
    }

    public UIView Pop<T>() where T : UIView
    {
        UIView pop = Pop();

        return pop is T ? pop : Pop<T>();
    }

    public UIView PopTo(string viewName)
    {
        UIView pop = Pop();

        return pop.name == viewName ? pop : PopTo(viewName);
    }

    public UIView Pop(string viewName)
    {
        if(!history.Contains(UIView.Get(viewName)))
        { 
            return null;
        }

        List<UIView> list = history.ToList();

        foreach (var item in list)
        {
            if(item.name == viewName)
            {
                list.Remove(item);
                break;
            }
        }

        return null;
    }

    public UIView PopToRoot()
    {
        UIView pop = Pop();

        return history.Count <= 1 ? pop : PopToRoot();
    }
    public void OnStartShowAnimationEvent(UIView view)
    {
        EventHandler?.OnStartShowAnimationEvent(view);
    }
    public void OnFinishShowAnimationEvent(UIView view)
    {
        EventHandler?.OnFinishShowAnimationEvent(view);
        EventHandler?.OnFinishShow(view);
    }

    public void OnStartHideAnimationEvent(UIView view)
    {
        EventHandler?.OnStartHideAnimationEvent(view);
    }

    public void OnFinishHideAnimationEvent(UIView view)
    {
        EventHandler?.OnFinishHideAnimationEvent(view);
        EventHandler?.OnFinishHide(view);
    }
}
