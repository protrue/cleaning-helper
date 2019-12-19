using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CleaningHelper.Model;

namespace CleaningHelper.Tools
{
    public static class FrameModelSerializer
    {
        public static void Serialize(string path, FrameModel frameModel)
        {
            var binaryFormatter = new BinaryFormatter();

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                binaryFormatter.Serialize(fileStream, frameModel);
            }
        }

        public static FrameModel Deserialize(string path)
        {
            var binaryFormatter = new BinaryFormatter();

            FrameModel frameModel;

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                frameModel = binaryFormatter.Deserialize(fileStream) as FrameModel;
            }

            return frameModel;
        }
    }
}
