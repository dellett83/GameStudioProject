using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class IPgrabber : MonoBehaviour
{
    public TMP_Text hintText;

    private void Start()
    {
        GetLocalIPAddress();
    }
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                hintText.text = "Your IP: " + ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

}