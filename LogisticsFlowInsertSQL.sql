-- Очистка таблиц (если нужно)
-- DELETE FROM Orders;
-- DELETE FROM Clients;
-- DELETE FROM Employees;
-- DELETE FROM Drivers;
-- DELETE FROM Vehicles;
-- DELETE FROM Cargos;
-- DELETE FROM Routes;

-- Вставка в Routes (5 записей)
INSERT INTO Routes (Id, Origin, Destination, Distance, EstimatedTime)
VALUES
    (NEWID(), 'Москва', 'Санкт-Петербург', 710.0, '08:00:00'),
    (NEWID(), 'Москва', 'Казань', 820.0, '12:00:00'),
    (NEWID(), 'Москва', 'Нижний Новгород', 440.0, '06:30:00'),
    (NEWID(), 'Санкт-Петербург', 'Москва', 710.0, '08:00:00'),
    (NEWID(), 'Казань', 'Москва', 820.0, '12:00:00');

-- Вставка в Vehicles (5 записей)
INSERT INTO Vehicles (Id, Mark, Model, Number, LoadCapacity)
VALUES
    (NEWID(), 'Volvo', 'FH16', 'А123ВС77', 20000.00),
    (NEWID(), 'MAN', 'TGX', 'В456ОР77', 18000.00),
    (NEWID(), 'Mercedes', 'Actros', 'С789ТХ77', 22000.00),
    (NEWID(), 'Scania', 'R500', 'Е321КА77', 19000.00),
    (NEWID(), 'DAF', 'XF', 'Н654СВ77', 21000.00);

-- Вставка в Cargos (5 записей)
INSERT INTO Cargos (Id, Name, Description, Weight, Volume)
VALUES
    (NEWID(), 'Электроника', 'Компьютерная техника и комплектующие', 1500.00, 8.5),
    (NEWID(), 'Одежда', 'Готовая текстильная продукция', 800.00, 15.2),
    (NEWID(), 'Мебель', 'Офисная мебель', 2500.00, 25.0),
    (NEWID(), 'Продукты', 'Продовольственные товары', 1200.00, 12.8),
    (NEWID(), 'Строительные материалы', 'Сухие смеси и инструменты', 3000.00, 18.5);

-- Вставка в Clients (5 записей)
INSERT INTO Clients (Id, Name, Email, PhoneNumber, Address)
VALUES
    (NEWID(), 'ООО "Ромашка"', 'romashka@mail.ru', '+7 (495) 123-45-67', 'г. Москва, ул. Ленина, д. 1'),
    (NEWID(), 'ИП Иванов И.И.', 'ivanov@mail.ru', '+7 (495) 234-56-78', 'г. Москва, ул. Пушкина, д. 25'),
    (NEWID(), 'ООО "Вектор"', 'vector@mail.ru', '+7 (812) 345-67-89', 'г. Санкт-Петербург, Невский пр-т, д. 100'),
    (NEWID(), 'ЗАО "ТехноПром"', 'technoprom@mail.ru', '+7 (843) 456-78-90', 'г. Казань, ул. Баумана, д. 50'),
    (NEWID(), 'ООО "СтройГрад"', 'stroigrad@mail.ru', '+7 (831) 567-89-01', 'г. Нижний Новгород, ул. Горького, д. 75');

-- Вставка в Employees (5 записей)
INSERT INTO Employees (Id, Name, Password, Email, PhoneNumber)
VALUES
    (NEWID(), 'Петров Алексей', 'password123', 'petrov@company.ru', '+7 (495) 111-11-11'),
    (NEWID(), 'Сидорова Мария', 'password123', 'sidorova@company.ru', '+7 (495) 222-22-22'),
    (NEWID(), 'Козлов Дмитрий', 'password123', 'kozlov@company.ru', '+7 (495) 333-33-33'),
    (NEWID(), 'Николаева Анна', 'password123', 'nikolaeva@company.ru', '+7 (495) 444-44-44'),
    (NEWID(), 'Васильев Сергей', 'password123', 'vasiliev@company.ru', '+7 (495) 555-55-55');

