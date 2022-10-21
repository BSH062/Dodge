using UnityEngine;
using System.Collections;

public class AnimationChk : MonoBehaviour {

    public Animator _playerAnim;
    public AudioSource _publicAudio;
    public AudioClip _attackSnd;
    public AudioClip _damageSnd;

	// Use this for initialization
	void Start () {

        _playerAnim = gameObject.GetComponent<Animator>();
	
	}

    void AttackDone() //플레이어 공격후
    {
        if (_playerAnim != null) _playerAnim.SetBool("attackChk", false); //공격애니메이션 false로 
        else Debug.Log("Need Animator File"); //플레이어 애니메이션이 비어있다면 로그 출력
        if (_attackSnd != null && _publicAudio != null) _publicAudio.PlayOneShot(_attackSnd);//플레이어 사운드 출력
        else Debug.Log("Need AudioSource File or Attack Audio Clip");
    }

    void DamageDone()
    {
        if (_playerAnim != null) _playerAnim.SetBool("damageChk", false); //피격 모션 에서 다시 idle로 돌림
        else Debug.Log("Need Animator File");
        if (_damageSnd != null && _publicAudio != null) _publicAudio.PlayOneShot(_damageSnd);
        else Debug.Log("Need AudioSource File or Damage Audio Clip");
        
    }
}
