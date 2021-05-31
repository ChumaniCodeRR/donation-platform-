using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer.Models
{
    public interface IPay
    {
        RequestReponse ProcessPayment(IPayable donation);

        RequestReponse SubscriptionPayment(IPayable donation);

        bool ValidatePayment(IPayable donation);

    }
}
