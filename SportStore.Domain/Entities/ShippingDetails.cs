using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Domain.Entities
{
    class ShippingDetails
    {
        [Required(ErrorMessage ="Por favor inserte el Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Por favor agregue la primera línea de dirección")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage ="Por favopr introduzca el nombre de la Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage ="Por favor intrduzca el estado")]
        public string State { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage ="Por favor introduzca el País")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }

    }
}
