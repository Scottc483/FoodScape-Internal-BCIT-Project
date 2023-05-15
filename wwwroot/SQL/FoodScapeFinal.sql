IF OBJECT_ID('TicketTypeEvent')
    IS NOT NULL DROP TABLE TicketTypeEvent;
IF OBJECT_ID('EventVendor')
    IS NOT NULL DROP TABLE EventVendor;
IF OBJECT_ID('Ticket')
    IS NOT NULL DROP TABLE Ticket;
IF OBJECT_ID('Review')
    IS NOT NULL DROP TABLE Review;
IF OBJECT_ID('IPN')
    IS NOT NULL DROP TABLE IPN;
IF OBJECT_ID('FsUser')
    IS NOT NULL DROP TABLE [FsUser];
IF OBJECT_ID('TicketType')
    IS NOT NULL DROP TABLE TicketType;
IF OBJECT_ID('Vendor')
    IS NOT NULL DROP TABLE Vendor;
IF OBJECT_ID('Event')
    IS NOT NULL DROP TABLE [Event];
GO
CREATE TABLE [FsUser] (
    userID INT PRIMARY KEY IDENTITY,
    firstName VARCHAR(30),
    lastName VARCHAR(30),
    email VARCHAR(50)
    
);
CREATE TABLE Review (
    reviewID INT PRIMARY KEY IDENTITY,
    userID INT,
    review VARCHAR(500),
    rating INT,
    createdDate DATETIME,
    updatedDate DATETIME,
    FOREIGN KEY(userID) REFERENCES [FsUser](userID)
);
CREATE TABLE Vendor (
    vendorID INT PRIMARY KEY IDENTITY,
    [name] VARCHAR(50),
    [description] VARCHAR(500),
    startDate DATE,
    endDate DATE,
    vendorType VARCHAR(25),
    [location] VARCHAR(100),
    imageURL VARCHAR(200) NULL
);
CREATE TABLE [Event] (
    eventID INT PRIMARY KEY IDENTITY,
    [address] VARCHAR(255),
    capacity INT,
    [name] VARCHAR(50),
    ageRestriction INT NULL,
    imageURL VARCHAR(200) NULL,
    startDate DATETIME NULL,
    endDate DATETIME NULL,
    [description] VARCHAR(200) NULL
);
CREATE TABLE TicketType (
    ticketTypeID INT PRIMARY KEY IDENTITY,
    ticketType VARCHAR(50),
    price DECIMAL,
    discount DECIMAL,
    [day] INT,
    eventID INT,
    FOREIGN KEY(eventID) REFERENCES [Event](eventID)
);
CREATE TABLE Ticket (
    ticketID INT PRIMARY KEY IDENTITY,
    userID INT,
    ticketTypeID INT,
    FOREIGN KEY(userID) REFERENCES [FsUser](userID),
    FOREIGN KEY(ticketTypeID) REFERENCES TicketType(ticketTypeID)
);
CREATE TABLE EventVendor (
    eventVendorID INT PRIMARY KEY IDENTITY,
    eventID INT,
    vendorID INT,
    FOREIGN KEY(eventID) REFERENCES [Event](eventID),
    FOREIGN KEY(vendorID) REFERENCES Vendor(vendorID)
);
CREATE TABLE IPN (
    paymentID VARCHAR(255) PRIMARY KEY,
    userID INT,
    amount VARCHAR(30),
    currency VARCHAR(30),
    paymentMethod VARCHAR(30),
    FOREIGN KEY(userID) REFERENCES [FsUser](userID),
);
/* This is needed to INSERT into EVENTS */
SET IDENTITY_INSERT EVENT ON
INSERT INTO [Event] (eventID, address, capacity, name, ageRestriction, imageURL, startDate, endDate, description)
VALUES (1, '2901 E Hastings St', 250, 'After Dark', 19,'/images/AfterDark.png', '2023-08-12', '2023-08-12', 'Music Festival');
INSERT INTO [Event] (eventID, address, capacity, name, ageRestriction, imageURL, startDate, endDate, description)
VALUES (2, '2901 E Hastings St', 50, 'Eat Or Defeat', 16,'/images/EatOrDefeat.png', '2023-08-12', '2023-08-12', 'Eating Contest');
INSERT INTO [Event] (eventID, address, capacity, name, ageRestriction, imageURL, startDate, endDate, description)
VALUES (3, '2901 E Hastings St', 500, 'Flavors In Frames', 16,'/images/FlavorsInFrames.png', '2023-08-11', '2023-08-13', 'Food Photography Contest');
INSERT INTO [Event] (eventID, address, capacity, name, ageRestriction, imageURL, startDate, endDate, description)
VALUES (4, '2901 E Hastings St', 75, 'Food Truck ThrowDown', 4,'/images/FoodTruckThrowDown.png', '2023-08-13', '2023-08-13', 'Food Truck Cook-off');
INSERT INTO [Event] (eventID, address, capacity, name, ageRestriction, imageURL, startDate, endDate, description)
VALUES (5, '2901 E Hastings St', 75, 'Batch Boba', 4,'/images/FoodScapeBoba.png', '2023-08-13', '2023-08-13', 'Boba Tasting!');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Black Ox Steak', 'Satisfy your carnivorous cravings with Black Ox Steak - where juicy cuts of meat meet bold flavors on the go!', '2022-02-17', '2022-02-18', 'Main Park', 'Food', '/images/BlackOxSteak.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Javon`s Macaron`s', 'Indulge in a delicate and delectable treat with Javon`s Macarons - where every bite is a burst of flavor and elegance!', '2022-02-17', '2022-02-18', 'Food court', 'Food', '/images/JavonsMacarons.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('The Donut Co.', 'Experience the ultimate in sweet satisfaction with The Donut Co. - where every bite is a freshly-made delight that`s worth the trip!', '2022-02-17', '2022-02-18', 'Main Park', 'Food', '/images/TheDonutCo.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Reynor Steakhouse', 'Sink your teeth into a sizzling steak at Reynor Steakhouse - where quality meats meet bold flavors that will leave you craving more!', '2022-02-17', '2022-02-18', 'Food court', 'Food', '/images/ReynorSteakhouse.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Tias Taqueria', 'Get your Mexican fix on the go with Tia`s Taqueria - where every taco, burrito, and quesadilla is a flavor-packed adventure that`s sure to satisfy your hunger!', '2022-02-17', '2022-02-18', 'Main Park', 'Food', '/images/TiasTaqueria.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Pizza Veloce', 'Experience the speed of Italian cuisine with Pizza Veloce - where fresh ingredients and artisanal techniques come together to create the perfect slice on the go!', '2022-02-17', '2022-02-18', 'Main Park', 'Food', '/images/PizzaVeloce.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Batch Boba', 'Batch Boba is a boba tasting event where you can sample a variety of unique and delicious boba drinks from various vendors in one location. Join us for a refreshing experience filled with great company and music.', '2022-02-17', '2022-02-18', 'Main Park', 'Beverages', '/images/FoodScapeBoba.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Mad Peter Ales', 'Mad Peter Ales is a beer tasting event where you can try a range of distinct brews from local and national breweries. Come join us for a fun-filled and refreshing experience!', '2022-02-17', '2022-02-18', 'Main Park', 'Beverages', '/images/MadPeterAles.png');
INSERT INTO Vendor (name, description, startDate, endDate, location, vendorType, imageURL) VALUES ('Hatchet Hurlers', 'Hatchet Hurlers is a thrilling event that lets you try your hand at axe throwing. Our experienced coaches will guide you through proper technique and safety protocols for a fun and unique experience. Join us for a memorable activity that`s perfect for friends or team-building events.', '2022-02-17', '2022-02-18', 'Main Park', 'Activity', '/images/HatchetHurlers.png');

