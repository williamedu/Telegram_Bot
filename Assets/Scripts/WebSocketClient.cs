using System;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using UnityEngine;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket ws;
    private string serverUrl = "ws://192.168.56.1:8765";  // Usa tu IP local

    async void Start()
    {
        Debug.Log("Intentando conectar al servidor WebSocket...");
        await ConnectWebSocket();
    }

    private async Task ConnectWebSocket()
    {
        ws = new WebSocket(serverUrl);

        ws.OnOpen += () => Debug.Log("Conectado al servidor WebSocket");

        ws.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Mensaje recibido: " + message);
        };

        ws.OnError += (error) =>
        {
            Debug.LogError("Error en WebSocket: " + error);
        };

        ws.OnClose += (code) =>
        {
            Debug.LogWarning("Conexión WebSocket cerrada con código: " + code);
        };

        try
        {
            await ws.Connect();
            Debug.Log("WebSocket conectado exitosamente.");
        }
        catch (Exception ex)
        {
            Debug.LogError("No se pudo conectar al WebSocket: " + ex.Message);
        }
    }

    private void Update()
    {
        if (ws != null)
        {
            ws.DispatchMessageQueue();
        }
    }

    private async void OnApplicationQuit()
    {
        if (ws != null)
        {
            await ws.Close();
        }
    }
}
