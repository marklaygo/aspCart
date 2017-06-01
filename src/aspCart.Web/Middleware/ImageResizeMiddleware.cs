using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Middleware
{
    public class ImageResizeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFileProvider _fileProvider;

        public ImageResizeMiddleware(RequestDelegate next, IFileProvider fileProvider)
        {
            _next = next;
            _fileProvider = fileProvider;
        }

        public Task Invoke(HttpContext context)
        {
            var imageExtention = new List<string> { "jpg", "png" };
            bool isImage = imageExtention.Any(ext => context.Request.Path.ToString().ToLower().EndsWith(ext));

            // check if the request is a valid image
            if(isImage)
            {
                var imagePath = $"wwwroot/{Uri.UnescapeDataString(context.Request.Path.ToString())}";

                // check if the image request have query string,
                // otherwise, return to next middleware
                if(context.Request.QueryString.HasValue)
                {
                    int w = 0, h = 0;

                    var imageQueryString = QueryHelpers.ParseQuery(context.Request.QueryString.ToString());

                    if (imageQueryString.ContainsKey("w"))
                        w = Convert.ToInt32(imageQueryString["w"]);

                    if (imageQueryString.ContainsKey("h"))
                        h = Convert.ToInt32(imageQueryString["h"]);

                    // if either width or height is present in the query string,
                    // resize the image
                    if(w > 0 || h > 0)
                    {
                        using (var input = System.IO.File.OpenRead(imagePath))
                        {
                            var bitmap = SKBitmap.Decode(input);

                            w = w == 0 ? bitmap.Width : w;
                            h = h == 0 ? bitmap.Height : h;

                            var scaledBitmap = bitmap.Resize(new SKImageInfo(w, h), SKBitmapResizeMethod.Lanczos3);

                            using (var image = SKImage.FromBitmap(scaledBitmap))
                            using (var data = image.Encode())
                            using (var ms = new System.IO.MemoryStream())
                            {
                                data.SaveTo(ms);
                                context.Response.Body.WriteAsync(ms.ToArray(), 0, ms.ToArray().Length);

                                return Task.CompletedTask;
                            }
                        }
                    }
                }
            }

            return _next(context);
        }
    }

    public static class ImageResizeMiddlewareExtentions
    {
        public static IApplicationBuilder UseImageResize(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageResizeMiddleware>();
        }
    }
}
