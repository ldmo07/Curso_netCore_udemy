using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.Now >= Expires;
        public DateTime Created {get;set;}
        public DateTime? Revoked {get;set;} 
        public bool IsActive => Revoked == null && !IsExpired;       
    }
}