using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어가 소지한 도구(손전등)의 장착 및 on/off를 담당한다.
//
// 이동 로직과 도구 로직은 서로 다른 책임이다. 도구가 손전등 외에
// 연막탄 등으로 늘어나도(4차시 Strategy) PlayerMovement 코드는
// 전혀 건드릴 필요가 없도록 이동과 분리된 컴포넌트로 만든다.
public class PlayerTool : MonoBehaviour
{
    [Header("손전등")]
    public GameObject flashlightObject;
    public bool isFlashlightOn;

    void Update()
    {
        // PlayerMovement와 동일하게 Keyboard.current를 이용한
        // 임시 구현이다. F키로 손전등을 켜고 끈다.
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard.fKey.wasPressedThisFrame)
        {
            isFlashlightOn = !isFlashlightOn;

            if (flashlightObject != null)
            {
                flashlightObject.SetActive(isFlashlightOn);
            }
        }
    }
}
