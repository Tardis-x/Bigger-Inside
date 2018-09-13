using System;
using System.Security.Cryptography;
using System.Text;

namespace ua.org.gdg.devfest
{
  public static class Cyphering
  {
    public static string GetHashByKey(string key, string text)
    {
      var cypher = new HMACSHA256(Encoding.ASCII.GetBytes(key));
      var hash = cypher.ComputeHash(Convert.FromBase64String(text));
      var token = new StringBuilder();

      foreach (var t in hash)
      {
        token.Append(t.ToString("x2"));
      }
      
      return token.ToString();
    }
  }
}