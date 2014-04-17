using Snapshot;
using SnapshotService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SnapshotService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult RenderSnapshot(string URL, string imageType, string imageWidth, string imageHeight)
        {
            URLToImage urlToIMage = new URLToImage();
            string base64Image = urlToIMage.GetSnapshot(URL, imageType, imageWidth, imageHeight);
            string imageString = "data:image/" + imageType + ";base64," + base64Image;
            var imModel = new ImageModel { base64Image = imageString };
            return View(imModel);
        }

        public HttpResponseMessage RenderSnapshotBase64(string URL, string imageType, string imageWidth, string imageHeight)
        {
            URLToImage urlToIMage = new URLToImage();
            string base64Image= urlToIMage.GetSnapshot(URL, imageType, imageWidth, imageHeight);
            string imageString = "data:image/" + imageType + ";base64," + base64Image ;
            if (String.IsNullOrEmpty(base64Image))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            
            HttpResponseMessage response = new HttpResponseMessage { Content = new StreamContent(ms) };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            response.Content.Headers.ContentLength = (base64Image.Length);
            return response;
        }

       
    }
}
