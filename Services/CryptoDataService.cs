using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;           // <-- сюда
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CryptoMonitor.Data;                       // <-- ваш DbContext
using CryptoMonitor.Models;                     // <-- ваши сущности


namespace CryptoMonitor.Services
{
    public class CryptoDataService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _http;

        public CryptoDataService(IServiceScopeFactory scopeFactory, IHttpClientFactory httpFactory)
        {
            _scopeFactory = scopeFactory;
            _http = httpFactory.CreateClient("coinGecko");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<CryptoMonitorDbContext>();
                var source = await db.ApiSources.FirstAsync(s => s.Name == "CoinGecko", stoppingToken);

                try
                {
                    // Запросим рынки
                    var response = await _http.GetAsync(
                    "coins/markets?vs_currency=usd&order=market_cap_desc&per_page=50&page=1",
                    stoppingToken);

                    if (!response.IsSuccessStatusCode)
                        throw new Exception($"CoinGecko вернул {(int)response.StatusCode} {response.StatusCode}");

                    var json = await response.Content.ReadAsStringAsync(stoppingToken);
                    Console.WriteLine($"[CryptoDataService] JSON length = {json?.Length}");
                    var list = JsonSerializer.Deserialize<List<CoinMarketDto>>(json, 
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    Console.WriteLine($"[CryptoDataService] Parsed {list?.Count ?? 0} coins");

                    foreach (var coin in list)
                    {
                        var asset = await db.CryptoAssets
                            .FirstOrDefaultAsync(a => a.Symbol == coin.Symbol, stoppingToken)
                            ?? db.CryptoAssets.Add(new CryptoAsset
                            {
                                Symbol = coin.Symbol,
                                Name = coin.Name,
                                ImageUrl = coin.Image
                            }).Entity;

                        db.PriceHistories.Add(new PriceHistory
                        {
                            Asset = asset,
                            Timestamp = DateTime.UtcNow,
                            PriceUsd = coin.CurrentPrice,
                            Volume24h = coin.TotalVolume,
                            MarketCap = coin.MarketCap
                        });
                    }

                    db.DataUpdateLogs.Add(new DataUpdateLog
                    {
                        ApiSourceId = source.SourceId,
                        RunTime = DateTime.UtcNow,
                        Status = true,
                        Message = "OK"
                    });

                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    db.DataUpdateLogs.Add(new DataUpdateLog
                    {
                        ApiSourceId = source.SourceId,
                        RunTime = DateTime.UtcNow,
                        Status = false,
                        Message = ex.Message
                    });
                    await db.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(3600), stoppingToken);
            }
        }
    }

}
