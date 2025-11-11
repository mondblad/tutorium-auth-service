namespace Tutorium.AuthService.Core.Models.Kafka
{
    public class KafkaOptions
    {
        public string Bootstrap { get; set; }
        public string TopicToAuth { get; set; }
    }
}
