using MediaStore.Exceptions;
using System;

namespace MediaStore
{
    public partial class Function
    {
        public class ProductModifier
        {
            public void AddProduct(UID id)
            {
                if (DataContainer.MediaContainer.MediaUnits.ContainsKey(id))
                {
                    throw new InvalidOperationException($"Product with UID {id} already exists. Cannot add duplicate product.");
                }

                MediaUnit newProduct = new MediaUnit();
                // The newProduct is created with default values for its properties (Information, Review, Price).
                // These would need to be populated later, e.g., using the UpdateProduct method.

                if (!DataContainer.MediaContainer.MediaUnits.TryAdd(id, newProduct))
                {
                    // This case implies an unexpected issue, potentially a race condition
                    // if ContainsKey was false but TryAdd failed.
                    throw new Exception($"Failed to add product with UID {id} to the MediaContainer due to an unexpected issue.");
                }
                // Product slot created. Further details should be added via UpdateProduct.
            }

            public void RemoveProduct(UID id)
            {
                if (!DataContainer.MediaContainer.MediaUnits.TryRemove(id, out _))
                {
                    throw new MediaNotFoundException($"Product with UID {id} not found. Cannot remove non-existent product.");
                }
                // Product successfully removed.
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
