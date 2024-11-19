namespace Basilisk
{
    public class Bitfield
    {
        private byte[] bitfield;

        public Bitfield(int totalPieces)
        {
            int byteCount = (totalPieces + 7) / 8;  
            bitfield = new byte[byteCount];
        }

       
        public void SetPiece(int index)
        {
            int byteIndex = index / 8;
            int bitIndex = index % 8;
            bitfield[byteIndex] |= (byte)(1 << (7 - bitIndex));
        }

        
        public byte[] GetBytes()
        {
            return bitfield;
        }

       
        public void PrintBitfield()
        {
            for (int i = 0; i < bitfield.Length; i++)
            {
                Console.WriteLine($"Byte {i}: {Convert.ToString(bitfield[i], 2).PadLeft(8, '0')}");
            }
        }
    }
}
