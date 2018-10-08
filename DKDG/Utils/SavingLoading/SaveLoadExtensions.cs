using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

using DKDG.Models;

using Microsoft.Win32;

namespace DKDG
{
    public static class SaveLoadExtensions
    {
        #region Methods

        public static void Deserialize(this object obj, IDictionary<string, object> propsDict)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
                propInfo.SetValue(obj, propsDict[propInfo.Name]);
        }

        public static T Open<T>(string ext) where T : ISavable
        {
            var openDialog = new OpenFileDialog();
            openDialog.Multiselect = true;
            openDialog.Title = "Open";
            openDialog.Filter = String.Format("Save File (*.{0}) | *{0}", ext);
            openDialog.ShowDialog();

            var ser = new DataContractSerializer(typeof(T));
            var reader = new FileStream(openDialog.FileName, FileMode.Open);
            reader.Close();
            return (T)ser.ReadObject(reader);
        }

        public static IEnumerable<string> PropNames(this object obj)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
                yield return propInfo.Name;
        }

        public static void Save<T>(this T obj) where T : ISavable
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save";
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".xml";
            saveDialog.FilterIndex = 0;
            saveDialog.Filter = String.Format("Save File (*.{0}) | *{0}", obj.Extension);
            saveDialog.ShowDialog();

            var ser = new DataContractSerializer(obj.GetType());
            var writer = new FileStream(saveDialog.FileName, FileMode.OpenOrCreate);
            ser.WriteObject(writer, obj);
            writer.Close();
        }

        #endregion Methods
    }
}
