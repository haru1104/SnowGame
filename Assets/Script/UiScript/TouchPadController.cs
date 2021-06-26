using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPadController : MonoBehaviour
{
    //TouchPad 오브젝트의 Rect Transform을 가져온다
    private RectTransform _touchPad;

    // 터치 입력 중에 방향 컨트롤러의 영역 안에 있는 입력을 구분하기 위한 아이디
    private int _touchID = -1;

    // 터치 입력이 시작되는 좌표
    private Vector3 _startPos = Vector3.zero;

    // 방향 컨트롤러가 원으로 움직일 수 있는 반지름
    public float _dragRadius = 60f;

    // 플레이어의 움직임을 관리하는 PlayerMovement 스크립트와 연결
    // 방향키가 변경되면 캐릭터에게 신호를 보내야하기 때문 (만들고 주석 해제)
    public PlayerController _player;

    // 버튼이 눌렸는지 체크하는 변수
    private bool _buttonPressed = false;
    
    void Start()
    {
        // TouchPad 오브젝트 가져오기
        _touchPad = GetComponent<RectTransform>();

        // TouchPad의 좌표를 가져와 이 좌표를 움직임의 기준값으로 사용
        _startPos = _touchPad.position;
       
    }

    void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    // 버튼이 눌렸을 경우 (버튼을 누르고 있는 경우)
    public void OnButtonDown()
    {
        _buttonPressed = true;
    }

    // 버튼이 눌리지 않았을 경우 (버튼에서 손을 땐 경우)
    public void OnButtonUp()
    {
        _buttonPressed = false;

        // 버튼에서 손을 땠을 때 TouchPad의 좌표를 원래 지점으로 복귀
        HandleInput(_startPos);
    }

    void HandleInput(Vector3 input)
    {
        // 버튼이 눌려져 있을 경우
        if (_buttonPressed)
        {
            // 방향 컨트롤러의 기준 좌표로부터 입력 받은 좌표가 얼마나 떨어져있는지 계산
            Vector3 diffVec = (input - _startPos);

            // 입력 지점과 기준 좌표의 거리를 계산한 값이 최대치보다 큰 경우
            if (diffVec.sqrMagnitude > _dragRadius * _dragRadius)
            {
                // 방향 벡터의 크기를 1로 만듦
                diffVec.Normalize();

                // 방향 컨트롤러가 최대치 이상으로 움직이지 못하게 제한
                _touchPad.position = _startPos + diffVec * _dragRadius;
            }
            // 최대치보다 작은 경우
            else
            {
                // 해당 위치로 컨트롤러 이동
                _touchPad.position = input;
            }
        }
        // 버튼에서 손을 땐 경우
        else
        {
            // 원래 위치로 되돌려둠
            _touchPad.position = _startPos;
        }

        // 방향키와 기준 좌표의 차이
        Vector3 diff = _touchPad.position - _startPos;

        // 방향키의 방향을 유지한 채로, 거리를 나누어 방향만 구함
        Vector3 normalDiff = new Vector3(diff.x / _dragRadius, diff.y / _dragRadius); 
        
        // 플레이어가 빈 오브젝트가 아니라면 (널 체킹)
        if (_player != null &&_player.isDead == false)
        {
            // 플레이어가 연결되어 있으면 플레이어에게 변경된 좌표를 전달
            _player.OnStickChanged(normalDiff); 
        }
    }

    // 물리 연산이 가능한 업데이트
    private void FixedUpdate()
    {
        // 모바일에서는 터치 패드 방식으로 여러 터치 입력을 받아 처리
        HandleTouchInput();

        // 모바일이 아닌 PC나 유니티 에디터 상에서 작동할 때는 터치 입력이 아닌 마우스로 입력을 받아 처리
        // C# 전처리기 기능을 사용
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
        HandleInput(Input.mousePosition);
#endif
    }

    void HandleTouchInput()
    {
        // 터치 아이디를 매기기 위한 변수
        int id = 0;

        // 터치는 다중 입력을 지원하기 때문에 하나 이상 입력이 되면 실행
        if (Input.touchCount > 0)
        {
            // 입력된 터치들을 하나씩 조회
            foreach (Touch t in Input.touches)
            {
                // 터치 아이디를 매기기 위한 번호를 1씩 증가
                id++;

                // 현재 터치 입력의 x, y좌표를 구함
                Vector3 tPos = new Vector3(t.position.x, t.position.y);

                // 터치 입력이 방금 시작되었다면
                if (t.phase == TouchPhase.Began)
                {
                    // 그리고 터치의 좌표가 현재 방향키 범위 내에 있다면
                    if (t.position.x <= (_startPos.x + _dragRadius))
                    {
                        // 현재 터치 아이디를 기준으로 방향 컨트롤러를 조작하도록 함
                        _touchID = id;
                    }
                }

                // 터치 입력이 움직였다거나, 가만히 있는 상황인 경우
                if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                {
                    // 터치 아이디로 지정된 경우에만
                    if (_touchID == id)
                    {
                        // 좌표 입력을 받아들임
                        HandleInput(tPos);
                    }
                }

                // 터치 입력이 끝난 경우
                if (t.phase == TouchPhase.Ended)
                {
                    // 입력 받고 있던 터치 아이디인 경우
                    if (_touchID == id)
                    {
                        // 터치 아이디 지정 해제
                        _touchID = -1;
                    }
                }
            }
        }
    }
}
