using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace MOPCON_WP8Apps.Helpers
{
    /// <summary>
    /// Isolated storage file helper class
    /// </summary>
    /// <typeparam name="T">Data type to serialize/deserialize</typeparam>
    public class IsolatedStorageXML<T>
    {
        /// <summary>
        /// Lock object for thread pool save operations
        /// </summary>
        private object saveLock = new object();

        /// <summary>
        /// Begins a background thread save operation
        /// </summary>
        /// <param name="fileName">Name of the file to write to.</param>
        /// <param name="data">The data to store.</param>
        /// <param name="completed">Action delegate to cal on completion.</param>
        /// <param name="handleException">Exception handler.</param>
        public void BeginSave(
            string folderName,
            string fileName,
            T data,
            Action completed,
            Action<Exception> handleException)
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    lock (saveLock)
                    {
                        this.SaveToFile(folderName, fileName, data);
                    }

                    if (completed != null)
                    {
                        completed();
                    }
                }
                catch (Exception e)
                {
                    if (handleException != null)
                    {
                        handleException(e);
                    }
                }
            });
        }



        /// <summary>
        /// Loads data from a file
        /// </summary>
        /// <param name="fileName">Name of the file to read.</param>
        /// <returns>Data object</returns>
        public T LoadFromFile(string folderName, string fileName)
        {
            T loadedFile = default(T);
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(fileName))
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                        using (FileStream myFileStream = store.OpenFile(fileName, FileMode.Open))
                        {
                            // Call the Deserialize method and cast to the object type.
                            loadedFile = (T)mySerializer.Deserialize(myFileStream);
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

        /// <summary>
        /// Saves data to a file.
        /// </summary>
        /// <param name="fileName">Name of the file to write to</param>
        /// <param name="data">The data to save</param>
        public void SaveToFile(string folderName, string fileName, T data)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                    if (store.FileExists(fileName))
                    {
                        store.DeleteFile(fileName);
                    }

                    if (data != null)
                    {
                        using (StreamWriter myWriter =
                            new StreamWriter(store.OpenFile(fileName, FileMode.CreateNew)))
                        {
                            mySerializer.Serialize(myWriter, data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Add desired error handling for your application
                // ApplicationState.ErrorLog.Add(new ErrorLog("SaveToFile", e.Message));
            }
        }

    }
}
