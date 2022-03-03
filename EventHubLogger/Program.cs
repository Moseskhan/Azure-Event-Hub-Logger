using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventHubLogger
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var init = new Init();
			await init.PublishEvents();
			await init.ReadEvents();
		}

	}

	public class Init
	{
		public EventHubConnection SetConn()
		{
			var conn = new EventHubConnection();
			conn.ConnectionString = "Endpoint=sb://moses-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=WH/fJWB1/v+HIct5XlNa1WwWx+eFIIeW+1IPfO1yoOw=;EntityPath=test-event-hub";
			conn.EventHubName = "test-event-hub";
			conn.ConsumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
			return conn;
		}


		public async Task<bool> PublishEvents()
		{
			var publisher = new EventPublisher(SetConn());
			var events = new List<EventData>();
			Console.WriteLine("-------------Start Publishing Events------------------------------");
			for (var i = 0; i < 20; i++)
			{
				var eventBody = new BinaryData($"Event Number: { i }");
				var eventData = new EventData(eventBody);
				events.Add(eventData);
			}


			await publisher.PublishAsync(events);
			Console.WriteLine("-------------End Publishing Events------------------------------");
			return true;

		}

		public async Task<bool> ReadEvents()
		{
			Console.WriteLine("------------Start Reading Events------------------------------");
			var reader = new EventReader(SetConn());
			var events = await reader.ReadAsync();
			
			foreach (var eventData in events)
			{
				Console.WriteLine(eventData);
			}
			Console.WriteLine("------------End Reading Events------------------------------");
			return true;

		}
	}
}
