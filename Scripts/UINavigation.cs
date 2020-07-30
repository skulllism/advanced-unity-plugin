using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation
{
    public readonly Stack<UIView> history = new Stack<UIView>();

    private UIView current = null;

    public UIView Push(UIView view)
    {
        if (current != null)
        {
            current.Hide();
        }

        UIView page = view;
        page.Show();

        current = page;
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
        if(current != null)
        {
            current.Hide();
            history.Pop();
        }

        if (history.Count == 0)
        {
            return null;
        }

        current = history.Peek();
        current.Show();
        return current;
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
