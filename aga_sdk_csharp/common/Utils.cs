using aga_sdk_csharp.dto;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

/**
 * @author Alexander Llacho
 */
namespace aga_sdk_csharp.common
{
    public class Utils
    {
        private static Utils __instance = null;

        private Utils()
        {
        }

        public static Utils getInstance
        {
            get
            {
                if (__instance == null)
                {
                    __instance = new Utils();
                }

                return __instance;
            }
        }

        public string CreateTempDir()
        {
            string tempLocalPath = Path.GetTempPath();

            Guid pathGuid = Guid.NewGuid();
            string tempDir = string.Concat(tempLocalPath, @"temp_sign\", pathGuid, @"\");

            var directory = Directory.CreateDirectory(tempDir);

            return directory.FullName;
        }

        public string CreateZip(FileStream file, ConfigAga configAga, string pathDir)
        {
            string pathProperties = string.Concat(pathDir, Constants.PARAM_PROPERTIES);
            FileStream newFile = null;

            using (newFile = new FileStream(string.Concat(pathDir, Path.GetFileName(file.Name)), FileMode.Create))
            {
                file.CopyTo(newFile);
            }

            //create param - properties
            Dictionary<string, string> propertiesParam = new Dictionary<string, string>();
            propertiesParam.Add("contentFile", Path.GetFileName(file.Name));
            propertiesParam.Add("timestamp", configAga.timestamp);
            propertiesParam.Add("certificateId", configAga.certificateId);
            propertiesParam.Add("secretPassword", configAga.secretPassword);

            using (StreamWriter sw = new StreamWriter(pathProperties))
            {
                if (File.Exists(pathProperties))
                {
                    foreach (var entry in propertiesParam)
                    {
                        sw.WriteLine("{0}={1}", entry.Key, entry.Value);
                    }
                }
            }

            try
            {
                //zip files
                //SevenZipCompressor.SetLibraryPath(Path.GetFullPath("7z.dll"));
                SevenZipCompressor szc = new SevenZipCompressor();
                szc.CompressionLevel = CompressionLevel.Ultra;
                szc.CompressionMode = CompressionMode.Create;

                string sevenZOutput = string.Concat(pathDir, Constants.FILE_ZIP);
                string[] pathFiles = { newFile.Name, pathProperties };

                szc.CompressionMode = File.Exists(sevenZOutput) ? SevenZip.CompressionMode.Append : SevenZip.CompressionMode.Create;
                FileStream archive = new FileStream(sevenZOutput, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                szc.DirectoryStructure = true;
                szc.EncryptHeaders = true;
                szc.DefaultItemName = sevenZOutput;
                szc.CompressFiles(archive, pathFiles);
                archive.Close();

                return sevenZOutput;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

            return null;
        }
    }
}