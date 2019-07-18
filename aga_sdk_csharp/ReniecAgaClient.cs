using aga_sdk_csharp.dto;
using aga_sdk_csharp.service;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace aga_sdk_csharp
{
    public class ReniecAgaClient
    {
        private ConfigAga configAga = null;
        private SignService signService;

        public ReniecAgaClient(ConfigAga oConfigAga, String path7Zdll)
        {
            this.SetConfig(oConfigAga, path7Zdll);
        }

        public async Task<byte[]> SignAga(FileStream file)
        {
            if (this.configAga == null)
            {
                return null;
            }

            return await this.signService.ProcSignAga(file);
        }

        private void SetConfig(ConfigAga oConfigAga, String path7Zdll)
        {
            try
            {
                this.configAga = oConfigAga;
                this.signService = SignService.getInstance(this.configAga, path7Zdll);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
