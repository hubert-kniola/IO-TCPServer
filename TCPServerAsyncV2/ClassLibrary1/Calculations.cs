using System;

namespace ServerLibrary
{
    /// <summary>
    /// Klasa odpowiedzialna za obliczenia wykonywane przez serwer
    /// </summary>
    public class Calculations
    {
        /// <summary>
        /// Metoda przetwarzająca dane podane przez użytkownika na wymagany wynik
        /// </summary>
        /// <param name="n">Wartość podawana przez użytkownika</param>
        /// <returns></returns>
        public static int printResult(int n)
        {
            return n * 2;
        }

        public static int[] calculator(int one, int two)
        {
            int[] tb = new int[4];
            tb[0] = one + two;
            tb[1] = one - two;
            tb[2] = one * two;
            tb[3] = (int)Math.Pow(one, two);
            return tb;
        }

        public static string[] printCalculator(int one, int two, int[] tb)
        {
            string[] tbs = new string[4];
            tbs[0] = one.ToString() + " + " + two.ToString() + " = " + tb[0].ToString() + "\r\n";
            tbs[1] = one.ToString() + " - " + two.ToString() + " = " + tb[1].ToString() + "\r\n";
            tbs[2] = one.ToString() + " * " + two.ToString() + " = " + tb[2].ToString() + "\r\n";
            tbs[3] = one.ToString() + " ^ " + two.ToString() + " = " + tb[3].ToString() + "\r\n";
            return tbs;
        }
    }
}
