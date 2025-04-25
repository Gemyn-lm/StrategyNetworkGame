using TMPro;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyInfoDisplay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image background;
    public NetworkPlayer affiliatedPlayer;

    private void Start()
    {
        if (affiliatedPlayer == null)
        {
            print("aff player is null");
            return;
        }
        affiliatedPlayer.networkPlayerName.OnValueChanged += OnNameChanged;
        affiliatedPlayer.isReady.OnValueChanged += UpdateColor;
        text.text = affiliatedPlayer.networkPlayerName.Value.ToString();

        UpdateColor(affiliatedPlayer.isReady.Value, affiliatedPlayer.isReady.Value);
    }

    private void OnDestroy()
    {
        affiliatedPlayer.networkPlayerName.OnValueChanged -= OnNameChanged;
        affiliatedPlayer.isReady.OnValueChanged -= UpdateColor;
    }

    public void UpdateColor(bool oldValue, bool newValue)
    {
        if (newValue)
            background.color = Color.green;
        else
            background.color = Color.red;

    }

    public void OnNameChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    {
        text.text = newValue.ToString();
    }
}
