using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FinalGameShoppingConcoleCRUD
{
    class CrudOperation
    {
        internal void PurchaseGames() {
            using (var context = new GameShoppingDBEntities()) {
                bool cond = true;
                int gameID, gameQty;
                double ttlPrice = 0.00, discount = 0.00;
                var inStock = 0;
                var gamePrice = 0.00; // =0;
                Console.WriteLine("\n\n\tPurchasing Games\n");
                Console.Write("\nEnter customer name: ");
                String customerName = Console.ReadLine();
                DisplayGameList();
                while (cond == true){
                    Console.Write("\n\nEnter game Id: ");
                    gameID = int.Parse(Console.ReadLine());
                    var validID = (from game in context.Games
                                  where game.GameId == gameID
                                  select game.GameId).FirstOrDefault();
                    
                    if (validID > 0)
                    {
                         inStock = (from game in context.Games
                                      where game.GameId == validID
                                      select game.Stock).FirstOrDefault();
                        if (inStock <= 0)
                        {
                            Console.WriteLine("Sorry, this game is out of stock. Please check back later.");
                        }
                        else {
                            
                            Console.Write("\nEnter Quality (More than 5 = 10% discount): ");
                            gameQty = int.Parse(Console.ReadLine());
                            if (gameQty > inStock) {
                                Console.Write("\nPlease enter a quality less than stock.\n");
                            }
                            else {
                                 gamePrice = (from price in context.Games
                                             where price.GameId == gameID
                                             select price.Price).FirstOrDefault();
                                ttlPrice = gamePrice * gameQty;
                                if (gameQty >= 5)
                                {
                                    discount = ttlPrice * 0.1;
                                    ttlPrice = ttlPrice - discount;
                                }

                                var existCustName = (from cust in context.Customers
                                                     where cust.Name.ToUpper() ==  customerName.ToUpper()
                                                     select cust.CustomerId).FirstOrDefault();
                                if (existCustName == 0) {
                                    Customer customer = new Customer();
                                    customer.Name = customerName;
                                    context.Customers.Add(customer);
                                }

                                Order order = new Order();
                                order.GameId = gameID;
                                order.Date = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yy"));
                                order.Quantity = gameQty;
                                order.CustomerId = existCustName;
                                order.Discount = discount;
                                context.Orders.Add(order);
                                Game game = context.Games.Find(gameID);
                                game.Stock = game.Stock - gameQty;
                                context.SaveChanges();
                                ProcessFun();
                                Console.WriteLine("\n\nYour order has been placed. Thank you for shopping with us.");
                                /**Display msg*/
                                LastOrderDisplay(order.OrderId);
                                cond = false;
                                holdAndClear();
                            }
                                                        
                        }
                    }
                    else {
                        Console.WriteLine("Invalid Game Id. Please try again.");
                    }
                }
            }
     }
        /* from https://stackoverflow.com/a/1379461 
         *idea from stake overflow, and modified it
         */
        internal void ProcessFun() {
            Console.Write("\nPlacing your order");
            Thread.Sleep(500); Console.Write(".");
            Thread.Sleep(500); Console.Write(".");
            Thread.Sleep(500);  Console.Write(".");
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i <= 100; i++)
            {
              
                Console.Write("\rOrder Approving {0}%", i);
                Thread.Sleep(12);
                if (i == 100) {
                    Console.Write("\rOrder Approval Confirmed 100 %");

                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        internal void PrintFor(int x)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            for (int i = 1; i < x; i++)
            {
                Console.Write("-");
            }
            Console.ForegroundColor = ConsoleColor.White;

        }
        internal void LastOrderDisplay(int oid) {
            using (var context = new GameShoppingDBEntities())
            {
                var display = (from displayOrder in context.Orders
                               where displayOrder.OrderId == oid
                               select displayOrder).FirstOrDefault();
                PrintFor(150);
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"\n| {"Order Id",10} | {"Date",12} | {"Customer",-18} | {"Game",-18} | {"Price",10} | {"Quantity",10} | {"Total",10} | {"Discount",10} | {"Tax",10} | {"Net Total",10} |");
                Console.ForegroundColor = ConsoleColor.White;

                PrintFor(150);
                Console.WriteLine();
                double totalAmount = display.Game.Price * display.Quantity;
                double tax = (totalAmount - display.Discount) * 0.13;
                double netTotal = totalAmount - display.Discount + tax;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"| {display.OrderId,10} | {display.Date.ToString("dd-MMM-yy"),12} | {display.Customer.Name,-18} | {display.Game.Name,-18} | {display.Game.Price,10:C2} | {display.Quantity,10} | {totalAmount,10:C2} | {display.Discount,10:C2} | {tax,10:C2} | {netTotal,10:C2} |");
                    Console.ForegroundColor = ConsoleColor.White;

                PrintFor(150);
                Console.WriteLine();
            }

        }
        internal void CustomerOrders() {
            using (var context = new GameShoppingDBEntities())
            {
                bool cond = true;
                DisplayCustomerList();
                while (cond == true)
                {
                    Console.Write("\n\nEnter customer Id: ");
                    int custID = int.Parse(Console.ReadLine());
                    var validID = (from customer in context.Customers
                                   where customer.CustomerId == custID
                                   select customer.CustomerId).FirstOrDefault();
                    if (validID > 0 ) {
                        var displayorders = (from displayOrder in context.Orders
                                             where displayOrder.CustomerId == custID
                                             select displayOrder).ToList();
                        
                        PrintFor(150);
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine($"\n| {"Order Id",10} | {"Date",12} | {"Customer",-18} | {"Game",-18} | {"Price",10} | {"Quantity",10} | {"Total",10} | {"Discount",10} | {"Tax",10} | {"Net Total",10} |");
                Console.ForegroundColor = ConsoleColor.White;

                        PrintFor(150);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        foreach (var display in displayorders)
                        {
                            double totalAmount = display.Game.Price * display.Quantity;
                            double tax = (totalAmount - display.Discount) * 0.13;
                            double netTotal = totalAmount - display.Discount + tax;

                            Console.WriteLine($"| {display.OrderId,10} | {display.Date.ToString("dd-MMM-yy"),12} | {display.Customer.Name,-18} | {display.Game.Name,-18} | {display.Game.Price,10:C2} | {display.Quantity,10} | {totalAmount,10:C2} | {display.Discount,10:C2} | {tax,10:C2} | {netTotal,10:C2} |");
                        }
                        Console.ForegroundColor = ConsoleColor.White;

                        PrintFor(150);

                        cond = false;
                        holdAndClear();
                    }
                    else {
                        Console.WriteLine("Please enter only valid Customer ID. ");
                    }
                }
            }
        }

        /*View all Transaction*/
        internal void AllOrderDisplay()
        {
            using (var context = new GameShoppingDBEntities())
            {
                var displayorders = (from displayOrder in context.Orders
                               select displayOrder).ToList();
                if (displayorders.Count() > 0)
                {
                    Console.WriteLine("\n\n\tAll Orders: \n");

                PrintFor(137);
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"\n| {"Order Id",10} | {"Date",12} | {"Customer",-18} | {"Game",-18} | {"Price",10} | {"Quantity",10} | {"Discount",10} | {"Tax",10} | {"Total",10} |");
                        Console.ForegroundColor = ConsoleColor.White;

                    PrintFor(137);
                Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    foreach (var display in displayorders) {
                    double totalAmount = display.Game.Price * display.Quantity;
                    double tax = (totalAmount - display.Discount) * 0.13;
                    double netTotal = totalAmount - display.Discount + tax;

                    Console.WriteLine($"| {display.OrderId,10} | {display.Date.ToString("dd-MMM-yy"),12} | {display.Customer.Name,-18} | {display.Game.Name,-18} | {display.Game.Price,10:C2} | {display.Quantity,10} | {display.Discount,10:C2} | {tax,10:C2} | {netTotal,10:C2} |");
                }
                    Console.ForegroundColor = ConsoleColor.White;

                    PrintFor(137);
                }
                else
                {
                    Console.WriteLine("No Data in the Order Table!!!\nNo record(s) fetched.");
                }
            }

        }

    


        internal void DisplayGameList()
        {
            using (var context = new GameShoppingDBEntities())
            {
                var gameList = from game in context.Games
                            select game;
                Console.WriteLine("\nGames List\n");
                PrintFor(55);
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"\n| {"Game ID",10} | {"Name",-15} | {"Price",8} | {"In Stock",8} |");
                    Console.ForegroundColor = ConsoleColor.White;

                PrintFor(55);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;

                foreach (var game in gameList)
                {
                    Console.WriteLine($"| {game.GameId,10} | {game.Name,-15} | {game.Price.ToString("C"),8} | {game.Stock,8} |");
                }
                Console.ForegroundColor = ConsoleColor.White;

                PrintFor(55);

            }
        }
        internal void DisplayCustomerList()
        {
            using (var context = new GameShoppingDBEntities())
            {
                var customerList = from customer in context.Customers
                               select customer;
                
                if (customerList.Count() > 0)
                
                {
                    Console.WriteLine("\n\n\tCustomer List\n");
                    PrintFor(35);
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"\n| {"Customer ID",12} | {"Name",-15} |");
                    Console.ForegroundColor = ConsoleColor.White;

                    PrintFor(35);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    foreach (var customer in customerList)
                    {
                        Console.WriteLine($"| {customer.CustomerId,12} | {customer.Name,-15} |");
                    }
                    Console.ForegroundColor = ConsoleColor.White;

                    PrintFor(35);

                }
                else {
                    Console.WriteLine("No record(s) fetched. Please Check Back Later. ");
                }
            }
        }
        //Wait for input and back
        internal void holdAndClear()
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("\n\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        internal void AdminStockUpdate() {
            using (var context = new GameShoppingDBEntities())
            {
                void Increase(int id, int st) {
                    Game game = context.Games.Find(id);
                    game.Stock = game.Stock + st;
                }
                bool cond = true;
                while (cond == true)
                {
                    Console.WriteLine("\nDo you want to add stocks, Enter 1.\n Enter any other NUMBER to exit.  ");
                    int positive =  int.Parse(Console.ReadLine());
                    if (positive == 1) {
                        DisplayGameList();
                        Console.Write("\nEnter Game ID: ");
                        int gid = int.Parse(Console.ReadLine());
                        Console.Write("\nEnter stock to Add: ");
                        int gst = int.Parse(Console.ReadLine());
                        Increase(gid, gst);
                        //Increase(1, 10);
                         //Increase(2, 5);
                         //Increase(3, 1);
                         //Increase(4, 0);
                         //Increase(5, 2);
                        
                        context.SaveChanges();
                        Console.WriteLine("Stocks Updated!!!");
                        holdAndClear();
                    }
                    cond = false;

                }
            }
        }

    }
   
}
