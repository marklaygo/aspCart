using aspCart.Core.Domain.Catalog;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Web.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Areas.Admin.Controllers
{
    public class ImageManagerController : AdminController
    {
        #region Fields

        private readonly IImageManagerService _imageManagerService;
        private readonly IHostingEnvironment _hostingEnvironment;

        #endregion

        #region Constructor

        public ImageManagerController(
            IHostingEnvironment hostingEnvironment,
            IImageManagerService imageManagerService)
        {
            _imageManagerService = imageManagerService;
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion

        #region Methods

        // GET: /ImageManager/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /ImageManager/GetAllImages
        public IActionResult GetAllImages()
        {
            // get all image from database
            var imageList = _imageManagerService.GetAllImages();
            var model = new List<ImageModel>();

            foreach (var image in imageList)
            {
                var imageModel = new ImageModel
                {
                    Id = image.Id,
                    FileName = image.FileName
                };
                model.Add(imageModel);
            }

            return Json(model);
        }

        // POST: /ImageManager/SearchImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchImage(string keyword)
        {
            if (keyword == "" || keyword == null)
                return new NoContentResult();

            var imageList = _imageManagerService.SearchImages(keyword);

            var model = new List<ImageModel>();

            foreach (var image in imageList)
            {
                var imageModel = new ImageModel
                {
                    Id = image.Id,
                    FileName = image.FileName
                };
                model.Add(imageModel);
            }

            return Json(model);
        }

        // POST: /ImageManager/UploadImages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImages()
        {
            var files = Request.Form.Files;

            if (files.Count > 0)
            {
                var imageList = new List<Image>();
                var dir = Path.Combine(_hostingEnvironment.WebRootPath, "images/app");
                Directory.CreateDirectory(dir);

                try
                {
                    foreach (var file in files)
                    {
                        var imagePath = Path.Combine(dir, file.FileName);
                        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(imagePath);
                        var ext = Path.GetExtension(imagePath);
                        var imageFileName = fileNameWithoutExt + "." + Guid.NewGuid().ToString().Substring(0, 8) + ext;
                        imagePath = Path.Combine(dir, imageFileName);

                        var productImage = new Image
                        {
                            Id = Guid.NewGuid(),
                            FileName = "/images/app/" + imageFileName
                        };

                        // save image to local disk
                        using (FileStream fs = System.IO.File.Create(Path.Combine(dir, imagePath)))
                        {
                            await file.CopyToAsync(fs);
                            imageList.Add(productImage);
                        }
                    }

                    // save image info to database
                    _imageManagerService.InsertImages(imageList);
                    return new NoContentResult();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return Json("error");
        }

        // POST: /ImageManager/DeleteImages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteImages(List<string> Ids)
        {
            try
            {
                var imageToDelete = new List<Guid>();

                foreach (var id in Ids)
                {
                    var image = _imageManagerService.GetImageById(Guid.Parse(id));

                    if (image != null)
                    {
                        // delete image from local disk
                        var dir = Path.Combine(_hostingEnvironment.WebRootPath, "images/app");
                        var imagePath = Path.Combine(dir, image.FileName);

                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);

                        imageToDelete.Add(image.Id);
                    }
                }

                // delete image from database
                _imageManagerService.DeleteImages(imageToDelete);

                return new NoContentResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
