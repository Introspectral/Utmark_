
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components
{
    public class QuantityComponent : IComponent
    {
        public int Quantity { get; set; }

        public QuantityComponent(int quantity)
        {
            Quantity = quantity;
        }

    }
}
