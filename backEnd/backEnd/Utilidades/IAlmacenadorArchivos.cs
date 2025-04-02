namespace backEnd.Utilidades
{
    public interface IAlmacenadorArchivos
    {
        Task BorrarArchivos(string ruta, string contenedor);
        Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta);
        Task<string> GuardarArchivo(string contenedor, IFormFile archivo);
    }
}
