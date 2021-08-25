using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;
namespace DecisionTreeTest
{
    class FileIO
    {
        public static bool WriteToFile<T>(T obj, string fileName, string dir = "")
        {
            dir = string.IsNullOrEmpty(dir) ? AppDomain.CurrentDomain.BaseDirectory : dir;
            string filePath = Path.Combine(dir, fileName);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(stream, obj);
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }           
        }

        public static bool WriteToFile<T>(T obj, string filePath)
        {
            //dir = string.IsNullOrEmpty(dir) ? AppDomain.CurrentDomain.BaseDirectory : dir;
            if(string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }
            try
            {
                FileMode mode = (File.Exists(filePath)) ? FileMode.Truncate : FileMode.CreateNew;
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var stream = File.Open(filePath, mode))
                {
                    serializer.Serialize(stream, obj);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool WritePngToFile(PngBitmapEncoder encoder,string path, string dir)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            try
            {
                string filePath = Path.Combine(dir, path) + ".png";
                using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    encoder.Save(fs);
                }
                return true;
            }
            catch(Exception ex)
            {               
                return false;
            }
        }

        public static T ReadFromFile<T>(string fileName, string dir = "")
        {
            T ret = default(T);
            dir = string.IsNullOrEmpty(dir) ? AppDomain.CurrentDomain.BaseDirectory : dir;
            string filePath = Path.Combine(dir, fileName);
            try
            {
                XmlSerializer deSerializer = new XmlSerializer(typeof(T));
                using (var stream = File.Open(filePath, FileMode.Open))
                {
                    ret  = (T)deSerializer.Deserialize(stream);
                }
                return ret;
            }
            catch(Exception ex)
            {
                return ret;
            }

        }
        public static T ReadFromFile<T>(string filePath)
        {
            T ret = default(T);
            // dir = string.IsNullOrEmpty(dir) ? AppDomain.CurrentDomain.BaseDirectory : dir;
            // string filePath = Path.Combine(dir, fileName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return default(T);
            }
            try
            {
                XmlSerializer deSerializer = new XmlSerializer(typeof(T));
                using (var stream = File.Open(filePath, FileMode.Open))
                {
                    ret = (T)deSerializer.Deserialize(stream);
                }
                return ret;
            }
            catch (Exception)
            {
                return ret;
            }

        }
    }
}
