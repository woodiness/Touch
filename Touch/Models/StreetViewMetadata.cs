using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Touch.Models
{
    internal class StreetViewMetadata
    {
        /// <summary>
        /// 判断某张图片的GPS点是否有街景，有的话返回“OK”
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static async Task<string> GetStreetViewStutas(string x, string y)
        {
            var http = new HttpClient();
            var response = await http.GetAsync("https://maps.googleapis.com/maps/api/streetview/metadata?location=" +
                                               x + "," + y + "&key=AIzaSyB2XzGNuHQLEd1AGjAnYkC6EoREiS_K09Q");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var Metadata = (RootObject) serializer.ReadObject(ms);
            return Metadata.status;
        }
    }

    [DataContract]
    public class Location
    {
        [DataMember]
        public double lat { get; set; }

        [DataMember]
        public double lng { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public string copyright { get; set; }

        [DataMember]
        public string date { get; set; }

        [DataMember]
        public Location location { get; set; }

        [DataMember]
        public string pano_id { get; set; }

        [DataMember]
        public string status { get; set; }
    }
}