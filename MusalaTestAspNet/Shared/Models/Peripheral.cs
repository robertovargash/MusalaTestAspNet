using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusalaTestAspNet.Shared.Models
{
    public enum PeripherialStatus
    {
        Offline, Online
    }
    public class Peripheral
    {
        public int Id { get; set; }
        public string Vendor { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public PeripherialStatus Status { get; set; }
        public Gateway Gateway { get; set; } = null!;
        public int GatewayId { get; set; }
    }
}
