using МашинкиСупер;

namespace МашинкиСупер
{
    class Program
    {
        static void Main(string[] args)
        {
            Avto avto = new Avto("", 0, 0, 0, 0, 0, 0);  //Связанные классы
            Avto[] cars = new Avto[3];                   //Массив машин
            Random rnd = new Random();

            for (int i = 0; i < cars.Length; i++)
            {
                Console.WriteLine($"Машина {i + 1}");
                Console.Write("Введите номер машины: ");
                string carNumber = Console.ReadLine();

                Console.Write("Введите максимальный объем бака: ");
                double max_benzin_bak = Convert.ToDouble(Console.ReadLine());

                while (max_benzin_bak <= 0) //Проверка на дурака
                {
                    Console.Write("Введите заново: ");
                    max_benzin_bak = Convert.ToDouble(Console.ReadLine());
                }

                Console.Write("Введите текущий обьем бака: ");
                double benzin_bak_volume = Convert.ToDouble(Console.ReadLine());

                while (benzin_bak_volume > max_benzin_bak) //Проверка на дурака
                {
                    Console.Write("Объем бензина в баке не может превышать объем бака. Введите заново: ");
                    benzin_bak_volume = Convert.ToDouble(Console.ReadLine());
                }

                double distance = rnd.Next(1, 3000);

                Console.Write("Введите расход бака (л/100 км): ");
                double rashod = Convert.ToDouble(Console.ReadLine());

                while (rashod == 0 || rashod >= max_benzin_bak || rashod >= 10)
                {
                    Console.Write("Введите верное значение расхода: ");
                    rashod = Convert.ToDouble(Console.ReadLine());
                }

                Console.Write("Введите начальный пробег: ");
                double startProbeg = Convert.ToDouble(Console.ReadLine());

                double fullProbeg = startProbeg;

                Console.WriteLine();

                cars[i] = new Avto(carNumber, max_benzin_bak, benzin_bak_volume, distance, rashod, startProbeg, fullProbeg);
            }

            avto.Ezda(cars);
        }
    }
}