-- Вставка в Drivers (5 записей)
DECLARE @VehicleId1 UNIQUEIDENTIFIER = (SELECT Id FROM Vehicles WHERE Number = 'А123ВС77');
DECLARE @VehicleId2 UNIQUEIDENTIFIER = (SELECT Id FROM Vehicles WHERE Number = 'В456ОР77');
DECLARE @VehicleId3 UNIQUEIDENTIFIER = (SELECT Id FROM Vehicles WHERE Number = 'С789ТХ77');
DECLARE @VehicleId4 UNIQUEIDENTIFIER = (SELECT Id FROM Vehicles WHERE Number = 'Е321КА77');
DECLARE @VehicleId5 UNIQUEIDENTIFIER = (SELECT Id FROM Vehicles WHERE Number = 'Н654СВ77');

INSERT INTO Drivers (Id, Name, Phone, LicenseNumber, VehicleID)
VALUES
    (NEWID(), 'Смирнов Александр', '+7 (916) 111-11-11', 'AB123456', @VehicleId1),
    (NEWID(), 'Попов Игорь', '+7 (916) 222-22-22', 'CD654321', @VehicleId2),
    (NEWID(), 'Лебедев Михаил', '+7 (916) 333-33-33', 'EF789012', @VehicleId3),
    (NEWID(), 'Новикова Елена', '+7 (916) 444-44-44', 'GH345678', @VehicleId4),
    (NEWID(), 'Федоров Павел', '+7 (916) 555-55-55', 'IJ901234', @VehicleId5);

-- Вставка в Orders (10 записей)
DECLARE @CargoId1 UNIQUEIDENTIFIER = (SELECT Id FROM Cargos WHERE Name = 'Электроника');
DECLARE @CargoId2 UNIQUEIDENTIFIER = (SELECT Id FROM Cargos WHERE Name = 'Одежда');
DECLARE @CargoId3 UNIQUEIDENTIFIER = (SELECT Id FROM Cargos WHERE Name = 'Мебель');
DECLARE @CargoId4 UNIQUEIDENTIFIER = (SELECT Id FROM Cargos WHERE Name = 'Продукты');
DECLARE @CargoId5 UNIQUEIDENTIFIER = (SELECT Id FROM Cargos WHERE Name = 'Строительные материалы');

DECLARE @ClientId1 UNIQUEIDENTIFIER = (SELECT Id FROM Clients WHERE Name = 'ООО "Ромашка"');
DECLARE @ClientId2 UNIQUEIDENTIFIER = (SELECT Id FROM Clients WHERE Name = 'ИП Иванов И.И.');
DECLARE @ClientId3 UNIQUEIDENTIFIER = (SELECT Id FROM Clients WHERE Name = 'ООО "Вектор"');
DECLARE @ClientId4 UNIQUEIDENTIFIER = (SELECT Id FROM Clients WHERE Name = 'ЗАО "ТехноПром"');
DECLARE @ClientId5 UNIQUEIDENTIFIER = (SELECT Id FROM Clients WHERE Name = 'ООО "СтройГрад"');

DECLARE @EmployeeId1 UNIQUEIDENTIFIER = (SELECT Id FROM Employees WHERE Name = 'Петров Алексей');
DECLARE @EmployeeId2 UNIQUEIDENTIFIER = (SELECT Id FROM Employees WHERE Name = 'Сидорова Мария');
DECLARE @EmployeeId3 UNIQUEIDENTIFIER = (SELECT Id FROM Employees WHERE Name = 'Козлов Дмитрий');
DECLARE @EmployeeId4 UNIQUEIDENTIFIER = (SELECT Id FROM Employees WHERE Name = 'Николаева Анна');
DECLARE @EmployeeId5 UNIQUEIDENTIFIER = (SELECT Id FROM Employees WHERE Name = 'Васильев Сергей');

