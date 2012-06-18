using Microsoft.Xna.Framework.Content;

namespace bc.flash.resources
{
    public class BinaryContentReader : ContentTypeReader<BcBinaryData>
    {
        protected override BcBinaryData Read(ContentReader input, BcBinaryData existingInstance)
        {
            int size = input.ReadInt32();
            byte[] data = input.ReadBytes(size);

            if (existingInstance != null)
            {
                existingInstance.Data = data;
                return existingInstance;
            }

            return new BcBinaryData(data);
        }
    }
}
