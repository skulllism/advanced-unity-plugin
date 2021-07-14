using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation : UIView.IEventHandler
{
    public interface IEventHandler
    {
        void OnHide(UIView view);
        void OnFinishShowAnimationEvent(UIView view);
    }

    public IEventHandler EventHandler { set; get; }

    public readonly Stack<UIView> history = new Stack<UIView>();

    public UIView Current { private set; get; }

    public UIView Push(UIView view)
    {
        if (Current != null)
        {
            if(!view.isOverlay)
            {
                Current.Hide();
                Current.EventHandler = null;
            }
        }

        Current = view;
        Current.EventHandler = this;
        view.Show();

        history.Push(view);
        return view;
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
            Current.Hide();
            Current.EventHandler = null;
            history.Pop();
        }

        if (history.Count == 0)
        {
            Current = null;
        }
        else
        {
            Current = history.Peek();
            Current.EventHandler = this;
            Current.Show();
        }
  
        return Current;
    }

    public UIView Pop(string pageName)
    {
        UIView pop =  Pop();

        return pop.name == pageName ? pop : Pop(pageName);
    }

    public UIView PopToRoot()
    {
        UIView pop = Pop();

        return history.Count == 1 ? pop : PopToRoot();
    }
    public void OnStartShowAnimationEvent(UIView view)
    {
    }
    public void OnFinishShowAnimationEvent(UIView view)
    {
        EventHandler?.OnFinishShowAnimationEvent(view);
    }

    public void OnStartHideAnimationEvent(UIView view)
    {
        EventHandler?.OnHide(view);
    }

    public void OnFinishHideAnimationEvent(UIView view)
    {
    }
}
