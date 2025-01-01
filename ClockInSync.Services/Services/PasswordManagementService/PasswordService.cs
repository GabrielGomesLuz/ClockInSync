using Microsoft.AspNetCore.Identity;

namespace ClockInSync.Services.PasswordManagementService
{

    public interface IPasswordService
    {
        public string HashPassword(string password, object userContext);
        public bool VerifyPassword(string hashedPassword, string passwordToVerify, object userContext);


    }

    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<object>();  // O tipo de T não importa, então passamos "object"
        }

        // Gerar o hash da senha
        public string HashPassword(string password, object userContext)
        {
            return _passwordHasher.HashPassword(userContext, password);
        }

        // Verificar se a senha fornecida corresponde ao hash armazenado
        public bool VerifyPassword(string hashedPassword, string passwordToVerify, object userContext)
        {
            var result = _passwordHasher.VerifyHashedPassword(userContext, hashedPassword, passwordToVerify);
            return result == PasswordVerificationResult.Success;
        }
    }
}
