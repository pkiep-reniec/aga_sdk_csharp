using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using aga_sdk_csharp.dto;
using aga_sdk_csharp.service;

namespace aga_sdk_csharp
{
    public class ReniecAgaClient
    {
        private ConfigAga configAga = null;
        private SignService signService;

        public ReniecAgaClient(ConfigAga oConfigAga)
        {
            this.SetConfig(oConfigAga);
        }

        public async Task<byte[]> SignAga(FileStream file)
        {
            if (this.configAga == null)
            {
                return null;
            }

            return await this.signService.ProcSignAga(file);
        }

        private void SetConfig(ConfigAga oConfigAga)
        {
            try
            {
                this.configAga = oConfigAga;
                this.signService = SignService.getInstance(this.configAga);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
