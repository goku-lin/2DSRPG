using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton : Button
{
    /// <summary>
    /// 长按时间(ms)
    /// </summary>
    public int longPressTime = 400;

    [SerializeField]
    public class LongClickEvent : UnityEvent { }

    private LongClickEvent onClickUpEvent = new LongClickEvent();
    private LongClickEvent onLongClickUpEvent = new LongClickEvent();
    private LongClickEvent onLongPressEvent = new LongClickEvent();

    /// <summary>
    /// 倒计时协程
    /// </summary>
    private Coroutine countDownCoroutine = null;

    /// <summary>
    /// 单击后抬起时响应，防止长按也响应
    /// </summary>
    public LongClickEvent OnClickUp
    {
        get { return onClickUpEvent; }
        set { onClickUpEvent = value; }
    }

    /// <summary>
    /// 长按后抬起时响应
    /// </summary>
    public LongClickEvent OnLongClickUp
    {
        get { return onLongClickUpEvent; }
        set { onLongClickUpEvent = value;}
    }

    /// <summary>
    /// 长按事件
    /// </summary>
    public LongClickEvent OnLongPress
    {
        get { return onLongPressEvent; }
        set { onLongPressEvent = value;}
    }

    private DateTime firstClickTime = default(DateTime);
    private DateTime firstClickTime_Up = default(DateTime);

    /// <summary>
    /// 单击的事件处理器
    /// </summary>
    private void ClickUpHandler()
    {
        OnClickUp?.Invoke();
        ResetTime();
    }

    /// <summary>
    /// 长按抬起的事件处理器
    /// </summary>
    private void LongClickUpHandler()
    {
        OnLongClickUp?.Invoke();
        ResetTime();
    }

    /// <summary>
    /// 长按事件处理器
    /// </summary>
    private void LongPressHandler()
    {
        OnLongPress?.Invoke();
        if (countDownCoroutine != null )
        {
            StopCoroutine(countDownCoroutine);
            countDownCoroutine = null;
        }
    }

    /// <summary>
    /// 鼠标按下时执行
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (firstClickTime.Equals(default(DateTime)))
        {
            firstClickTime = DateTime.Now;
            countDownCoroutine = StartCoroutine(CountDown());
        }
    }

    /// <summary>
    /// 鼠标抬起时执行
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        // 在鼠标抬起时事件触发，时差大于600ms触发
        if (!firstClickTime.Equals(default(DateTime)))
            firstClickTime_Up = DateTime.Now;
        if (!firstClickTime.Equals(default(DateTime)) && !firstClickTime_Up.Equals(default(DateTime)))
        {
            //计算两次时间间隔（ms）
            TimeSpan intervalTime = firstClickTime_Up - firstClickTime;
            if (intervalTime.TotalMilliseconds > longPressTime)
                LongClickUpHandler();
            else
            {
                ClickUpHandler();
                ResetTime();
            }
        }
    }

    /// <summary>
    /// 鼠标移出
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ResetTime();
    }


    /// <summary>
    /// 重置记时
    /// </summary>
    private void ResetTime()
    {
        firstClickTime = default(DateTime);
        firstClickTime_Up = default(DateTime);
        if (countDownCoroutine != null)
        {
            StopCoroutine(countDownCoroutine);
            countDownCoroutine = null;
        }
    }

    /// <summary>
    /// 协程倒计时
    /// </summary>
    private IEnumerator CountDown()
    {
        TimeSpan temporary = firstClickTime - DateTime.Now;
        while (temporary.TotalMilliseconds < longPressTime)
        {
            temporary = DateTime.Now - firstClickTime;
            //yield return 后面的返回值永远只有一帧，不论时yield return 1还是yield return 100
            yield return null;
        }
        LongPressHandler();
    }
}
