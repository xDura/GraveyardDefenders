#if OLD_MULTI
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;

namespace XD.NET
{
    public class MultiplayerManager : Singleton<MultiplayerManager>
    {
        public static int dataBufferSize = 4097;
        const string serverIP = "127.0.0.1";
        const int port = 26950;
        const int maxPlayers = 4;

#if _XD_SERVER && _XD_CLIENT
        public Server server;
        public List<Client> clients = new List<Client>();
#elif _XD_SERVER
        public Server server;
#elif XD_CLIENT
        public Client client;
#endif

        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();

#if _XD_SERVER && _XD_CLIENT
            server = new Server();
            server.StartServer(maxPlayers, port);

            Client client = new Client();
            client.StartClient(IPAddress.Parse(serverIP), port);
            clients.Add(client);
#elif _XD_SERVER
            Server server = new Server();
            server.StartServer(maxPlayers, port);
#elif XD_CLIENT
            client = new Client();
            client.StartClient(IPAddress.Parse(serverIP), port);
#endif
        }

        public override void OnSingletonDestroy(bool isMainInstance)
        {
            base.OnSingletonDestroy(isMainInstance);
            if (isMainInstance)
            {
#if _XD_SERVER && _XD_CLIENT
                if (server != null) server.ShutDownServer();
                for (int i = 0; i < clients.Count; i++) clients[i].ShutDown();
#elif _XD_SERVER
                if (server != null) server.ShutDownServer();
#elif XD_CLIENT
                if (client != null) client.ShutDown();
#endif
            }
        }

        public void FixedUpdate()
        {
//#if _XD_SERVER
//            if (server != null)
//            {
//                foreach (KeyValuePair<int, TcpClient> clientPair in server.clients)
//                {
//                    clientPair.Value.
//                }
//            }
//#endif
        }

#if _XD_SERVER
        public class Server
        {
            public int maxPlayers;
            public int port;
            public string ip;
            public TcpListener listener;

            public Dictionary<int, ServerSocket> clients;
            
            public class ServerSocket
            {
                public int clientID;
                public TcpClient socket;
                public NetworkStream stream;
                public byte[] buffer;

                public void Connect(int id, TcpClient a_socket)
                {
                    clientID = id;
                    socket = a_socket;
                    stream = new NetworkStream(socket.Client);
                    buffer = new byte[dataBufferSize];
                    socket.GetStream().BeginRead(buffer, 0, dataBufferSize, OnDataReceived, null);
                }

                public void OnDataReceived(IAsyncResult result)
                {
                    Debug.Log($"Client Data Received {buffer}");
                }
            }

            public void StartServer(int a_maxPlayers, int a_port)
            {
                maxPlayers = a_maxPlayers;
                port = a_port;
                listener = new TcpListener(IPAddress.Loopback, port);
                BeginListen();
            }

            public void ShutDownServer()
            {
                listener.Stop();
            }

            void BeginListen()
            {
                listener.Start();
                listener.BeginAcceptTcpClient(new AsyncCallback(OnClientConnectRequest), null);
            }

            void OnClientConnectRequest(IAsyncResult result)
            {
                BeginListen();
                int foundKey = -1;
                for (int i = 0; i < maxPlayers - 1; i++)
                {
                    if (!clients.ContainsKey(i))
                    {
                        foundKey = i;
                        break;
                    }
                }
                if (foundKey != -1)
                {
                    TcpClient client = listener.EndAcceptTcpClient(result);
                    Debug.Log($"Client request: Accepting {client.Client.RemoteEndPoint}");
                    ServerSocket socket = new ServerSocket();
                    socket.Connect(foundKey, client);
                }
                else
                {
                    Debug.LogError("Shit happens");
                    return;
                }
                
            }
        }
#endif

#if _XD_CLIENT
        public class Client
        {
            public TcpClient socket;


            public void StartClient(IPAddress adress, int port)
            {
                socket = new TcpClient()
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                socket.BeginConnect(adress, port, OnConnectResponse, socket);
            }

            public void ShutDown()
            {
                socket.Close();
            }

            void OnConnectResponse(IAsyncResult result)
            {
                Debug.Log($"Server responed and connected = {socket.Connected}");
            }
        }
#endif

    }   
}
#endif
