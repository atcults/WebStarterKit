using System;

namespace Common.Helpers
{
    public class GuidComb
    {
         public static Guid New()
         {
             var dateBytes = BitConverter.GetBytes(SystemTime.Now().Ticks);
             var guidBytes = Guid.NewGuid().ToByteArray();
             
             // copy the last six bytes from the date to the last six bytes of the GUID
             Array.Copy(dateBytes, dateBytes.Length - 7, guidBytes, guidBytes.Length - 7, 6);
             return new Guid(guidBytes);
         }
    }
}