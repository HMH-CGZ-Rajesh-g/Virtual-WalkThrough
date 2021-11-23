using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using cdb.virtualwalkthrough;
using Newtonsoft.Json;

public class Connection : MonoBehaviour
{

    public string url = "ws://virtualwalkthroughbackend.azurewebsites.net/";// ws://modernmixedrealityassist.azurewebsites.net/";
  WebSocket websocket;


  public GameObject playerobj;
  public Vector3 pos;
  public Vector3 rot;
  public Vector3 Childrot;
  string id;
  public bool admin = false;

  public string reqstring,clientMessage;

    public static Connection instance;
    private string previousmessage=string.Empty;
  // Start is called before the first frame update

        private void Awake()
    {
        VirtualwalkThrough.SendMessageToClient += VirtualwalkThrough_SendMessageToClient;
        VirtualwalkThrough.IDEnterd += VirtualwalkThrough_IDEnterd;

        if (instance == null)
            instance = this;
        GlobalConstants.isAdmin = admin;
    }

    private void OnDestroy()
    {
        VirtualwalkThrough.IDEnterd -= VirtualwalkThrough_IDEnterd;
        VirtualwalkThrough.SendMessageToClient -= VirtualwalkThrough_SendMessageToClient;


    }
    private void VirtualwalkThrough_IDEnterd(string obj)
    {
        id = obj;
        StartConnection();
    }

    private void VirtualwalkThrough_SendMessageToClient(string obj)
    {
        clientMessage = obj;
    }

    async void StartConnection()
  {
        Debug.Log("Trying Connection Open!");

        // websocket = new WebSocket("ws://echo.websocket.org");
        // websocket = new WebSocket("ws://localhost:8080");
        websocket = new WebSocket(url);
    websocket.OnOpen += () =>
    {
        Debug.Log("Connection Open!"+id);

        SocketMessage<string> msg = new SocketMessage<string>("session", id.ToString());
        websocket.SendText(JsonConvert.SerializeObject(msg));
    };

    websocket.OnError += (e) =>
    {
      Debug.Log("Error! " + e);
    };

    websocket.OnClose += (e) =>
    {
      Debug.Log("Connection closed!");
    };

    websocket.OnMessage += (bytes) =>
    {
        Debug.Log("Received OnMessage!");
        // Reading a plain text message
        if (admin == false)
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + message);
            SocketMessage<string> socket = JsonConvert.DeserializeObject<SocketMessage<string>>(message);
             
            string[] received = socket.data.Split('/');
            if (received.Length >= 5)
            {
                pos = StringToVector3(received[0]);
                rot = StringToVector3(received[1]);
                Childrot = StringToVector3(received[2]);
                if (id == received[3])
                {
                    playerobj.transform.localPosition = pos;
                    playerobj.transform.localEulerAngles = rot;
                    playerobj.transform.GetChild(0).transform.localEulerAngles = Childrot;
                }
                VirtualwalkThrough.FireAppAction(received[4]);
            }
        }
    };

    // Keep sending messages at every 0.3s
    //  InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);
   // InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);
   
      await websocket.Connect();
  }

 


  void Update()
  {
      
        if (admin && playerobj != null)
        {
            //playerobj.transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * 5 * Time.deltaTime);
            //playerobj.transform.Rotate(Input.GetAxis("Horizontal") * Vector3.up * 15 * Time.deltaTime);

            ConvertVectorToString(playerobj.transform.position, playerobj.transform.localEulerAngles, playerobj.transform.GetChild(0).transform.localEulerAngles, id, clientMessage);
            clientMessage = "";
            // SendWebSocketMessage();

#if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
#endif
        }
    
  }


  void ConvertVectorToString(Vector3 pos1, Vector3 rot1, Vector3 rot2,string clientID, string message)
  {
    if (admin)
    {
      reqstring = (pos1 + "/" + rot1+"/"+rot2+"/"+clientID+"/"+message).ToString();
            if (previousmessage != reqstring)
            {
                SendWebSocketMessage();
                previousmessage = reqstring;
            }
    }
   
    
  }

  public static Vector3 StringToVector3(string sVector)
  {
    // Remove the parentheses
    if (sVector.StartsWith("(") && sVector.EndsWith(")"))
    {
      sVector = sVector.Substring(1, sVector.Length - 2);
    }

    // split the items
    string[] sArray = sVector.Split(',');

    // store as a Vector3
    Vector3 result = new Vector3(
        float.Parse(sArray[0]),
        float.Parse(sArray[1]),
        float.Parse(sArray[2]));

    return result;
  }
  async void SendWebSocketMessage()
  {
    if (websocket.State == WebSocketState.Open)
    {
            // Sending bytes
            // await websocket.Send(new byte[] { 10, 20, 30 });
//            Debug.Log("Sending values" + reqstring);
            // Sending plain text
            SocketMessage<string> msg = new SocketMessage<string>("dummy", reqstring);
            await websocket.SendText(JsonConvert.SerializeObject(msg));
            //await websocket.SendText(reqstring);
    }
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }
}
