using Microsoft.Xna.Framework.Graphics;

namespace bc.flash.resources
{
    public class BcTexture2D : BcManagedResource
    {
        private Texture2D mTexture;

        public BcTexture2D(Texture2D texture)
        {
            mTexture = texture;
        }

        public Texture2D Texture
        {
            get { return mTexture; }
        }

        public int Width
        {
            get { return mTexture.Width; }
        }

        public int Height
        {
            get { return mTexture.Height; }
        }

        public override void Dispose()
        {
            if (mTexture != null)
            {
                mTexture.Dispose();
                mTexture = null;
            }
        }
    }
}
