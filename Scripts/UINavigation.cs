using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation
{
    public readonly Stack<UIView> history = new Stack<UIView>();

    public UIView Current { private set; get; }

    public UIView Push(UIView view)
    {
        if (Current != null)
        {
            if(!Current.isAlwaysShow)
            {
                Current.Hide();
            }
        }

        UIView page = view;
        page.Show();

        Current = page;
        history.Push(page);
        return page;
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
            history.Pop();
        }

        if (history.Count == 0)
        {
            return null;
        }

        Current = history.Peek();
        Current.Show();
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
