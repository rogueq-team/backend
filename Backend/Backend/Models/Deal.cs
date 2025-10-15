using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Deal
    {
        private int _dealId = 0;
        private int _applicationId = 0;
        private int _advertiserId = 0;
        private int _platformId = 0;
        private string _description = string.Empty;
        private string _status = string.Empty;
        private DateTime _createdAt = DateTime.Now;

        [JsonIgnore]
        public int DealId { get { return _dealId; } set { _dealId = value; } }

        public int ApplicationId { get { return _applicationId; } set { _applicationId= value; } }

        public int AdvertiserId{ get { return _advertiserId; } set { _advertiserId = value; } }

        public int PlatformId { get { return _platformId; } set { _platformId = value; } }

        public string Description { get { return _description; } set { _description = value; } }
        
        public string Status { get { return _status; } set { _status = value; } }
        
        public DateTime Date {get{ return _createdAt; } }
    }
}
