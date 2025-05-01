using MediaStore.Exceptions;

namespace MediaStore
{
    public partial class Function
    {
        public class ProductModifier
        {
            public void AddProduct(MediaUnit media)
            {

            }

            public void RemoveProduct(UID id)
            {

            }

            public void UpdateProduct(UID id, (MediaProperty property, string value)[] modifiers)
            {
                if (DataContainer.MediaContainer.MediaUnits.TryGetValue(id, out MediaUnit? unit))
                {
                    foreach (var modifier in modifiers)
                    {
                        unit.Information.Properties[modifier.property] = modifier.value;
                    }
                }
                else
                {
                    throw new MediaNotFoundException($"Media with ID {id} not found.");
                }
            }
        }
    }
}
