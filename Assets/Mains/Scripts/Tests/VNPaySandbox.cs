using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using YNL.Utilities.Extensions;

public class VNPayTest : MonoBehaviour
{
    public string vnp_TmnCode = "RA025UR5";
    public string vnp_HashSecret = "UB00V0N9VSBWG4RYOZSAMIFXJ0D41QQD";
    public string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
    public string returnUrl = "http://your-return-url.com";

    [Button]
    public void StartPayment()
    {
        string vnp_TxnRef = DateTime.Now.Ticks.ToString();
        string vnp_OrderInfo = "HelloWorld";
        string vnp_Amount = (123123 * 100).ToString(); // 100000 VND

        SortedList<string, string> requestData = new SortedList<string, string>();
        requestData.Add("vnp_Version", "2.1.0");
        requestData.Add("vnp_Command", "pay");
        requestData.Add("vnp_TmnCode", vnp_TmnCode);
        requestData.Add("vnp_Amount", vnp_Amount);
        requestData.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        requestData.Add("vnp_CurrCode", "VND");
        requestData.Add("vnp_IpAddr", "127.0.0.1");
        requestData.Add("vnp_Locale", "vn");
        requestData.Add("vnp_OrderInfo", vnp_OrderInfo);
        requestData.Add("vnp_OrderType", "other");
        requestData.Add("vnp_ReturnUrl", returnUrl);
        requestData.Add("vnp_TxnRef", vnp_TxnRef);

        string queryString = string.Join("&", requestData.Select(kv =>
            $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"
        ));

        string signData = string.Join("&", requestData.Select(kv =>
            $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"
        ));

        string secureHash = HmacSHA512(signData, vnp_HashSecret);

        string finalUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={secureHash}";
        Application.OpenURL(finalUrl); // Mở trình duyệt Unity tới trang thanh toán
        MDebug.Log(signData);
        MDebug.Log(finalUrl);
    }

    public static string HmacSHA512(string message, string secretKey)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hashValue).Replace("-", "").ToUpper();
        }
    }
}