using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigationController
{
    private static UINavigation current;
    public static UINavigation Current
    {
        set
        {
            if(current != null)
            {
                foreach (var view in current.history)
                {
                    view.HideImmediately();
                }
            }

            current = value;
            current.Pop()?.Show();
        }
        get
        {
            return current;
        }
    }
} 
