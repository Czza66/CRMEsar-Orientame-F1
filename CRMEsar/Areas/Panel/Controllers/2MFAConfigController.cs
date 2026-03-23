using CRMEsar.AccesoDatos.Data.Repository;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades._2MFAConfig;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class _2MFAConfigController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;

        public _2MFAConfigController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var value = _contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "int").SettingId.ToString();
            var periodo = _contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "string").SettingId.ToString();
                
            if (periodo == null && value == null) 
            {
                NotFound();
            }

            var vm = new _2MFAVM
            {
                Group = "2MFA",
                DescriptionValue = _contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "int").Description,

                DescriptionString = _contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "string").Description,

                ValueInt = _contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "int").ValueInt.Value,

                ValueString = _contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "string").ValueString,

                IdEncriptadoValue = _protectorUtils.EncriptarGuid(_contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "int").SettingId,"Value2MFA"),

                IdEncriptadoString = _protectorUtils.EncriptarGuid(_contenedorTrabajo.SystemSetting
                .GetFirstOrDefault(s => s.Group == "2MFA" && s.DataType == "string").SettingId,"String2MFA")

            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(_2MFAVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            Guid idRealValue = _protectorUtils.DesencriptarGuid(vm.IdEncriptadoValue, "Value2MFA");
            Guid idRealString = _protectorUtils.DesencriptarGuid(vm.IdEncriptadoString, "String2MFA");

            var settingValue = _contenedorTrabajo.SystemSetting.GetFirstOrDefault(x => x.SettingId == idRealValue);
            var settingUnit = _contenedorTrabajo.SystemSetting.GetFirstOrDefault(x => x.SettingId == idRealString);

            if (settingValue == null || settingUnit == null)
                return NotFound();

            // Actualizas SOLO lo editable
            settingValue.ValueInt = vm.ValueInt;
            settingUnit.ValueString = vm.ValueString;

            // (opcional) refrescar auditoría
            settingValue.UpdatedAtUtc = DateTime.UtcNow;
            settingUnit.UpdatedAtUtc = DateTime.UtcNow;

            _contenedorTrabajo.Save();

            TempData["RespuestaOperacion"] = "2MFA actualizado correctamente";
            return RedirectToAction(nameof(Edit)); // mejor que return View(vm)
        }
    }
}
