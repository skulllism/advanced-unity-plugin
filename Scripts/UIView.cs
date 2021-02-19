using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VaporWorld;

public class UIView : MonoBehaviour
{
    public bool isAlwaysShow;
    public Graphic firstSelect;
    
    private int screenHeight;
    private int screenWidth;

    private static List<UIView> views = new List<UIView>();


    // UNITY VIEWPORT COORDINATE
    // (0.0, 0.0)--------------------(1.0, 0.0)
    // |                                      |
    // |                                      |
    // (0.0, 1.0)--------------------(1.0, 1.0)
    private void UIViewTranslate(Vector2 viewPortTranlation)
	{
        //UI 화면 비율 이동 구현


	}


    private void Awake()
    {
        transform.localPosition = Vector3.zero;
        views.Add(this);
        gameObject.SetActive(false);

        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

	private void OnDestroy()
	{
        views.Remove(this);
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

        if(firstSelect != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelect.gameObject);
            return;
        }

    }

    public void HideImmediately()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        ShowImmediately();
    }

    public void Hide()
    {
        HideImmediately();
    }
}
