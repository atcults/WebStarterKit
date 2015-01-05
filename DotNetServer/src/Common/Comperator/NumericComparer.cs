using System.Collections;

namespace Common.Comperator
{
    public class NumericComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if ((x is string) && (y is string))
            {
                return CompaireNatural.Compare((string)x, (string)y);
            }
            return -1;
        }

    }
}

//public class Test
//{
//    public static void Main(string[] args)
//    {

//        string[] files = new string[] {
//            "1.txt", "_1.txt",
//            "[1.txt", "=1.txt",
//            "10.txt", "3.txt",
//            "a10b1.txt", "a1b1.txt",
//            "a2b1.txt", "a2b11.txt",
//            "a2b2.txt", "b1.txt",
//            "b10.txt", "b2.txt",
//            "b[1.txt", "b01.txt",
//            "c30.txt", "c25.txt",
//            "c35.txt", "c40.txt",
//            "BIG.txt", "big.txt"};

//        try
//        {

//            NumericComparer nc = new NumericComparer();
//            Array.Sort(files, nc);
//            foreach (string file in files)
//            {
//                Console.WriteLine(file);
//            }

//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
//        }
//    }
//}