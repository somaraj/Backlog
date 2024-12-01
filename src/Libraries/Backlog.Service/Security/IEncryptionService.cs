﻿namespace Backlog.Service.Security
{
    public interface IEncryptionService
    {
        string CreateSaltKey(int size);

        string CreatePasswordHash(string password, string saltKey);

        string EncryptText(string plainText, string encryptionPrivateKey = "");

        string DecryptText(string cipherText, string encryptionPrivateKey = "");

        string GenerateToken(Guid employeeCode);

        bool ValidateToken(string token, int expiryTimeInMinutes = 180);
    }
}