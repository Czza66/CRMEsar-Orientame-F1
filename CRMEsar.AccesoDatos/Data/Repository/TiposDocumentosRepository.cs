using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    public class TiposDocumentosRepository :Repository<TiposDocumentos>, ITiposDocumentos
    {
        private readonly ApplicationDbContext _db;

        public TiposDocumentosRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public IEnumerable<SelectListItem> GetListaTiposDocumentos()
        {
            return _db.TipoDocumentos
                .Where(T => T.Estado.NormalizedName == "ACTIVO")
                .Select(T => new SelectListItem
                {
                    Text = T.TipoDocumento,
                    Value = T.TiposDocumentosId.ToString()
                });
        }

        public void update(TiposDocumentos tiposDocumentos)
        {
            var objDesdeDB = _db.TipoDocumentos.FirstOrDefault(t => t.TiposDocumentosId == tiposDocumentos.TiposDocumentosId);

            if (objDesdeDB != null)
            {
                objDesdeDB.TipoDocumento = tiposDocumentos.TipoDocumento;
                objDesdeDB.Abreviatura = tiposDocumentos.Abreviatura;
                objDesdeDB.PaisId = tiposDocumentos.PaisId;
                objDesdeDB.EstadoId = tiposDocumentos.EstadoId;

            }
        }
    }
}
