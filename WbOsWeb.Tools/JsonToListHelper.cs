using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbOsWeb.Entity;
using Newtonsoft.Json;

namespace WbOsWeb.Tools
{
    public class JsonToListHelper
    {
        public Wuliu JsonToList(string jsonstr)
        {
            byte[] array = Encoding.UTF8.GetBytes(jsonstr);
            MemoryStream stream = new MemoryStream(array);             //convert stream 2 string      
            StreamReader streamReader = new StreamReader(stream);
            JsonSerializer serializer = new JsonSerializer();
            Wuliu p1 = (Wuliu)serializer.Deserialize(new JsonTextReader(streamReader), typeof(Wuliu));
            streamReader.Dispose();
            List<Traces> calllogs = p1.Traces.ToList<Traces>(); //数组转成List
            return p1;
        }
    }
}
