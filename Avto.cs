
    namespace МашинкиСупер
    {
        internal class Avto
        {
            protected string carNumber;
            protected double max_benzin_bak;
            protected double benzin_bak_volume;           //Текущий объем бака
            protected double rashod;                      //Расход 100 км
            protected double fullTime = 0;
            protected double distance;
            protected double proehali = 0;
            protected double canDrive;                    //Можно проехать при текущем объёме бака
            protected double coordinatesDistance = 0;
            protected double startProbeg;
            protected double fullProbeg;
            protected double speed;
            private double benzin_bak_OSTATOK;
            public Avto[] cars = new Avto[3];

            public Avto(string carNumber, double max_benzin_bak, double benzin_bak_volume, double distance, double rashod, double startProbeg, double fullProbeg)
            {
                this.carNumber = carNumber;
                this.max_benzin_bak = max_benzin_bak;
                this.benzin_bak_volume = benzin_bak_volume;
                this.distance = distance;
                this.rashod = rashod;
                this.startProbeg = startProbeg;
                this.fullProbeg = fullProbeg;
            }

            //Разница между Drive и Ezda: Ezda(возможность выбора машины), Drive(Чтобы продолжить путь для этой же машинки
            public void Ezda(Avto[] cars)
            {
                Random rnd = new Random();

                while (true)
                {
                    CarChioice(cars);
                    int choice = Convert.ToInt32(Console.ReadLine());

                    if (choice == 4) //Условие на выход из программы
                    {
                        Console.Clear();
                        Environment.Exit(0);
                    }
                    else
                    {
                        int i = choice - 1;

                        Console.Clear();

                        CarsOutput(cars, i); //Вывод информации о машинке
                        Console.WriteLine();

                        if (cars[i].startProbeg < cars[i].fullProbeg && cars[i].proehali < cars[i].distance)  //Машинка едет, но не закончила свой путь
                        {
                            CarsOutput(cars, i);
                            Console.WriteLine($"Вы проехали {cars[i].proehali}/{cars[i].distance}. Желатете продолжить ехать?\n1. Да\n2. Нет");
                            int user_choice = Convert.ToInt32(Console.ReadLine());

                            switch (user_choice)
                            {
                                case 1:
                                    zapravka(cars, i);  //Заправка
                                    drive(cars, i);
                                    break;
                                case 2:
                                    Console.WriteLine("Вы отказались ехать дальше");
                                    Ezda(cars); //В случае отказа можно выбрать другую машину, перейдя в езду
                                    break;
                            }
                        }

                        else if (cars[i].proehali == cars[i].distance) //Машина проехала свой путь. Стоит ли его продолжать?
                        {
                            Console.WriteLine("Вы проехали необходимое расстояние. Желаете ехать дальше?\n1. Да\n2. Нет");
                            int user_choice = Convert.ToInt32(Console.ReadLine());

                            switch (user_choice)
                            {
                                case 1:
                                    cars[i].distance = rnd.Next(1, 3000); // Новое расстояние
                                    cars[i].proehali = 0;       //Обнуляем пройденный путь, чтобы информация обновлялась

                                    Console.WriteLine($"Необходимо проехать: {cars[i].distance} км");
                                    drive(cars, i);  //Повторный запуск 
                                    break;

                                case 2:
                                    Ezda(cars);  //Запуск С ВЫБОРОМ НОВОЙ МАШИНЫ
                                    break;
                            }
                        }

                        else
                        {
                            drive(cars, i); //Если машина впервые едет
                        }
                    }
                }

                void drive(Avto[] cars, int i)  //МЕТОД DRIVE ВНУТРИ EZDA
                {
                    Random rnd = new Random();

                    while (cars[i].distance > cars[i].proehali)  //Пока расстояние больше чем проехали 
                    {
                        cars[i].speed = rnd.Next(60, 90);  //Случайная скорость

                        while (cars[i].benzin_bak_volume == 0)  //Проверка на дурака
                        {
                            Console.WriteLine("Ваш бак пуст. Заправляемся");
                            cars[i].benzin_bak_volume = zapravka(cars, i);

                            Console.WriteLine();
                        }

                        Console.WriteLine($"Необходимо проехать: {cars[i].distance - cars[i].proehali} км");

                        cars[i].canDrive = (cars[i].benzin_bak_volume / cars[i].rashod) * 100;  //Сколько можно проехать 

                        Console.WriteLine($"При объеме бака {cars[i].benzin_bak_volume} л вы можете проехать {Math.Round(cars[i].canDrive, 3)} км");

                        Console.Write("Нажмите любую клавишу, чтобы ехать: ");
                        Console.ReadKey();
                        Console.Clear();

                        speedUpDown(cars, i);  //Метод для изменения скорости во время езды

                        if (cars[i].distance - (cars[i].proehali + cars[i].canDrive) <= 0) //Если бензина есть больше для необходимого расстояния
                        {
                            cars[i].canDrive = cars[i].distance - cars[i].proehali; //Для обновления пробега через CanDrive

                            //формула
                            cars[i].benzin_bak_volume = (cars[i].canDrive * cars[i].rashod) / 100; //остаток бензина после поездки

                            //cars[i].benzin_bak_OSTATOK = cars[i].
                            cars[i].proehali = cars[i].distance;  //Приравниваем пройденное расстояние ко всему расстоянию            
                                                                  //cars[i].benzin_bak_volume = 0;  //Обнуляем бак

                            Probeg(cars, i);
                            Console.WriteLine($"\nВы проехали {Math.Round(cars[i].proehali, 3)}/{Math.Round(cars[i].distance, 3)} км");
                            Console.WriteLine($"Новое значение пробега: {cars[i].fullProbeg} км\n");
                        }

                        else  //Если бензина есть меньше для необходимого расстояния
                        {
                            cars[i].proehali += cars[i].canDrive;  //Обновляем пройденное расстояние
                            cars[i].benzin_bak_volume = 0;   //Обнуляем бак

                            Probeg(cars, i);
                            Console.WriteLine($"\nВы проехали {Math.Round(cars[i].proehali, 3)}/{Math.Round(cars[i].distance, 3)} км");
                            Console.WriteLine($"Новое значение пробега: {Math.Round(cars[i].fullProbeg, 3)} км\n");

                            CarsOutput(cars, i);
                            Console.WriteLine("Ваш бак пуст. Желаете заправиться?\n1. Да\n2. Нет");
                            int choice = Convert.ToInt32(Console.ReadLine());

                            switch (choice)
                            {
                                case 1:
                                    cars[i].benzin_bak_volume = zapravka(cars, i); //Заправились
                                    CarsOutput(cars, i);
                                    break;
                                case 2:
                                    Console.WriteLine("Вы отказались заправляться. Выводим информацию о машине: ");
                                    CarsOutput(cars, i);

                                    Console.Write("\nНажмите любую клавишу, чтобы выбрать машину: ");
                                    Console.ReadKey();
                                    Console.Clear();

                                    Ezda(cars);
                                    break;
                            }
                        }

                        time(cars, i);
                    }

                    cars[i].startProbeg = cars[i].fullProbeg; //Чтобы при следующей поездке начальным пробегом был уже пройденный
                    Console.WriteLine("\nВы проехали необходимое расстояние! Желаете ехать дальше?\n1. Да\n2. Нет");
                    int user_choice = Convert.ToInt32(Console.ReadLine());

                    switch (user_choice)
                    {
                        case 1:
                            Console.Clear();

                            cars[i].distance = rnd.Next(1, 3000);
                            cars[i].proehali = 0;

                            Console.WriteLine("Ваш бак пуст. Перед тем как ехать, нужно заправиться.");
                            cars[i].benzin_bak_volume = zapravka(cars, i);

                            Console.WriteLine($"Теперь необходимо нeобходимо проехать: {cars[i].distance} км\n");
                            drive(cars, i);
                            break;

                        case 2:
                            Console.Clear();
                            Ezda(cars);  //Запускаем езду с выбором машины
                            break;
                    }
                }
            }

            protected void Probeg(Avto[] cars, int i)
            {
                cars[i].fullProbeg += Math.Round(cars[i].canDrive, 3);
            }

            protected void time(Avto[] cars, int i)  //Время в пути
            {
                cars[i].fullTime += cars[i].canDrive / cars[i].speed;
            }

            protected void speedUpDown(Avto[] cars, int i)  //Изменение скорости
            {
                Console.WriteLine($"Вы движетесь со скоростью {cars[i].speed} км/ч");
                Console.WriteLine($"Выберите действие: \n1. Ускориться\n2. Притормозить\n3. Двигаться с текущей скоростью");

                int user_choice = Convert.ToInt32(Console.ReadLine());

                CarsOutput(cars, i);


                switch (user_choice)
                {
                    case 1:
                        Console.Write($"Скорость {cars[i].speed} ->");
                        cars[i].speed += 20;
                        Console.Write($" {cars[i].speed} км/ч");
                        break;
                    case 2:
                        Console.Write($"Скорость {cars[i].speed} ->");
                        cars[i].speed -= 20;
                        Console.Write($" {cars[i].speed} км/ч");
                        break;
                    case 3:
                        Console.WriteLine($"Скорость {cars[i].speed} км/ч");
                        break;
                }

                Console.WriteLine();
            }

            protected double zapravka(Avto[] cars, int i)
            {
                Console.Write($"Сколько литров залить? (максимум {cars[i].max_benzin_bak}): ");
                double add = Convert.ToDouble(Console.ReadLine());

                while (add <= 0 || add > cars[i].max_benzin_bak) //Проверка на дурака
                {
                    Console.Write("Введите правильное значение: ");
                    add = Convert.ToDouble(Console.ReadLine());
                }

                cars[i].benzin_bak_volume = add;
                return cars[i].benzin_bak_volume;
            }

            protected void Coordinates(double CanDrive) //Перемещение по координатам
            {
                coordinatesDistance += CanDrive * 3;  //1 км пути = 3 пунктам по оси X
            }

            protected void DriveTime(double speed, double CanDrive, ref double fullTime) //Время в пути
            {
                fullTime = CanDrive / speed;
            }

            protected void CarChioice(Avto[] cars) //Вывод машин для выбора
            {
                Console.WriteLine("Выберите машину: ");
                for (int i = 0; i < cars.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. Машина {cars[i].carNumber}");  //Обращение к машине по номеру
                }
                Console.WriteLine("4. Выйти");
            }
            public void CarsOutput(Avto[] cars, int i)  //Вывод информации о всех машинах
            {
                Console.WriteLine($"\nНомер машины {cars[i].carNumber}");
                Console.WriteLine($"Максимальный объем бака: {cars[i].max_benzin_bak}л ");
                Console.WriteLine($"Текущий объем бака: {cars[i].benzin_bak_volume} л");
                Console.WriteLine($"Пробег: {cars[i].fullProbeg} км");
                Console.WriteLine($"Нужно проехать: {cars[i].distance} км");
                Console.WriteLine($"Времени в пути: {Math.Round(cars[i].fullTime, 3)} ч\n");
            }

        }
    }