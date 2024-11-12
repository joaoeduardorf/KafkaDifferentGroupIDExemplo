using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivisionTwoNumbersConsumer
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string InputTopic { get; set; }
        public string OutputTopic { get; set; }
        public string GroupId { get; set; }
        public string Acks { get; set; }
    }
}
