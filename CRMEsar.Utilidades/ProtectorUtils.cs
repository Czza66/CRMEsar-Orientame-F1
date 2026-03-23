using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace CRMEsar.Utilidades
{
    public class ProtectorUtils
    {
        private readonly IDataProtectionProvider _dataProtection;

        public ProtectorUtils(IDataProtectionProvider dataProtection)
        {
            _dataProtection = dataProtection;
        }

        public string EncriptarGuid(Guid id, string purpose) 
        {
            var protector = _dataProtection.CreateProtector(purpose);
            return protector.Protect(id.ToString());
        }

        public Guid DesencriptarGuid(string valorEncriptado, string purpose) 
        {
            try
            {
                var protector = _dataProtection.CreateProtector(purpose);
                var desencriptado = protector.Unprotect(valorEncriptado);
                return Guid.Parse(desencriptado);
            }
            catch
            {
                throw new Exception("ID inválido o manipulado.");
            }
        }
    }
}
