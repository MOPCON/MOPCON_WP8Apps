using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MOPCON_WP8Apps.Helpers
{
    /// <summary>
    /// Isolated storage file helper class
    /// </summary>
    /// <typeparam name="T">Data type to serialize/deserialize</typeparam>
    public class IsolatedStorageJSON<T>
    {
        /// <summary>
        /// Lock object for thread pool save operations
        /// </summary>
        private object saveLock = new object();

        /// <summary>
        /// Loads data from a file
        /// </summary>
        /// <param name="fileName">Name of the file to read.</param>
        /// <returns>Data object</returns>
        public async static Task<T> LoadFromFileAsync(string folderName, string fileName)
        {
            var loadedFile = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            //T loadedFile = default(T);
            string tempStr = "";
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string fullFilename = "";
                    if (myIsolatedStorage.DirectoryExists(folderName) == false)
                    {
                        myIsolatedStorage.CreateDirectory(folderName);
                    }
                    fullFilename = string.Format("{0}/{1}", folderName, fileName);

                    if (myIsolatedStorage.FileExists(fullFilename))
                    {
                        // 開啟串流，準備讀取該檔案
                        using (IsolatedStorageFileStream isoFileStream = myIsolatedStorage.OpenFile(fullFilename, FileMode.Open))
                        {
                            using (StreamReader myStreamReader = new StreamReader(isoFileStream))
                            {
                                tempStr = await myStreamReader.ReadToEndAsync();

                                loadedFile = JsonConvert.DeserializeObject<T>(tempStr);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //ApplicationState.ErrorLog.Add(new ErrorLog("LoadFromFile", e.Message));
            }

            return loadedFile;
        }

        public static T LoadFromString(string SourceString)
        {
            T loadedFile = default(T);
            try
            {
                loadedFile = JsonConvert.DeserializeObject<T>(SourceString);
            }
            catch (Exception e)
            {
                //ApplicationState.ErrorLog.Add(new ErrorLog("LoadFromFile", e.Message));
            }

            return loadedFile;
        }

        /// <summary>
        /// Saves data to a file.
        /// </summary>
        /// <param name="fileName">Name of the file to write to</param>
        /// <param name="data">The data to save</param>
        public async static Task SaveToFileAsync(string folderName, string fileName, T data)
        {
            try
            {
                string output = JsonConvert.SerializeObject(data);

                #region Isolated Storage Copy Code
                using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string fullFilename = "";
                    if (myIsolatedStorage.DirectoryExists(folderName) == false)
                    {
                        myIsolatedStorage.CreateDirectory(folderName);
                    }
                    fullFilename = string.Format("{0}/{1}", folderName, fileName);

                    using (var writeStream = new IsolatedStorageFileStream(fullFilename, FileMode.Create, myIsolatedStorage))
                    using (var writer = new StreamWriter(writeStream))
                    {
                        await writer.WriteAsync(output);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                // Add desired error handling for your application
                // ApplicationState.ErrorLog.Add(new ErrorLog("SaveToFile", e.Message));
            }
            return;
        }

    }
}
