using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using CRMEsar.Models;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    public class SliderPresentacionesRepository : Repository<SliderPresentaciones>,ISliderPresentacionesRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderPresentacionesRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void update(SliderPresentaciones sliderPresentaciones)
        {
            var objDesdeDB = _db.SliderPresentaciones.FirstOrDefault(e => e.SliderId == sliderPresentaciones.SliderId);
            if (objDesdeDB != null) 
            {
                objDesdeDB.Titulo = sliderPresentaciones.Titulo;
                objDesdeDB.Descripcion = sliderPresentaciones.Descripcion;
                objDesdeDB.ImagenRuta = sliderPresentaciones.ImagenRuta;
                objDesdeDB.TextoAlternativo = sliderPresentaciones.TextoAlternativo;
                objDesdeDB.UrlDestino = sliderPresentaciones.UrlDestino;
                objDesdeDB.Orden = sliderPresentaciones.Orden;
                objDesdeDB.Activo = sliderPresentaciones.Activo;
            }
        }

    }
}
