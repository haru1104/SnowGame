using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPadController : MonoBehaviour
{
    //TouchPad ������Ʈ�� Rect Transform�� �����´�
    private RectTransform _touchPad;

    // ��ġ �Է� �߿� ���� ��Ʈ�ѷ��� ���� �ȿ� �ִ� �Է��� �����ϱ� ���� ���̵�
    private int _touchID = -1;

    // ��ġ �Է��� ���۵Ǵ� ��ǥ
    private Vector3 _startPos = Vector3.zero;

    // ���� ��Ʈ�ѷ��� ������ ������ �� �ִ� ������
    public float _dragRadius = 60f;

    // �÷��̾��� �������� �����ϴ� PlayerMovement ��ũ��Ʈ�� ����
    // ����Ű�� ����Ǹ� ĳ���Ϳ��� ��ȣ�� �������ϱ� ���� (����� �ּ� ����)
    public PlayerController _player;

    // ��ư�� ���ȴ��� üũ�ϴ� ����
    private bool _buttonPressed = false;
    
    void Start()
    {
        // TouchPad ������Ʈ ��������
        _touchPad = GetComponent<RectTransform>();

        // TouchPad�� ��ǥ�� ������ �� ��ǥ�� �������� ���ذ����� ���
        _startPos = _touchPad.position;
       
    }

    void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    // ��ư�� ������ ��� (��ư�� ������ �ִ� ���)
    public void OnButtonDown()
    {
        _buttonPressed = true;
    }

    // ��ư�� ������ �ʾ��� ��� (��ư���� ���� �� ���)
    public void OnButtonUp()
    {
        _buttonPressed = false;

        // ��ư���� ���� ���� �� TouchPad�� ��ǥ�� ���� �������� ����
        HandleInput(_startPos);
    }

    void HandleInput(Vector3 input)
    {
        // ��ư�� ������ ���� ���
        if (_buttonPressed)
        {
            // ���� ��Ʈ�ѷ��� ���� ��ǥ�κ��� �Է� ���� ��ǥ�� �󸶳� �������ִ��� ���
            Vector3 diffVec = (input - _startPos);

            // �Է� ������ ���� ��ǥ�� �Ÿ��� ����� ���� �ִ�ġ���� ū ���
            if (diffVec.sqrMagnitude > _dragRadius * _dragRadius)
            {
                // ���� ������ ũ�⸦ 1�� ����
                diffVec.Normalize();

                // ���� ��Ʈ�ѷ��� �ִ�ġ �̻����� �������� ���ϰ� ����
                _touchPad.position = _startPos + diffVec * _dragRadius;
            }
            // �ִ�ġ���� ���� ���
            else
            {
                // �ش� ��ġ�� ��Ʈ�ѷ� �̵�
                _touchPad.position = input;
            }
        }
        // ��ư���� ���� �� ���
        else
        {
            // ���� ��ġ�� �ǵ�����
            _touchPad.position = _startPos;
        }

        // ����Ű�� ���� ��ǥ�� ����
        Vector3 diff = _touchPad.position - _startPos;

        // ����Ű�� ������ ������ ä��, �Ÿ��� ������ ���⸸ ����
        Vector3 normalDiff = new Vector3(diff.x / _dragRadius, diff.y / _dragRadius); 
        
        // �÷��̾ �� ������Ʈ�� �ƴ϶�� (�� üŷ)
        if (_player != null &&_player.isDead == false)
        {
            // �÷��̾ ����Ǿ� ������ �÷��̾�� ����� ��ǥ�� ����
            _player.OnStickChanged(normalDiff); 
        }
    }

    // ���� ������ ������ ������Ʈ
    private void FixedUpdate()
    {
        // ����Ͽ����� ��ġ �е� ������� ���� ��ġ �Է��� �޾� ó��
        HandleTouchInput();

        // ������� �ƴ� PC�� ����Ƽ ������ �󿡼� �۵��� ���� ��ġ �Է��� �ƴ� ���콺�� �Է��� �޾� ó��
        // C# ��ó���� ����� ���
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
        HandleInput(Input.mousePosition);
#endif
    }

    void HandleTouchInput()
    {
        // ��ġ ���̵� �ű�� ���� ����
        int id = 0;

        // ��ġ�� ���� �Է��� �����ϱ� ������ �ϳ� �̻� �Է��� �Ǹ� ����
        if (Input.touchCount > 0)
        {
            // �Էµ� ��ġ���� �ϳ��� ��ȸ
            foreach (Touch t in Input.touches)
            {
                // ��ġ ���̵� �ű�� ���� ��ȣ�� 1�� ����
                id++;

                // ���� ��ġ �Է��� x, y��ǥ�� ����
                Vector3 tPos = new Vector3(t.position.x, t.position.y);

                // ��ġ �Է��� ��� ���۵Ǿ��ٸ�
                if (t.phase == TouchPhase.Began)
                {
                    // �׸��� ��ġ�� ��ǥ�� ���� ����Ű ���� ���� �ִٸ�
                    if (t.position.x <= (_startPos.x + _dragRadius))
                    {
                        // ���� ��ġ ���̵� �������� ���� ��Ʈ�ѷ��� �����ϵ��� ��
                        _touchID = id;
                    }
                }

                // ��ġ �Է��� �������ٰų�, ������ �ִ� ��Ȳ�� ���
                if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                {
                    // ��ġ ���̵�� ������ ��쿡��
                    if (_touchID == id)
                    {
                        // ��ǥ �Է��� �޾Ƶ���
                        HandleInput(tPos);
                    }
                }

                // ��ġ �Է��� ���� ���
                if (t.phase == TouchPhase.Ended)
                {
                    // �Է� �ް� �ִ� ��ġ ���̵��� ���
                    if (_touchID == id)
                    {
                        // ��ġ ���̵� ���� ����
                        _touchID = -1;
                    }
                }
            }
        }
    }
}