INSERT INTO TicketType (ticketType, price, discount, [day], eventID) VALUES ('GA', 20.00, 0, 1, 3);
INSERT INTO TicketType (ticketType, price, discount, [day], eventID) VALUES ('VIP', 50.00, 0, 1, 3);
INSERT INTO TicketType (ticketType, price, discount, [day], eventID) VALUES ('Pass', 45.00, 25, 1, 3);
INSERT INTO TicketType (ticketType, price, discount, [day], eventID) VALUES ('GA', 30.00, 0, 2, 2);
INSERT INTO TicketType (ticketType, price, discount, [day], eventID) VALUES ('VIP', 50.00, 0, 2, 1);
INSERT INTO TicketType (ticketType, price, discount, [day], eventID) VALUES ('GA', 35.00, 0, 3, 4);
INSERT INTO TicketType (ticketType, price, discount, [day], eventID) VALUES ('VIP', 50.00, 0, 3, 4);
INSERT INTO EventVendor (eventID, vendorID) VALUES (4, 1);
INSERT INTO EventVendor (eventID, vendorID) VALUES (3, 2);
INSERT INTO EventVendor (eventID, vendorID) VALUES (2, 3);
INSERT INTO EventVendor (eventID, vendorID) VALUES (4, 3);
INSERT INTO EventVendor (eventID, vendorID) VALUES (1, 4);
INSERT INTO EventVendor (eventID, vendorID) VALUES (2, 4);
INSERT INTO EventVendor (eventID, vendorID) VALUES (3, 5);
INSERT INTO EventVendor (eventID, vendorID) VALUES (4, 5);
INSERT INTO EventVendor (eventID, vendorID) VALUES (1, 6);
INSERT INTO EventVendor (eventID, vendorID) VALUES (3, 6);

INSERT INTO EventVendor (eventID, vendorID) VALUES (2, 7);
INSERT INTO EventVendor (eventID, vendorID) VALUES (1, 8);
INSERT INTO EventVendor (eventID, vendorID) VALUES (4, 9);