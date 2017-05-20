using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ImageSender.Controllers
{
    public class IndexController : ApiController
    {
        public readonly string pathReceive = @"C:\Users\Syune\Desktop\images\Receive\";


        /// <summary>
        /// get image by  its name and return HttpResponseMessage
        /// </summary>
        /// <param name="name">name of image</param>
        /// <returns></returns>
        [Route("api/index/getimage")]
        [HttpGet]
        public HttpResponseMessage GetImage(string name)
        {

            using (FileStream stream = new FileStream(pathReceive + name, FileMode.Open, FileAccess.Read))
            {
                var ms = new MemoryStream();
                stream.CopyTo(ms);//copy stream to memory stream to use methods of  class MemoryStream
                ms.Seek(0, SeekOrigin.Begin);
                ms.Position = 0;
                // processing the stream.

                var result = new HttpResponseMessage(HttpStatusCode.OK) {  Content = new ByteArrayContent(ms.ToArray()) };
                result.Content.Headers.ContentDisposition =  new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                   FileName = name
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return result;
               
            }
        }

        /// <summary>
        /// POST method which get request bytes and write them in new file , name of file is the name of image
        /// </summary>
        /// <param name="name">image name</param>
        /// <returns></returns>
        [Route("api/index/addimage")]
        [HttpPost]
        public bool AddImage(string name) 
        {
            Task<byte[]> task = this.Request.Content.ReadAsByteArrayAsync();
            task.Wait();
            byte[] bytes = task.Result;
            File.WriteAllBytes(pathReceive + name, bytes);
            return true;
        }
    }
}
