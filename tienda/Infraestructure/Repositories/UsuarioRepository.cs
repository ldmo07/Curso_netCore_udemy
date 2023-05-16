using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(TiendaContext context) : base(context)
        {
        }

        public async Task<Usuario> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Usuarios.Include(u=>u.Roles)
                                          .Include(u=>u.RefreshToken)
                                          .FirstOrDefaultAsync(u => u.RefreshToken.Any(t=>t.Token==refreshToken));
        }

        public async Task<Usuario> GetByUsernameAsync(string username)
        {
            return await _context.Usuarios.Include(u=>u.Roles)
                                          .Include(u=>u.RefreshToken)
                                          .FirstOrDefaultAsync(u => u.Username.ToLower()==username.ToLower());
        }
    }
}