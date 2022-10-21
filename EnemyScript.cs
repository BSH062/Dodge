using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour {
	
	public List<string> _animationName= new List<string>();
	public float _speed;
	public float _attack;
	
	public GameObject _target;
	
	
	public float _hp;
	public GameObject _hpBar;
	public TextMesh _HpVal;
	public GameObject _HpObj;

    public GameObject _DamEffect;
    public GameObject _DamText;

    public float _timerForLv;
    public float _timerForLvLim;

    public AudioClip _damageSnd;

	// Use this for initialization
	void Start () {
		
		GetComponent<Animation>()[_animationName[0]].layer = 0;
		GetComponent<Animation>()[_animationName[1]].layer = 1;
		GetComponent<Animation>()[_animationName[2]].layer = 3;
		GetComponent<Animation>()[_animationName[3]].layer = 4;
		GetComponent<Animation>().CrossFade(_animationName[0],0.1f); //애니메이션 전환을 부드럽게 함 클립 명칭,fadeout 되는시간
		GetComponent<Animation>()[_animationName[2]].speed = 2.0f;
		GetComponent<Animation>()[_animationName[3]].speed = 2.0f;
		_target = GameObject.FindWithTag ("player");
		
	
	}
	void Update () {

        _timerForLv += Time.deltaTime;
        if (_timerForLv > _timerForLvLim) //시간이 흐를수록 에너미 속도를 올려줌
        {
            _speed += 0.5f;
            _timerForLv = 0;
        }
		
		if(_target!=null)
		{//타겟이 살아있다면 타겟 포지션방향으로 이동 , Walk모션 실행 , 타겟방향으로 바라봄
			transform.position += (_target.transform.position - transform.position).normalized * _speed * Time.deltaTime;
			GetComponent<Animation>().CrossFade(_animationName[1],0.1f);
			transform.LookAt(_target.transform);
			
			if ((_target.transform.position - transform.position).magnitude < 10.0f)
			{//타겟과 나의 위치의 벡터길이를 반환하고 그게 10보다 작다면 공격모션 실행 
				GetComponent<Animation>().CrossFade(_animationName[2],0.1f);
			}
			else	
			{//아니라면 공격모션 멈춤
				GetComponent<Animation>().Stop(_animationName[2]);
			}
		}

	}
	
	void Damaged(float _dam) //에너미 어택 스크립드에서 부를것
	{
        if (GetComponent<AudioSource>() != null) GetComponent<AudioSource>().PlayOneShot(_damageSnd);
		_hp -= _dam;
        GetComponent<Animation>().CrossFade(_animationName[3], 0.1f);
        if(_DamEffect!=null) Instantiate(_DamEffect, new Vector3(transform.position.x, 2.0f, transform.position.z), Quaternion.identity);
		if (_DamText != null)
			Instantiate(_DamText, new Vector3(transform.position.x, 1.2f, transform.position.z + 0.2f), Quaternion.identity);
		
		if(_hp >0)
		{
			if(_hpBar!=null) _hpBar.transform.localScale = new Vector3 (_hp*0.01f,1,1);
			if(_HpVal!=null)_HpVal.text = _hp.ToString();
		}
		else if(_hp <= 0)
		{
			if(_hpBar!=null) _hpBar.transform.localScale = new Vector3 (0,1,1);
			if(_HpVal!=null) _HpVal.text = "0";

            _target.GetComponent<PlayerScript>()._gameWin = true;
            _target.GetComponent<PlayerScript>().GameOver();
			
		}

	}
}
