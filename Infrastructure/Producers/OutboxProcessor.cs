using Common.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Producers
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IEventRepository _eventRepo;
        private readonly IProducer _producer;
        private readonly string _topic;

        public OutboxProcessor(IEventRepository eventRepo, IProducer producer, IOptions<Topic> options)
        {
            _eventRepo = eventRepo;
            _producer = producer;
            _topic = options.Value.TopicName;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var unprocessed = await _eventRepo.GetUnprocessedEventsAsync(); 
                foreach (var evt in unprocessed)
                {
                    try
                    {
                        await _producer.ProduceAsync(_topic, evt.EventBaseData);
                        await _eventRepo.MarkAsProcessed(evt.EventBaseData.Id);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

                await Task.Delay(5000, stoppingToken); 
            }
        }
    }

}
