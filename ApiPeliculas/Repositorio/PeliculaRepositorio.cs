using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PeliculaRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;

            //Arreglar Problema del PUT
            var peliculaExistente = _db.Peliculas.Find(pelicula.Id);
            if (peliculaExistente != null)
            {
                _db.Entry(peliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else
            {
                _db.Peliculas.Update(pelicula);
            }
            
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _db.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _db.Peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _db.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(int id)
        {
            return _db.Peliculas.Any(p => p.Id == id);
        }

        public bool ExistePelicula(string Nombre)
        {
            bool valor = _db.Peliculas.Any(p => p.Nombre.ToLower().Trim() == Nombre.ToLower().Trim());
            return valor;
        }

        public Pelicula GetPelicula(int PeliculaId)
        {
            return _db.Peliculas.FirstOrDefault(p => p.Id == PeliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _db.Peliculas.OrderBy(p => p.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int catId)
        {
            return _db.Peliculas.Include( ca => ca.Categoria).Where(ca => ca.categoriaId == catId).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
