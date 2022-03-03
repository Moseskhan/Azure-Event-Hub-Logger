using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHubLogger
{
	public class EventReader
	{
		private EventHubConnection _connection;
		private EventHubConsumerClient _consumer;

		public EventReader(EventHubConnection connection)
		{
			_connection = connection;
			_consumer = new EventHubConsumerClient(connection.ConsumerGroup, connection.ConnectionString);
		}

		public async Task<List<string>> ReadAsync()
		{
			using var cancellationSource = new CancellationTokenSource();
			cancellationSource.CancelAfter(TimeSpan.FromSeconds(500));
			//var options = new ReadEventOptions
			//{
			//	MaximumWaitTime = TimeSpan.FromSeconds(1)
			//};
			DateTimeOffset oneHourAgo = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1));
			EventPosition startingPosition = EventPosition.FromEnqueuedTime(oneHourAgo);
			List<string> events = new List<string>();
			await foreach (PartitionEvent partitionEvent in _consumer.ReadEventsAsync(cancellationSource.Token))
			{
				events.Add(partitionEvent.Data.EventBody.ToString());
				File.AppendAllText(@"C:\Repos\test-event-hub.txt", partitionEvent.Data.EventBody.ToString() + Environment.NewLine);
			}

			await _consumer.CloseAsync();

			return events;

		}
	}
}
