using backEnd.Repositorios;

namespace backEnd.Controllers
{
    public class GeneroController
    {
        private readonly IRepositorio repositorio;

        public GeneroController(IRepositorio repositorio) { 
            this.repositorio = repositorio;
        }
    }
}
