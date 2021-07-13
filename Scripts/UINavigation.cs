using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation
{
    public readonly Stack<UIView> history = new Stack<UIView>();

    public UIView Current { private set; get; }

    public UIView Push(UIView view,bool hideAnimation = false)
    {
        if (Current != null)
        {
            if(!view.isOverlay)
            {
                if(hideAnimation)
                {
                    Current.Hide();
                }
                else
                {
                    Current.HideImmediately();
                }
            }
        }

        Current = view;
        view.Show();

        history.Push(view);
        return view;
    }

    public UIView Push(string pageName, bool hideAnimation = false)
    {
        UIView page = UIView.Get(pageName);
        return Push(page,hideAnimation);
    }

    public UIView Pop()
    {
        if(Current != null)
        {
            Current.Hide();
            history.Pop();
        }

        if (history.Count == 0)
        {
            Current = null;
        }
        else
        {
            Current = history.Peek();
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
}
