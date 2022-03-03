using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubLogger
{
	public class EventPublisher
	{

		private EventHubConnection _connection;
		private EventHubProducerClient _producer;
		public EventPublisher(EventHubConnection connection)
		{
			_connection = connection;
			_producer = new EventHubProducerClient(_connection.ConnectionString);

		}

		public async Task<bool> PublishAsync(List<EventData> events)
		{
			using EventDataBatch eventBatch = await _producer.CreateBatchAsync();
			foreach (EventData eventData in events)
			{
				if (!eventBatch.TryAdd(eventData))
				{
					//Log event as unsuccessfully dispatched
					break;

				}
			}

			await _producer.SendAsync(eventBatch);

			return true;

		}
	}
}
