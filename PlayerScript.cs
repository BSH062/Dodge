using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
	// �÷��̾� �⺻ ����
	
	public float _speed; // Ǯ���̾� �ӵ�   �ν����Ϳ��� ���������ϰ� �ϱ����� public

	public float _hp; // �÷��̾� ü��
	public TextMesh _HpVal; // UI text�� ǥ�õ� hp  �巡��&������� ������� �����
	public GameObject _hpBar;  //HP �ð��� ȿ�� 
	public Rigidbody Playerrig; //�÷��̾� ������ٵ� �޾ƿ�
    public Animator _rabbit; // �÷��̾ ����ִ� �ִϸ����� ����

	Vector3 velocity;  //�÷��̾� �ӵ� ����
	Vector3 movevelocity; //�÷��̾� ���� ����

	// ���� ��� �� ���� �ڵ�
	public bool _gameWin; // bool ���� true , false �� ��ȯ true == �̱�  
    public bool _playerLive = true; // �÷��̾ ����ֳ� 
	public GameObject _uiResult; // Result UI ������Ʈ �޾ƿ��� 
	public Text _resultText; // Result UI���� Text�� �̰���� ������ ǥ���ϱ����� �����ص�
	
	public BoxCollider _attackChkCol; // �÷��̾ �����Ҷ�,���Ҷ�  �ݶ��̴� on/off ��Ű�� ���� �޾ƿ�  
	public GameObject _DamEffect; // ������ ����Ʈ
    public GameObject _DamText; // ������ ��ġ 
    private bool _attackChkbool; //���� �ߴ��� ���� üũ�� �����ϸ� true

	void Start ()
	{
		Playerrig= GetComponent<Rigidbody>();
		if (GetComponent<AudioSource>() != null) GetComponent<AudioSource>().Play(); //�÷��̾ �������ִ� ����� �ҽ��� ��������ʴٸ� �÷��� ���ּ��� 
		if (_rabbit == null) //�÷��̾� �ν����Ϳ� �ִϸ����Ͱ� ���� ��� �޾ƿ��� �ִϸ��̼� �ӵ��� 2.0f ���� �޾ƿ� (��� �ִϸ��̼� �ӵ��� 2�� �����)
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
		if(_playerLive) //�÷��̾ ����ִ� ��쿡�� �۵�
		{
			moveInput();
			transform.LookAt(transform.position + movevelocity); //�÷��̾ �ٶ� ����
			
			if((_rabbit != null))
			{
                if (Input.GetButtonDown("Fire1")) //���콺 ����Ű�� ������ ���� 
                {                    
                    _rabbit.SetBool("attackChk", true); //���� �ִϸ��̼� true�� ����
                    if (_attackChkCol != null) //������ ���� �ݶ��̴� �޾ƿ� �����ݶ��̴��� ���� �Ѵٸ� 
                    {
                        _attackChkCol.enabled = true; //�ݶ��̴��� Ȱ��ȭ ������ 
                    }
                    _attackChkbool = true; //���� ���¸� true�� �ٲ��� 
                }
                else
                {
                    if (_attackChkCol != null) _attackChkCol.enabled = false; //����Ű�� ������ �ʾҴٸ�  ���� �ݶ��̴��� ��Ȱ��ȭ 
                }
                if (_rabbit.GetBool("damageChk")) //������ �Ծ����� ��� 
                {
                    if (_rabbit.GetCurrentAnimatorStateInfo(0).IsName("3_damage")) //�ִϸ��̼� �̸��� �������� �ƴҰ�� ���� 
                    {
                        if (_rabbit.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.0f) _rabbit.SetBool("damageChk", false); //�ִϸ��̼� �ӵ� 0, 1 �� �����ϴµ� �׳� �ִϸ����� Has Exit Time �� �����ϴ°� ������ ���� 
                    }
                }   
		    }
        }
	}
	void moveInput()
    {
		Vector3 moveinput = new Vector3(-1*Input.GetAxisRaw("Horizontal"), 0, -1*Input.GetAxisRaw("Vertical")); //ī�޶� �ݴ�� ��ġ�ؼ� �Է°��� -1 ����� �����̸���

		movevelocity = moveinput.normalized * _speed; //���⿡ �ӷ��� ������

		Move(movevelocity);
	}
	void Move(Vector3 movevelocity)
	{
		this.velocity = movevelocity;
		_rabbit.SetBool("runChk", this.velocity != Vector3.zero); //���ν�Ƽ�� 0 �̾ƴѰ�� (����Ű �������ִ»���) true ,�ƴϸ� false�� ����
	}
	void Damaged(float _dam) //���߿� ���� ��ũ��Ʈ���� sendmassage�� �ҷ� �� 
	{
		_hp -= _dam;
        if(!_rabbit.GetCurrentAnimatorStateInfo(0).IsName("3_damage")) _rabbit.SetBool("damageChk", true); //�����ϴ� �ִϸ��̼��� �������� �ƴѰ�� ������üũ true�� ���� (������ �ִϸ��̼ǽ���)
		if(_DamEffect!=null) Instantiate(_DamEffect,new Vector3(transform.position.x, 1.0f, transform.position.z),Quaternion.identity); //������ ����Ʈ ���� (�������� �ν���Ʈȭ ��Ŵ)
        if(_DamText!=null) Instantiate(_DamText, new Vector3(transform.position.x, 1.2f, transform.position.z + 0.2f), Quaternion.identity);//������ ��ġ(text) ����
        
		if(_hp >0) //hp�� 0�̻��� ��� hpbar x�� ���� , hptext ��ġ ���ҵ� ��ŭ �ٽ� ���� 
		{
			if(_hpBar!=null) _hpBar.transform.localScale = new Vector3 (_hp*0.01f,1,1);
			if(_HpVal!=null) _HpVal.text = _hp.ToString();
		}
		else if(_hp <= 0) //hp�� 0�̳� �����ϰ� �Ǵ� ���� 
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
        Application.LoadLevel("1_play"); //Application.LoadLevel������ ���� ���ϴ� �Լ��� SceneManager.LoadScene()�� ���°� ������
    }
}
