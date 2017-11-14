using UnityEngine;

/*===============================================================*/
/// <summary>
/// @brief Player の移動処理を管理します プレイヤーオブジェクトに割り当てます
/// </summary>
/*===============================================================*/
public class PlayerMover : MonoBehaviour {

	// プレイヤー制御用 Rigidbody2D
	private Rigidbody2D rbody;
	// プレイヤー移動速度固定値
	//[SerializeField, TooltipAttribute( "プレイヤー移動速度" )]
	private float MOVE_SPEED = 3.0f;
	// プレイヤー移動速度
	private float moveSpeed;
	/// <summary>プレイヤーの移動方向</summary>
	public enum MOVE_DIR {
		/// <summary>停止</summary>
		STOP,
		/// <summary>左移動</summary>
		LEFT,
		/// <summary>右移動</summary>
		RIGHT


	}
	/// <summary>
	/// 移動方向 静的にし, 継承先で値が取得出来るようにします
	/// </summary>
	static protected MOVE_DIR moveDirection = MOVE_DIR.STOP; // 初期状態は, 停止
	/// <summary>
	/// 移動方向 静的, 継承先で値が取得出来るようにします
	/// </summary>
	static public MOVE_DIR GetMove { get { return moveDirection; } }

	/*===============================================================*/
	/// <summary>
	/// @brief UnityEngine ライフサイクルによる初期化
	/// </summary>
	void Awake( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>
	/// @brief 初期化
	/// </summary>
	void Initialize( ) {
		// プレイヤーの RigidBody2D コンポーネントをセットしておきます
		rbody = GetComponent<Rigidbody2D>( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>
	/// @brief UnityEngine ライフサイクルによって毎フレーム呼ばれます
	/// </summary>
	void Update( ) {
		// キーを取得します
		if( Input.GetKeyUp( KeyCode.D ) ) moveDirection = MOVE_DIR.STOP;
		if( Input.GetKeyUp( KeyCode.A ) ) moveDirection = MOVE_DIR.STOP;
		if( Input.GetKeyDown( KeyCode.D ) ) moveDirection = MOVE_DIR.RIGHT;
		if( Input.GetKeyDown( KeyCode.A ) ) moveDirection = MOVE_DIR.LEFT;

		
	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>
	/// @brief UnityEngine ライフサイクルによって固定フレームレートで呼ばれます
	/// </summary>
	void FixedUpdate( ) {
		// プレイヤーの移動処理
		switch( moveDirection ) {
			// 停止
			case MOVE_DIR.STOP : {
				moveSpeed = 0.0f;
				break;

			}
			// 左に移動
			case MOVE_DIR.LEFT : {
				moveSpeed = MOVE_SPEED * -1;
				break;

			}
			// 右に移動
			case MOVE_DIR.RIGHT : {
				moveSpeed = MOVE_SPEED;
				break;

			}

		}
		// Rigidbody コンポーネントに速度を設定する
		if( rbody != null ) rbody.velocity = new Vector2( moveSpeed, rbody.velocity.y );

		
	}
	/*===============================================================*/


}