using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Services
{
    public class SupabaseStorageService
    {
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public SupabaseStorageService(IConfiguration config)
        {
            _baseUrl = config["Supabase:BaseUrl"];
            _apiKey = config["Supabase:ApiKeyAnonPK"];
        }

        public async Task<string> UploadPlantImage(Stream stream, int userId)
        {
            var supabase = new Supabase.Client(_baseUrl, _apiKey);
            await supabase.InitializeAsync();

            var bucket = supabase.Storage.From("plants");

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            var bytes = ms.ToArray();

            var fileName = $"plant_{Guid.NewGuid()}.jpg";

            var path = $"user_{userId}/{fileName}";

            await bucket.Upload(bytes, path);

            return bucket.GetPublicUrl(path);
        }
    }
}
