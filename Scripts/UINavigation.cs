using System;
using System.Collections;
using System.Collections.Generic;
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

    public UIView Push(UIView view)
    {
        Current = view;
        history.Push(view);
        Debug.Log("Push / Current = " + current.name + " / History Count = " + history.Count);
        return current;
    }

    public void Push(UIView view, bool hideImmediately, bool showImmediately, Action onStart = null, Action onFinish = null)
    {
        if (Current != null && !view.isOverlay)
        {
            Hide(current, hideImmediately, onStart,
                () =>
                {
                    Show(current, showImmediately, null, onFinish);
                });

            Push(view);
            return;
        }

        Push(view);
        Show(current, showImmediately, onStart, onFinish);
    }
    public UIView Push(string pageName)
    {
        UIView page = UIView.Get(pageName);
        return Push(page);
    }

    public void Hide(bool immediately, Action onStart, Action onFinish)
    {
        Hide(current, immediately, onStart, onFinish);
    }

    private void Hide(UIView view, bool immediately, Action onStart,Action onFinish)
    {
        EventHandler?.OnStartHide(view);

        if (immediately)
        {
            current.HideImmediately(onStart,onFinish);
        }
        else
        {
            Current.Hide(onStart, onFinish);
        }
    }

    public void Show(bool immediately, Action onStart, Action onFinish)
    {
        Show(current, immediately, onStart, onFinish);
    }

    private void Show(UIView view, bool immediately, Action onStart, Action onFinish)
    {
        EventHandler?.OnStartShow(view);
        if (immediately)
        {
            view.ShowImmediately(onStart, onFinish);
        }
        else
        {
            view.Show(onStart, onFinish);
        }
    }

    public void Pop(bool hideImmediately , bool showImmediately, Action onStart = null, Action onFinish = null)
    {
        UIView view = null;

        if (Current != null)
        {
            Hide(current, hideImmediately, onStart, 
                ()=>
                {
                    if (view != null)
                    {
                        Show(current, showImmediately, null, onFinish);
                    }
                });
            view = Pop();
            return;
        }

        view = Pop();

        if (view != null)
        {
            Show(current, showImmediately, null, onFinish);
        }
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

        Debug.Log("Pop / Current = " + current?.name + " / History Count = " + history.Count);

        return Current;
    }

    public UIView Pop<T>() where T : UIView
    {
        UIView pop = Pop();

        return pop is T ? pop : Pop<T>();
    }

    public UIView Pop(string pageName)
    {
        UIView pop = Pop();

        return pop.name == pageName ? pop : Pop(pageName);
    }

    public void PopToRoot(bool hideImmediately, bool showImmediately, Action onStart = null, Action onFinish = null)
    {
        UIView view = null;
        if (Current != null)
        {
            Hide(current, hideImmediately, onStart,
                () =>
                {

                    if (view != null)
                    {
                        Show(current, showImmediately, null, onFinish);
                    }
                });

            view = PopToRoot();
            return;
        }

        view = PopToRoot();

        if (view != null)
        {
            Show(current, showImmediately, null, onFinish);
        }
    }

    public UIView PopToRoot()
    {
        UIView pop = Pop();

        return history.Count == 1 ? pop : PopToRoot();
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
