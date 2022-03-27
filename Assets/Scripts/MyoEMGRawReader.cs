using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MyoEMGRawReader : MonoBehaviour
{
    public string IP = "127.0.0.1";
    public int port = 12345;

    // read Thread
    Thread readThread;
    // udpclient object
    UdpClient client;
    String control = "";
    

    public void StartReadingData()
    {
        // create thread for reading UDP messages
        readThread = new Thread(new ThreadStart(ReceiveData));
        readThread.IsBackground = true;
        readThread.Start();
    }

    // Unity Application Quit Function
    void OnApplicationQuit()
    {
        stopThread();
    }

    // Stop reading UDP messages
    public void stopThread()
    {
        if (readThread.IsAlive)
        {
            readThread.Abort();
        }
        client.Close();
    }

    // receive thread function
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                // receive bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] buff = client.Receive(ref anyIP);

                // encode UTF8-coded bytes to text format
                string text = Encoding.UTF8.GetString(buff);
                string[] parts = text.Split(' ');
                control = parts[0];
            }
            catch (Exception err)
            {
                // print(err.ToString());
            }
        }
    }

    public string ReadControlFromArmband() 
    {
        return control;
    }
}