using System;
using System.Collections.Generic;
using System.Text;

namespace RentApp.Entities.BackEndEntities.Response
{
    public class UserResponse
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string partitionKey { get; set; }
        public string rowKey { get; set; }
        public DateTime timestamp { get; set; }
        public string eTag { get; set; }
    }
}
