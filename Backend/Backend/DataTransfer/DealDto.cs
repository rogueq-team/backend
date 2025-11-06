using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using Backend;
using Backend.Entities;
using Backend.Enums;

namespace Backend.DataDto
{
    public class DealDto
    {
        public Guid DealId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AdvertiserId { get; set; }
        public Guid PlatformId { get; set; }
        public string? Description { get; set; }
        public DealStatus Status { get; set; } //inProgress   isOver    cancel
        public UserDeal? Advertiser{ get; set; }
        public UserDeal? Platform;
        public DealDto() { }
        public DealDto(DealEntity Deal)
        {
            DealId = Deal.DealId;
            ApplicationId = Deal.ApplicationId;
            AdvertiserId = Deal.AdvertiserId;
            PlatformId = Deal.PlatformId;
            Description = Deal.Description;
            Status = Deal.Status;
            Advertiser = new UserDeal(Deal.Advertiser);
            Platform = new UserDeal(Deal.Platform);
        }
    }


    public class UserDeal
    {
        Guid UserId;
        string Login=string.Empty;
        string Email = string.Empty;
        public UserDeal(UserEntity? user)
        {
            if (user == null)
                UserId = Guid.Empty;
            else
            {
                UserId = user.UserId;
                Login = user.Login;
                Email = user.Login;
            }
        }

    } 

}