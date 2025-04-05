using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmLock.API.Models;

public class Device
{
    public int Id { get; set; }
    public string IMEI { get; set; }
    public string Model { get; set; }
    public string OwnerName { get; set; }
    public string OwnerPhone { get; set; }
    public bool IsLocked { get; set; } = false;
    public int UserId { get; set; } // Foreign key

    [ForeignKey("UserId")]
    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public List<EmiSchedule> EmiSchedules { get; set; } = new List<EmiSchedule>();

}
