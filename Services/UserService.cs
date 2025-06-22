using auto_front.Models;
using auto_front.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using auto_front.Lib;

namespace auto_front.Services
{
    public class UserService(BusContext context)
    {
        private readonly BusContext _context = context;

        public async Task<IResponse> CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.PasswordHash))
                return IResponse.Error("Login e senha não podem ser vazios.");
            // Hash the password before saving
            user.PasswordHash = Lib.Crypto.GenerateHash(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new IResponse(true, user, "Usuário criado com sucesso.");
        }

        public async Task<IResponse> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
                return IResponse.Error("ID inválido.");
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return IResponse.Error("Usuário não encontrado.");
            return IResponse.Ok(user);
        }

        public async Task<IResponse> GetUsers()
        {
            List<User> users = await _context.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    Login = u.Login
                })
                .ToListAsync();
            if (users.Count == 0 || users == null)
                return IResponse.Error("Nenhum usuário encontrado.");
            return IResponse.Ok(users);
        }

        public async Task<IResponse> Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return IResponse.Error("Login e senha não podem ser vazios.");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null)
                return IResponse.Error("Usuário não encontrado");

            bool isLogged = Lib.Crypto.Compare(password, user.PasswordHash);

            if (isLogged)
                return IResponse.Ok(null, "Login bem-sucedido.");
            else
                return IResponse.Error("Senha incorreta.");

        }
    }
}