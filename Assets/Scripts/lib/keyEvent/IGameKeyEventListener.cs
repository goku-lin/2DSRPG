using System;

namespace lib.keyEvent
{
    // 游戏按键事件监听器接口
    public interface IGameKeyEventListener
    {
        /// <summary>
        /// 获取事件监听器的优先级，用于确定事件处理的顺序
        /// </summary>
        /// <returns></returns>
        int getPriority();

        /// <summary>
        /// 当游戏按键状态变化时触发的方法
        /// </summary>
        /// <param name="gameKey">keyEvent</param>
        /// <param name="keyState">表示按键的状态，如按下、释放等</param>
        /// <param name="hasBeenReceived">表示事件是否已被接收</param>
        /// <returns></returns>
        bool onGameKeyStateChanged(int gameKey, GAMEKEY_STATE keyState, bool hasBeenReceived);

        /// <summary>
        /// 
        /// </summary>当游戏轴/摇杆数值变化时触发的方法
        /// <param name="axisStick">表示轴/摇杆的标识</param>
        /// <param name="angle">表示轴的角度</param>
        /// <param name="radii">表示轴的半径</param>
        /// <param name="hasBeenReceived">表示事件是否已被接收</param>
        /// <returns></returns>
        bool onGameAxisStickValueChanged(int axisStick, float angle, float radii, bool hasBeenReceived);
    }
}
