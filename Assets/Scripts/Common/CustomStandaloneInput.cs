using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomStandaloneInput : StandaloneInputModule
{
    /// <summary>
    /// マウスを有効にするか
    /// </summary>
    [SerializeField]
    bool enabledMouse;
    public bool EnabledMouse {
        set { enabledMouse = value; }
        get { return enabledMouse; }
    }
    public override void Process()
    {

        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents) {
            if (!usedEvent)
                usedEvent |= SendMoveEventToSelectedObject();

            if (!usedEvent)
                SendSubmitEventToSelectedObject();
        }

        if (!enabledMouse) {
            return;
        }

        if (input.mousePresent) {
            ProcessMouseEvent();
        }
    }

	/*===============================================================*/
	/// <summary>マウスコントロール</summary>
	/// <remarks>
	/// 1を選択するとプレイモード中にマウスでの停止が出来なくなります
	/// DebugDisplayLog.csをカメラなどのオブジェクトにアタッチした状態でESCキーを押すことで停止できます
	/// (注意)1を選択してもマウスイベントは取得されます:中央にロックされるのみです
	/// </remarks>
	/// <param name="control">
	/// 1:マウスカーソルを中央にロックします
	/// 2:ウィンドウ内のみマウスカーソルを有効にします
	/// 3:マウスカーソルを自由に動かせるようにします
	/// </param>
	static public void MouseEnable( int control ) {
		if( control == 1 ) Cursor.lockState = CursorLockMode.Locked;
		else if( control == 2 ) Cursor.lockState = CursorLockMode.Confined;
		else if( control == 3 ) Cursor.lockState = CursorLockMode.None;
		else Debug.LogError( "代入値が範囲外です！確認して下さい！" );


	}
	/*===============================================================*/
}
