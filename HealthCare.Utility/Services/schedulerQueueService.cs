using HealthCare_Data.Identity;
using HealthCareData.Identity;
using HealthCareModels.Models.DTOs;
using HealthCareRepositorys.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCareRepositorys.Repository
{
    public class SchedulerQueueService : ISchedulerQueueService
    {
        private static readonly ConcurrentQueue<schedularDto> _queue = new();
        private static readonly SemaphoreSlim _semaphore = new(1, 1);
        private static bool _isProcessing = false;

        private readonly IServiceScopeFactory _scopeFactory;

        public SchedulerQueueService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task EnqueueAsync(schedularDto dto)
        {
            Console.WriteLine("📥 Enqueuing scheduler DTO...");
            _queue.Enqueue(dto);
            _ = ProcessQueueAsync(); 
            return Task.CompletedTask;
        }

        private async Task ProcessQueueAsync()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            try
            {
                while (!_queue.IsEmpty)
                {
                    if (_queue.TryPeek(out var dto))
                    {
                        Console.WriteLine($"⏳ Processing scheduler for user: {dto.ApplicationUserId}");
                        await Task.Delay(TimeSpan.FromSeconds(5)); 

                        if (_queue.TryDequeue(out dto))
                        {
                            await _semaphore.WaitAsync();
                            try
                            {
                                using var scope = _scopeFactory.CreateScope();
                                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                                var schedulerRepo = scope.ServiceProvider.GetRequiredService<IShedularRepository>();

                                var therapistId = dto.TherapistIds.FirstOrDefault();
                                var therapistExists = await context.Therapists.AnyAsync(t => t.TherapistId == therapistId);

                                if (!therapistExists)
                                {
                                    Console.WriteLine("❌ TherapistId not found: " + therapistId);
                                    continue; 
                                }

                                var treat = new treatment
                                {
                                    Title = dto.Title,
                                    DateCreated = DateTime.UtcNow,
                                    ApplicationUserId = dto.ApplicationUserId,
                                    TherapistId = therapistId
                                };

                                await context.treatments.AddAsync(treat);
                                await context.SaveChangesAsync();

                                var scheduler = new schedulerDate
                                {
                                    dateFrom = dto.DateFrom,
                                    dateTo = dto.DateTo,
                                    treatmentId = treat.treatmentId,
                                    treatment = treat,
                                    IsEmailSent = false, 
                                    SchedulerTherapists = dto.TherapistIds.Select(id => new SchedulerTherapist
                                    {
                                        TherapistId = id
                                    }).ToList()
                                };

                                await schedulerRepo.CreateAsync(scheduler);
                                await schedulerRepo.SaveAsync();

                                Console.WriteLine($"✅ Scheduler created for CaseId:{treat.treatmentId}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("❌ Error during scheduler processing:");
                                Console.WriteLine(ex);
                            }
                            finally
                            {
                                _semaphore.Release();
                            }
                        }
                    }
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }
    }
}
