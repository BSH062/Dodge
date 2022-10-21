using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
	// 플레이어 기본 설정
	
	public float _speed; // 풀래이어 속도   인스펙터에서 수정가능하게 하기위해 public

	public float _hp; // 플레이어 체력
	public TextMesh _HpVal; // UI text에 표시될 hp  드래그&드랍으로 끌고오면 적용됨
	public GameObject _hpBar;  //HP 시각적 효과 
	public Rigidbody Playerrig; //플레이어 리지드바디 받아옴
    public Animator _rabbit; // 플레이어가 들고있는 애니메이터 연결

	Vector3 velocity;  //플레이어 속도 저장
	Vector3 movevelocity; //플레이어 방향 저장

	// 게임 결과 를 위한 코드
	public bool _gameWin; // bool 값은 true , false 만 반환 true == 이김  
    public bool _playerLive = true; // 플레이어가 살아있나 
	public GameObject _uiResult; // Result UI 오브젝트 받아오기 
	public Text _resultText; // Result UI에서 Text로 이겼는지 졌는지 표시하기위해 연결해둠
	
	public BoxCollider _attackChkCol; // 플레이어가 공격할때,안할때  콜라이더 on/off 시키기 위해 받아옴  
	public GameObject _DamEffect; // 데미지 이펙트
    public GameObject _DamText; // 데미지 수치 
    private bool _attackChkbool; //공격 했는지 상태 체크용 공격하면 true

	void Start ()
	{
		Playerrig= GetComponent<Rigidbody>();
		if (GetComponent<AudioSource>() != null) GetComponent<AudioSource>().Play(); //플레이어가 가지고있는 오디오 소스가 비어있지않다면 플레이 해주세요 
		if (_rabbit == null) //플레이어 인스펙터에 애니메이터가 없을 경우 받아오고 애니메이션 속도를 2.0f 으로 받아옴 (모든 애니메이션 속도가 2로 변경됨)
		{
			_rabbit = gameObject.GetComponentInChildren<Animator>();
			_rabbit.speed = 2.0f; 
		}
	}
	private void FixedUpdate()
	{
		Playerrig.MovePosition(Playerrig.position + velocity * Time.fixedDeltaTime);
	}
	void Update () {
		if(_playerLive) //플레이어가 살아있는 경우에만 작동
		{
			moveInput();
			transform.LookAt(transform.position + movevelocity); //플레이어가 바라볼 방향
			
			if((_rabbit != null))
			{
                if (Input.GetButtonDown("Fire1")) //마우스 왼쪽키를 누르면 진행 
                {                    
                    _rabbit.SetBool("attackChk", true); //어택 애니메이션 true로 변경
                    if (_attackChkCol != null) //위에서 무기 콜라이더 받아옴 무기콜라이더가 존재 한다면 
                    {
                        _attackChkCol.enabled = true; //콜라이더를 활성화 시켜줌 
                    }
                    _attackChkbool = true; //공격 상태를 true로 바꿔줌 
                }
                else
                {
                    if (_attackChkCol != null) _attackChkCol.enabled = false; //왼쪽키를 누르지 않았다면  무기 콜라이더는 비활성화 
                }
                if (_rabbit.GetBool("damageChk")) //데미지 입었을때 모션 
                {
                    if (_rabbit.GetCurrentAnimatorStateInfo(0).IsName("3_damage")) //애니메이션 이름이 데미지가 아닐경우 실행 
                    {
                        if (_rabbit.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.0f) _rabbit.SetBool("damageChk", false); //애니메이션 속도 0, 1 로 구분하는데 그냥 애니메이터 Has Exit Time 을 조절하는게 더좋아 보임 
                    }
                }   
		    }
        }
	}
	void moveInput()
    {
		Vector3 moveinput = new Vector3(-1*Input.GetAxisRaw("Horizontal"), 0, -1*Input.GetAxisRaw("Vertical")); //카메라를 반대로 설치해서 입력값을 -1 해줘야 방향이맞음

		movevelocity = moveinput.normalized * _speed; //방향에 속력을 곱해줌

		Move(movevelocity);
	}
	void Move(Vector3 movevelocity)
	{
		this.velocity = movevelocity;
		_rabbit.SetBool("runChk", this.velocity != Vector3.zero); //벨로시티가 0 이아닌경우 (방향키 누르고있는상태) true ,아니면 false를 적용
	}
	void Damaged(float _dam) //나중에 공격 스크립트마다 sendmassage로 불러 옴 
	{
		_hp -= _dam;
        if(!_rabbit.GetCurrentAnimatorStateInfo(0).IsName("3_damage")) _rabbit.SetBool("damageChk", true); //실행하는 애니메이션이 데미지가 아닌경우 데미지체크 true로 변경 (데미지 애니메이션실행)
		if(_DamEffect!=null) Instantiate(_DamEffect,new Vector3(transform.position.x, 1.0f, transform.position.z),Quaternion.identity); //데미지 이펙트 생성 (프리팹을 인스턴트화 시킴)
        if(_DamText!=null) Instantiate(_DamText, new Vector3(transform.position.x, 1.2f, transform.position.z + 0.2f), Quaternion.identity);//데미지 수치(text) 생성
        
		if(_hp >0) //hp가 0이상일 경우 hpbar x축 감소 , hptext 수치 감소된 만큼 다시 적용 
		{
			if(_hpBar!=null) _hpBar.transform.localScale = new Vector3 (_hp*0.01f,1,1);
			if(_HpVal!=null) _HpVal.text = _hp.ToString();
		}
		else if(_hp <= 0) //hp가 0이나 그이하가 되는 순간 
		{
			if(_hpBar!=null) _hpBar.transform.localScale = new Vector3 (0,1,1);
			_playerLive=false;
			if(_HpVal!=null) _HpVal.text = "0";
			_gameWin=false;
			GameOver();
		}
	}
	public void GameOver()
	{
		//game over
        if (_gameWin)
        {
            if(_resultText != null) _resultText.text = "WIN";
        }
        else
        {
            if(_resultText != null)_resultText.text = "LOSE";
        }
        
        //
		Time.timeScale = 0.0f;
		if(_uiResult != null) _uiResult.SetActive(true);
	}

    public void Regame()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel("1_play"); //Application.LoadLevel은이제 지원 안하는 함수임 SceneManager.LoadScene()로 쓰는게 좋다함
    }
}
