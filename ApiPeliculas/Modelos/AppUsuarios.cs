using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Modelos
{
    public class AppUsuarios : IdentityUser
    {
        public string Nombre { get; set; }
    }
}
