using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private FixedJoystick joystick;

    private float elapsedTime = 0f;
    private bool isCounting = false;

    private void OnEnable()
    {
        elapsedTime = 0f;
        isCounting = true;
    }

    private void OnDisable()
    {
        isCounting = false;
        joystick.ResetJoystick();
    }

    private void Update()
    {
        if (!isCounting) return;

        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerTxt.text = $"{minutes:00}:{seconds:00}";

        // 🚀 Gửi input từ joystick sang Player
        if (LevelManager.Ins != null && LevelManager.Ins.curLevel != null)
        {
            var player = LevelManager.Ins.curLevel.playerCtrl;
            if (player != null)
            {
                PlayerController controller = player.GetComponent<PlayerController>();
                if (controller != null)
                {
                    Vector3 direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
                    controller.SetInputDirection(direction);
                }
            }
        }
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });
    }
}
