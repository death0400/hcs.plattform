using System.Threading.Tasks;
using Hcs.Platform.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hcs.Platform.Core.Controller
{
    [Route("api/file")]
    public class FileController : ControllerBase
    {
        private readonly IFileManager fileManager;

        public FileController(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }
        [HttpPost("{*dir}")]
        [Authorize]
        public async Task<IActionResult> Post(IFormFile file, string dir)
        {
            return Ok(await fileManager.Create(file.OpenReadStream(), file.FileName, dir, file.ContentType));
        }
        [HttpGet("{key}")]
        [Authorize]
        public async Task<IActionResult> Get(string key)
        {
            var fi = await fileManager.GetFileInfo(key);
            if (fi == null)
            {
                return NotFound();
            }
            return Ok(fi);
        }
        [HttpGet("{key}/{*name}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string key, string name)
        {
            var fi = await fileManager.GetFileInfo(key);
            if (fi == null)
            {
                return NotFound();
            }
            if (Request.Headers.Keys.Contains("If-None-Match") && Request.Headers["If-None-Match"].ToString() == fi.ETag)
            {
                return StatusCode(304);
            }
            Response.Headers.Add("ETag", new[] { fi.ETag });
            var stream = await fileManager.Open(key);
            if (stream == null)
            {
                return NotFound();
            }
            return File(stream, fi.MimeType);
        }
        [HttpGet("download/{key}/{*name}")]
        [AllowAnonymous]
        public async Task<IActionResult> Download(string key, string name)
        {
            var fi = await fileManager.GetFileInfo(key);
            if (fi == null)
            {
                return NotFound();
            }
            if (Request.Headers.Keys.Contains("If-None-Match") && Request.Headers["If-None-Match"].ToString() == fi.ETag)
            {
                return StatusCode(304);
            }
            Response.Headers.Add("ETag", new[] { fi.ETag });
            var stream = await fileManager.Open(key);
            if (stream == null)
            {
                return NotFound();
            }
            return File(stream, fi.MimeType, fi.Name);
        }
    }
}