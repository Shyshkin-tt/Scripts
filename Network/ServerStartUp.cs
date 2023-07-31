
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;

public class ServerStartUp : MonoBehaviour
{
    //private const string _internalServerIP = "0.0.0.0"; // Внутренний IP сервера
    //private ushort _serverPort = 7777;


    private IMultiplayService _multiplayServices;
    //async void Start()
    //{
    //    bool server = false;
    //    var args = System.Environment.GetCommandLineArgs();
    //    for (int i = 0; i < args.Length; i++)
    //    {
    //        if (args[i] == "_dedicatedServer")
    //        {
    //            server = true;
    //        }
    //        if (args[i] == "-port" && (i + 1 < args.Length))
    //        {
    //            _serverPort = (ushort)int.Parse(args[i + 1]);
    //        }
    //    }

    //    if (server)
    //    {
    //        StartServer();
    //        //await StartServerServises();
    //    }

    //}

    //private void StartServer()
    //{
    //    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_internalServerIP, (ushort)_serverPort);
    //    NetworkManager.Singleton.StartServer();
    //}

    //async Task StartServerServises()
    //{
    //    await UnityServices.InitializeAsync();
    //    try
    //    {
    //        _multiplayServices = MultiplayService.Instance;
    //        await _multiplayServices.StartServerQueryHandlerAsync();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
}
