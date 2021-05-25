using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalGameShoppingConcoleCRUD
{
    class Program
    {
        static void Main(string[] args)
        {
          
            CrudOperation crud = new CrudOperation();
          
            do
            {
                int choice = DisplayMenu();

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        crud.PurchaseGames();
                        break;

                    case 2:
                        Console.Clear();
                        crud.CustomerOrders();
                        break;

                    case 3:
                        Console.Clear();
                        crud.AllOrderDisplay();
                        crud.holdAndClear();
                        break;

                    
                    case 4: // Exit
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Thank you for using our application!!!\nBye!");
                        Console.ForegroundColor = ConsoleColor.White;

                        Environment.Exit(0);
                        break;
                    case 991590858:
                        crud.AdminStockUpdate();
                        break;

                    default:
                        Console.WriteLine("Please only select from the given choice.");
                        break;

                }
            } while (true);

        }
        static int DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            for (int i = 1; i < 87; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine();

            Console.WriteLine("\n\n\t\t\tFinal Exam by PARAS PATEL\n");
            for (int i = 1; i < 87; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine("\n");
            //Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+\n");
            Console.WriteLine("\t\t\t1 - Purchase Games");
            Console.WriteLine("\t\t\t2 - View Customer's Transaction History");
            Console.WriteLine("\t\t\t3 - View All Transactions");
            Console.WriteLine("\t\t\t4 - Exit");
            Console.WriteLine("_______________________________________________________");
            Console.Write("\nEnter your choice: ");
            Console.ForegroundColor = ConsoleColor.White;
            return int.Parse(Console.ReadLine());


        }
    }
}
