using aga_sdk_csharp.common;
using aga_sdk_csharp.dto;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace aga_sdk_csharp.service
{
    class SignService
    {
        private static SignService __instance = null;
     
        private ConfigAga configAga;
        private Utils utils;

        private SignService(ConfigAga configAga)
        {
            this.configAga = configAga;
            this.utils = Utils.getInstance;
        }

        public static SignService getInstance(ConfigAga configAga)
        {
            if (__instance == null)
            {
                __instance = new SignService(configAga);
            }

            return __instance;
        }

        public async Task<byte[]> ProcSignAga(FileStream file)
        {
            if (this.configAga == null)
            {
                return null;
            }

            try
            {
                string pathDir = utils.CreateTempDir();
     

                FileStream fileSign = await SignMetadata(file, pathDir);

                if (fileSign != null)
                {
                    //Directory.Delete(pathDir, true);

                    return File.ReadAllBytes(fileSign.Name);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

            return null;
        }

        private async Task<FileStream> SignMetadata(FileStream file, string pathDir)
        {
            FileStream output = null;

            try
            {
                ServicePointManager.Expect100Continue = false;

                string pathFileZip = utils.CreateZip(file, this.configAga, pathDir);

                FileStream fileZip = new FileStream(pathFileZip, FileMode.Open, FileAccess.Read);

                //sign file zip
                using (var formContent = new MultipartFormDataContent())
                {
                    formContent.Add(new StreamContent(fileZip), "uploadFile", fileZip.Name);

                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(configAga.agaUri, formContent);

                        if (response.IsSuccessStatusCode)
                        {
                            client.Dispose();
                            Stream fileSign = response.Content.ReadAsStreamAsync().Result;

                            using (output = new FileStream(string.Concat(pathDir, Constants.FILE_SIGN), FileMode.Create))
                            {
                                fileSign.CopyTo(output);
                            }

                            return output;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

            return output;
        }
    }
}