DECLARE @DriverId1 UNIQUEIDENTIFIER = (SELECT Id FROM Drivers WHERE Name = 'Смирнов Александр');
DECLARE @DriverId2 UNIQUEIDENTIFIER = (SELECT Id FROM Drivers WHERE Name = 'Попов Игорь');
DECLARE @DriverId3 UNIQUEIDENTIFIER = (SELECT Id FROM Drivers WHERE Name = 'Лебедев Михаил');
DECLARE @DriverId4 UNIQUEIDENTIFIER = (SELECT Id FROM Drivers WHERE Name = 'Новикова Елена');
DECLARE @DriverId5 UNIQUEIDENTIFIER = (SELECT Id FROM Drivers WHERE Name = 'Федоров Павел');

DECLARE @RouteId1 UNIQUEIDENTIFIER = (SELECT Id FROM Routes WHERE Origin = 'Москва' AND Destination = 'Санкт-Петербург');
DECLARE @RouteId2 UNIQUEIDENTIFIER = (SELECT Id FROM Routes WHERE Origin = 'Москва' AND Destination = 'Казань');
DECLARE @RouteId3 UNIQUEIDENTIFIER = (SELECT Id FROM Routes WHERE Origin = 'Москва' AND Destination = 'Нижний Новгород');
DECLARE @RouteId4 UNIQUEIDENTIFIER = (SELECT Id FROM Routes WHERE Origin = 'Санкт-Петербург' AND Destination = 'Москва');
DECLARE @RouteId5 UNIQUEIDENTIFIER = (SELECT Id FROM Routes WHERE Origin = 'Казань' AND Destination = 'Москва');

INSERT INTO Orders (Id, OrderDate, DesiredDeliveryDate, Status, Price, CargoId, EmployeeId, ClientId, DriverId, RouteId)
VALUES
    (NEWID(), DATEADD(day, -10, GETDATE()), DATEADD(day, -5, GETDATE()), 'Доставлен', 25000.00, @CargoId1, @EmployeeId1, @ClientId1, @DriverId1, @RouteId1),
    (NEWID(), DATEADD(day, -8, GETDATE()), DATEADD(day, -3, GETDATE()), 'Доставлен', 18000.00, @CargoId2, @EmployeeId2, @ClientId2, @DriverId2, @RouteId2),
    (NEWID(), DATEADD(day, -6, GETDATE()), DATEADD(day, -1, GETDATE()), 'В пути', 32000.00, @CargoId3, @EmployeeId3, @ClientId3, @DriverId3, @RouteId3),
    (NEWID(), DATEADD(day, -4, GETDATE()), DATEADD(day, 2, GETDATE()), 'В пути', 15000.00, @CargoId4, @EmployeeId4, @ClientId4, @DriverId4, @RouteId4),
    (NEWID(), DATEADD(day, -2, GETDATE()), DATEADD(day, 4, GETDATE()), 'Обработка', 28000.00, @CargoId5, @EmployeeId5, @ClientId5, @DriverId5, @RouteId5),
    (NEWID(), DATEADD(day, -1, GETDATE()), DATEADD(day, 5, GETDATE()), 'Обработка', 22000.00, @CargoId1, @EmployeeId1, @ClientId2, @DriverId1, @RouteId1),
    (NEWID(), GETDATE(), DATEADD(day, 6, GETDATE()), 'Принят', 19000.00, @CargoId2, @EmployeeId2, @ClientId3, @DriverId2, @RouteId2),
    (NEWID(), GETDATE(), DATEADD(day, 7, GETDATE()), 'Принят', 35000.00, @CargoId3, @EmployeeId3, @ClientId4, @DriverId3, @RouteId3),
    (NEWID(), GETDATE(), DATEADD(day, 8, GETDATE()), 'Принят', 12000.00, @CargoId4, @EmployeeId4, @ClientId5, @DriverId4, @RouteId4),
    (NEWID(), GETDATE(), DATEADD(day, 9, GETDATE()), 'Принят', 26000.00, @CargoId5, @EmployeeId5, @ClientId1, @DriverId5, @RouteId5);

PRINT 'Данные успешно добавлены в базу данных!';