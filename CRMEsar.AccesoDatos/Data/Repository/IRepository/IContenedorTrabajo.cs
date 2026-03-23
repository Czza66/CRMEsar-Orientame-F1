using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        //Aca se deben ir agregando los diferentes repositorios
        IEstadoRepository Estado { get; }
        IEntidadRepository Entidad { get; }
        IModuloRepository Modulo { get; }
        IModulosSeccionesRepository ModulosSecciones { get; }
        IPermisosModulosSeccionesRepository PermisosModulosSecciones { get; }
        ILogRepository Log { get; }
        ITipoNotificacionesRepository TipoNotificaciones { get; }
        INotificacionesRepository Notificaciones { get; }
        IIntegracionesRepository Integraciones { get; }
        ISliderPresentacionesRepository SliderPresentaciones { get; }

        IPaisesRepository Paises { get; }
        ITiposDocumentos TiposDocumentos { get; }
        ICargosRepository Cargos { get; }
        IPrestadoresRepository Prestadores { get; }
        IAtencionesRdaRepository AtencionesRda { get; }
        ISystemSettingRepository SystemSetting { get; }

        void Save();
    }
}
