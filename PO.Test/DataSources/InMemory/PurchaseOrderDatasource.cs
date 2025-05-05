using Application.Commands;
using Common.Utils;
using Common.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO.Test.DataSources.InMemory
{
    class PurchaseOrderDatasource
    {
        public static PurchaseOrderCommand GetValidPurchaseOrderDto(Guid rootGuid)
        {
            return new PurchaseOrderCommand(
             rootGuid,
             new UserDto("mohamed@gmail.com", "01061566310", "mohamed"),
             new UserDto("supplier@example.com", "01061566111", "supplier"),
             new List<ItemLineDto>
             {
                new ItemLineDto(
                     1, QuantityType.Tab,
                    "panadol",
                    22,
                    "SKU-123-123-123",
                    rootGuid
                )
             },
             NumberGenerator.PoAndyymmdd,
             "egypt,cairo"
         );
        }

    }
}
