using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MT5BalanceOnly : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private string balanceFormat = "Balance: ${0:N2}";

    private string currentAccountId;

    private void Start()
    {
        // Obtener el ID guardado en PlayerPrefs
        currentAccountId = PlayerPrefs.GetString("CurrentAccountID", "0");

        // Suscribirse al evento del WebSocket
        if (MT5DataManager.Instance != null)
        {
            MT5DataManager.Instance.onDataReceived.AddListener(OnDataReceived);
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento
        if (MT5DataManager.Instance != null)
        {
            MT5DataManager.Instance.onDataReceived.RemoveListener(OnDataReceived);
        }
    }

    // Este método se llama cada vez que llegan nuevos datos del WebSocket
    private void OnDataReceived(Dictionary<long, MT5AccountData> accountsData)
    {
        // Obtener solo los datos de la cuenta actual
        if (long.TryParse(currentAccountId, out long accountId) &&
            accountsData.TryGetValue(accountId, out MT5AccountData accountData))
        {
            // Mostrar solo el balance
            UpdateBalanceUI(accountData.balance);
        }
    }

    private void UpdateBalanceUI(double balance)
    {
        if (balanceText != null)
        {
            balanceText.text = string.Format(balanceFormat, balance);
        }
    }
}