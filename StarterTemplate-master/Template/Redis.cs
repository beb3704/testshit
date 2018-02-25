using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.Extensions.Caching.Distributed;

namespace Template
{
    public class Redis
    {
        private readonly IDistributedCache _cache;
        private bool Disconnected { get; set; }

        public Redis(IDistributedCache cache)
        {
            _cache = cache;
        }


        public async Task SetStringAsync(string key, string value)
        {
            if (!Disconnected)
            {
                try
                {
                    await _cache.SetStringAsync(key, value);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("It was not possible to connect"))
                    {
                        Disconnected = true;
                    }
                }
            }
        }

        public async Task<string> GetStringAsync(string key)
        {
            if (Disconnected) return null;
            try
            {
                return await _cache.GetStringAsync(key);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("It was not possible to connect"))
                {
                    Disconnected = true;
                }

                return null;
            }
        }
    }
}