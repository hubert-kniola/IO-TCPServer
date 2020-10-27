using System;

namespace ServerLibrary
{
    /// <summary>
    /// Klasa odpowiedzialna za obliczenia wykonywane przez serwer
    /// </summary>
    public class Calculations
    {
        #region Methods
        /// <summary>
        /// Metoda przetwarzająca dane podane przez użytkownika na wymagany wynik
        /// </summary>
        /// <param name="n">Wartość podawana przez użytkownika</param>
        /// <returns></returns>
        public static int printResult(int n)
        {
            return n * 2;
        }

        /// <summary>
        /// Metoda wykonująca działania matematyczne
        /// </summary>
        /// <param name="one">Pierwsza liczba podana przez użytkownika</param>
        /// <param name="two">Druga liczba podana przez użytkownika</param>
        /// <returns></returns>
        public static int[] calculator(int one, int two)
        {
            int[] tb = new int[4];
            tb[0] = one + two;
            tb[1] = one - two;
            tb[2] = one * two;
            tb[3] = (int)Math.Pow(one, two);
            return tb;
        }

        /// <summary>
        /// Metoda odpowiedzialna za tworzenie tablicy zmiennych typu string
        /// </summary>
        /// <param name="one">Pierwsza liczba podana przez użytkownika</param>
        /// <param name="two">Druga liczba podana przez użytkownika</param>
        /// <param name="tb">Tablica zawierająca wynik operacji matematycznych na liczbach</param>
        /// <returns></returns>
        public static string[] printCalculator(int one, int two, int[] tb)
        {
            string[] tbs = new string[4];
            tbs[0] = one.ToString() + " + " + two.ToString() + " = " + tb[0].ToString() + "\r\n";
            tbs[1] = one.ToString() + " - " + two.ToString() + " = " + tb[1].ToString() + "\r\n";
            tbs[2] = one.ToString() + " * " + two.ToString() + " = " + tb[2].ToString() + "\r\n";
            tbs[3] = one.ToString() + " ^ " + two.ToString() + " = " + tb[3].ToString() + "\r\n";
            return tbs;
        }
        #endregion
    }
}
