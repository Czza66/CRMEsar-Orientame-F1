using CRMEsar.AccesoDatos.Data.Repository.IRepository;

namespace CRMEsar.Services.Prestadores
{
    public class PrestadorCodigoService : IPrestadorCodigoService
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly Random _random;

        public PrestadorCodigoService(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _random = new Random();
        }

        public int ObtenerCodigoDisponible()
        {
            int rangoMin = 1000;
            int rangoMax = 9999;

            var codigosUsados = _contenedorTrabajo.Prestadores
            .GetAll()
            .Select(p => p.Codigo)
            .Where(c => c > 0)
            .ToHashSet();

            var codigosDisponibles = Enumerable
            .Range(rangoMin, rangoMax - rangoMin + 1)
            .Where(c => !codigosUsados.Contains(c))
            .ToList();

            if (!codigosDisponibles.Any())
            {
                throw new InvalidOperationException(
                    "No hay códigos disponibles para asignar a prestadores."
                );
            }

            return codigosDisponibles[_random.Next(codigosDisponibles.Count)];
        }

    }
}
