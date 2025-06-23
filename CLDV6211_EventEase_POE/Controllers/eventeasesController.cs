using Azure.Storage.Blobs;
using CLDV6211_EventEase_POE.Data;
using CLDV6211_EventEase_POE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CLDV6211_EventEase_POE.Controllers
{
    public class eventeasesController : Controller
    {
        private readonly CLDV6211_EventEase_POEContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public eventeasesController(CLDV6211_EventEase_POEContext context, IConfiguration config)
        {
            _context = context;
            _blobServiceClient = new BlobServiceClient(config
                ["AzureBlobStorage:ConnectionString"]);
            _containerName = config["AzureBlobStorage:ContainerName"];

        }

        public async Task <IActionResult> Index()
        {
            return View(await _context.eventeases.ToListAsync());
        }

        // Upload image and add to database
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile imageFile, string name, string description)
        {
            if (imageFile == null) return BadRequest("No image uploaded");
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
                return BadRequest("Name and description are required.");

            var container = _blobServiceClient.GetBlobContainerClient(_containerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(imageFile.FileName);

            using var stream = imageFile.OpenReadStream();
            await blob.UploadAsync(stream, overwrite: true);

            var existingEventImages = _context.eventeases
                .Where(e => e.Name.StartsWith(name))
                .ToList();

            string uniqueName = existingEventImages.Count > 0
                ? $"{name} {existingEventImages.Count + 1}"
                : name;

            var eventease = new eventease
            {
                Name = uniqueName,
                description = description,
                ImageUrl = blob.Uri.ToString()
            };

            _context.eventeases.Add(eventease);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Delete and remove from blob storage

        public async Task<IActionResult> Delete(int id)
        {
            var eventease = await _context.eventeases.FindAsync(id);
            if (eventease == null) return NotFound();

            var container = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Extrect the actual file name properly even if it has special characters

            var blobUri = new Uri(eventease.ImageUrl);
            var blobName = WebUtility.UrlDecode(blobUri.Segments.Last());
            var blob = container.GetBlobClient(blobName);

            if (_context.eventeases.Count(e => e.ImageUrl == eventease.ImageUrl) <= 1)
            {

                await blob.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
            }
                _context.eventeases.Remove(eventease);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }

