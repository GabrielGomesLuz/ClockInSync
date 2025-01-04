using ClockInSync.Repositories.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClockInSync.Services.PasswordManagementService
{


    public static class PasswordHashHelper
    {
        private static readonly PasswordHasher<object> _passwordHasher =  new();


        // Gerar o hash da senha
        public static string HashPassword(string password, User userContext)
        {
            return _passwordHasher.HashPassword(userContext, password);
        }

        // Verificar se a senha fornecida corresponde ao hash armazenado
        public static bool VerifyPassword(string hashedPassword, string passwordToVerify, User userContext)
        {
            var result = _passwordHasher.VerifyHashedPassword(userContext, hashedPassword, passwordToVerify);
            return result == PasswordVerificationResult.Success;
        }
    }
}
