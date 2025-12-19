using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using Backend;
using Backend.Entities;
using Backend.Enums;

namespace Backend.DataDto
{
    public class GuidDto
    {
        public Guid Id { get; set; } = Guid.Empty;
    }

}