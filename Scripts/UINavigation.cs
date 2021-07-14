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
    }

    public IEventHandler EventHandler { set; get; }

    public readonly Stack<UIView> history = new Stack<UIView>();

    public UIView Current
    {
        set
        {
            if(current != null)
            {
                current.EventHandler = null;
            }
            current = value;
            current.EventHandler = this;
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
        if (Current != null)
        {
            if(!view.isOverlay)
            {
                Current.Hide();
            }
        }

        Current = view;

        Current.Show();

        history.Push(view);
        return view;
    }
    public UIView Push(string pageName)
    {
        UIView page = UIView.Get(pageName);
        return Push(page);
    }

    public UIView Pop(bool hideImmediately = false, bool showImmediately = false)
    {
        if(Current != null)
        {
            EventHandler?.OnStartHide(current);

            if(hideImmediately)
            {
                current.HideImmediately();
            }
            else
            {
                Current.Hide();
            }

            history.Pop();
        }

        if (history.Count == 0)
        {
            Current = null;
        }
        else
        {
            Current = history.Peek();

            EventHandler?.OnStartShow(current);
            if(showImmediately)
            {
                current.ShowImmediately();
            }
            else
            {
                Current.Show();
            }
        }

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

    public UIView PopToRoot()
    {
        UIView pop = Pop();

        return history.Count == 1 ? pop : PopToRoot();
    }
    public void OnStartShowAnimationEvent(UIView view)
    {
        EventHandler?.OnStartShow(current);
    }
    public void OnFinishShowAnimationEvent(UIView view)
    {
        EventHandler?.OnFinishShow(view);
    }

    public void OnStartHideAnimationEvent(UIView view)
    {
        EventHandler?.OnStartHide(current);
    }

    public void OnFinishHideAnimationEvent(UIView view)
    {
        EventHandler?.OnFinishHide(view);
    }
}
