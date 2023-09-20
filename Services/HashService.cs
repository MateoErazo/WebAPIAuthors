using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebAPIAuthors.DTOs.Hash;

namespace WebAPIAuthors.Services
{
  public class HashService
  {
    public HashResultDTO Hash(string text)
    {
      byte[] salt = new byte[16];

      using (var random = RandomNumberGenerator.Create())
      {
        random.GetBytes(salt);
      }

      return Hash(text, salt);
    }

    public HashResultDTO Hash(string text, byte[] salt)
    {
      var keyDerivation = KeyDerivation.Pbkdf2(
        password:text,salt:salt,prf: KeyDerivationPrf.HMACSHA1,iterationCount:10000,numBytesRequested:32);

      return new HashResultDTO
      {
        Hash = Convert.ToBase64String(keyDerivation),
        Salt = salt
      };
    }
  }
}
