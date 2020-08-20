using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    public bool isAlwaysShow;
    public UnityEngine.UI.Selectable first;

    private static List<UIView> views = new List<UIView>();

    protected virtual void Awake()
    {
        transform.localPosition = Vector3.zero;
        views.Add(this);
        gameObject.SetActive(false);
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

    public bool IsShowing()
    {
        return gameObject.activeSelf;
    }

    public void ShowImmediately()
    {
        gameObject.SetActive(true);

        if(first != null)
        {
            EventSystem.current.SetSelectedGameObject(first.gameObject);
            return;
        }

    }

    public void HideImmediately()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        ShowImmediately();
    }

    public void Hide()
    {
        HideImmediately();
    }
}
