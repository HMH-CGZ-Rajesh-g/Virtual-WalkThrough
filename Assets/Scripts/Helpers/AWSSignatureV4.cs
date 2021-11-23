using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AWSSignatureV4
{
    private readonly SHA256 _sha256;
    private const string ALGORITHM = "AWS4-HMAC-SHA256";
    private const string EMPTY_STRING_HASH = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

    public AWSSignatureV4()
    {
        _sha256 = SHA256.Create();
    }

    public string CreateSignature(string host, string pathName, DateTimeOffset datestamp, string sessionKey, string secretKey, string access)
    {
        string longDate = datestamp.ToString("yyyyMMddTHHmmssZ");
        string shortDate = datestamp.ToString("yyyyMMdd");
        string cR = CanonicalRequest(host,pathName, longDate, shortDate, sessionKey);
        string toSign = StringToSign(cR, longDate, shortDate);
        string signature = Signature(secretKey, shortDate, "us-east-1", "s3", toSign);
        return "AWS4-HMAC-SHA256 Credential=" + access + "/" + shortDate + "/us-east-1/s3/aws4_request,SignedHeaders=host;x-amz-content-sha256;x-amz-date;x-amz-security-token,Signature=" + signature;
    }

    private static byte[] HmacSHA256(byte[] key, string data)
    {
        var hashAlgorithm = new HMACSHA256(key);

        return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    private static byte[] getSignatureKey(String key, String dateStamp, String regionName, String serviceName)
    {
        byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
        byte[] kDate = HmacSHA256(kSecret, dateStamp);
        byte[] kRegion = HmacSHA256(kDate, regionName );
        byte[] kService = HmacSHA256( kRegion, serviceName);
        byte[] kSigning = HmacSHA256(kService,"aws4_request");

        return kSigning;
    }

    private string Hash(byte[] bytesToHash)
    {
        var result = _sha256.ComputeHash(bytesToHash);
        return ToHexString(result);
    }

    static string ToHexString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    private string CanonicalRequest(string host, string pathName, string longDate, string shortDate, string sessionKey) //"\nx-amz-security-token:" + sessionKey + //;x-amz-security-token
    {
        return  ("GET" + "\n" + pathName + "\n" + "" + "\nhost:" + host + "\n" + ("x-amz-content-sha256:" + EMPTY_STRING_HASH) + "\n" + ("x-amz-date:" + longDate + "\nx-amz-security-token:" + sessionKey + "\n\n") + ("host;x-amz-content-sha256;x-amz-date;x-amz-security-token\n") + EMPTY_STRING_HASH);
    }

    public string StringToSign(string canonicalRequest, string longDate, string shortDate)
    {
        return ("AWS4-HMAC-SHA256\n" + longDate + "\n" + shortDate + "/" + "us-east-1" + "/s3/aws4_request\n" + Hash(Encoding.UTF8.GetBytes(canonicalRequest)));
    }

    private string Signature(string secretKey, string dateStamp, string region, string service, string stringToSign)
    {
        var signingKey = getSignatureKey(secretKey, dateStamp, region, service);
        return ToHexString(HmacSHA256(signingKey,stringToSign));
    }
}
