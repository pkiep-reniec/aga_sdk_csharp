using aga_sdk_csharp;
using aga_sdk_csharp.dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace aga_sdk_test
{
    [TestClass]
    public class SignTest
    {
        private string tempDir = string.Concat(Path.GetTempPath(), @"temp_sign");
        private ReniecAgaClient reniecAgaClient;

        [TestMethod]
        public void SignFile()
        {
            Before();

            try
            {
                string pathFile = @"..\..\resources\signTest.json";

                Task<byte[]> result = reniecAgaClient.SignAga(new FileStream(pathFile, FileMode.Open));
                result.Wait();

                if (result.Result != null)
                {
                    using (var output = new FileStream(string.Concat(tempDir, "fileSign.p7s"), FileMode.Create))
                    {
                        Stream stream = new MemoryStream(result.Result);
                        stream.CopyTo(output);
                    }
                }

                Assert.IsNotNull(result.Result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void Before()
        {
            ConfigAga oConfigAga = new ConfigAga();
            oConfigAga.agaUri = "http://127.0.0.1:8080/refirma-aga/rest/service/sign-file";
            oConfigAga.timestamp = "true";
            oConfigAga.certificateId = "certdm";
            oConfigAga.secretPassword = "NH7PERU$$$";

            reniecAgaClient = new ReniecAgaClient(oConfigAga);
        }
    }
}
