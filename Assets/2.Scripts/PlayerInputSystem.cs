using UnityEngine;
using UnityEngine.InputSystem;

// 키보드에서 원시 입력(방향, 달리기 키 여부)만 읽어 다른 컴포넌트에 제공
//
public class PlayerInputSystem : MonoBehaviour
{
    public Vector2 moveInput;
    public bool runHeld;

    void Update()
    {
        // 키보드 읽기로 임시 구현하였습니다.
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            moveInput = Vector2.zero;
            runHeld = false;
            return;
        }

        float h = 0f;
        float v = 0f;

        if (keyboard.aKey.isPressed)
        {
            h -= 1f;
        }
        if (keyboard.dKey.isPressed)
        {
            h += 1f;
        }
        if (keyboard.sKey.isPressed)
        {
            v -= 1f;
        }
        if (keyboard.wKey.isPressed)
        {
            v += 1f;
        }

        moveInput = new Vector2(h, v);
        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }

        runHeld = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;
    }
}
