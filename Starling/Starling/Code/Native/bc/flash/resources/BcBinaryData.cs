
namespace bc.flash.resources
{
    public class BcBinaryData : BcManagedResource
    {
        private byte[] data;

        public BcBinaryData(byte[] data)
        {
            this.data = data;
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public int Lenght
        {
            get { return data.Length; }
        }

        public override void Dispose()
        {
            data = null;
        }
    }
}
