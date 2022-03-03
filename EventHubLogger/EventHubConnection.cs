using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubLogger
{
	public class EventHubConnection
	{
		public string ConnectionString { get; set; }
		public string EventHubName { get; set; }
		public string ConsumerGroup { get; set; } = EventHubConsumerClient.DefaultConsumerGroupName;
	}
}
