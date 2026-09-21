using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어가 소지한 도구(손전등)의 장착 및 on/off를 담당한다.
//
// 이동 로직과 도구 로직은 서로 다른 책임이다. 도구가 손전등 외에
// 연막탄 등으로 늘어나도(4차시 Strategy) PlayerMovement 코드는
// 전혀 건드릴 필요가 없도록 이동과 분리된 컴포넌트로 만든다.
public class PlayerTool : MonoBehaviour
{
    private PlayerInputSystem input;
    [Header("손전등")]
    public GameObject flashlightObject;
    public bool isFlashlightOn;

    private void Start()
    {
        input = GetComponent<PlayerInputSystem>();
    }

    void Update()
    {
        isFlashlightOn = input.isFlashlightOn;
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(isFlashlightOn);
        }
    }
}
