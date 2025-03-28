using backEnd.Entidades;

namespace backEnd.Repositorios
{
    public interface IRepositorio
    {
        void CrearGenero(Genero genero);
        Guid ObtenerGuid();
        Task<Genero> ObtenerPorId(int id);
        List<Genero> ObtenerTodosLosGeneros();
    }
}
