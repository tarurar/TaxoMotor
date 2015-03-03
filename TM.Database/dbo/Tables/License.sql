CREATE TABLE [dbo].[License]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Title] as [RegNumber],
    [RegNumber] NVARCHAR(64) NULL,
	[RegNumberInt] as CAST([RegNumber] as INT) PERSISTED,
    [BlankSeries] NVARCHAR(12) NULL, 
    [BlankNo] NVARCHAR(12) NULL, 
    [OrgName] NVARCHAR(255) NULL, 
    [Ogrn] NVARCHAR(255) NULL, 
    [Inn] NVARCHAR(255) NULL, 
    [Parent] INT NULL, 
	[RootParent] INT NULL, 
    [Status] INT NULL, 
    [Document] XML NULL, 
    [Signature] XML NULL, 
    [TaxiId] INT NULL, 
    [Lfb] NVARCHAR(128) NULL, 
    [JuridicalAddress] NVARCHAR(255) NULL, 
    [PhoneNumber] NVARCHAR(64) NULL, 
    [AddContactData] NVARCHAR(MAX) NULL, 
    [AccountAbbr] NVARCHAR(12) NULL, 
    [TaxiBrand] NVARCHAR(64) NULL, 
    [TaxiModel] NVARCHAR(64) NULL, 
    [TaxiStateNumber] NVARCHAR(24) NULL, 
    [TaxiYear] INT NULL, 
    [OutputDate] DATETIME NULL, 
    [CreationDate] DATETIME NULL, 
    [TillDate] DATETIME NULL, 
    [TillSuspensionDate] DATETIME NULL, 
    [CancellationReason] NVARCHAR(MAX) NULL, 
    [SuspensionReason] NVARCHAR(MAX) NULL, 
    [ChangeReason] NVARCHAR(MAX) NULL, 
    [InvalidReason] NVARCHAR(MAX) NULL,  
	-- краткое наименование перевозчика
	[ShortName] NVARCHAR(32) NULL,
	-- фамилия ИП
	[LastName] NVARCHAR(64) NULL,
	-- имя ИП
	[FirstName] NVARCHAR(64) NULL,
	-- отчетство ИП
	[SecondName] NVARCHAR(64) NULL,
	-- дата ОГРН
	[OgrnDate] DATETIME NULL,
	-- Страна
	[Country] NVARCHAR(64) NULL,
	-- Индекс
	[PostalCode] NVARCHAR(64) NULL,
	-- Область
	[Locality]	NVARCHAR(64) NULL,
	-- Регион
	[Region] NVARCHAR(64) NULL,
	-- Город 	
	[City]	NVARCHAR(64) NULL,
	-- Населенный пункт
	[Town] NVARCHAR(64) NULL,
	-- Улица
	[Street] NVARCHAR(64) NULL,
	-- Дом 
	[House] NVARCHAR(64) NULL,
	-- Корпус
	[Building] NVARCHAR(64) NULL,
	-- Строение
	[Structure] NVARCHAR(64) NULL,
	-- Сооружение
	[Facility] NVARCHAR(64) NULL,
	-- Владение
	[Ownership] NVARCHAR(64) NULL,
	-- Квартира
	[Flat] NVARCHAR(64) NULL,
	-- Факс перевозчика
	[Fax] NVARCHAR(64) NULL,
	-- Электронная почта перевозчика
	[EMail]	NVARCHAR(64) NULL,
	-- цвет ТС
	[TaxiColor] NVARCHAR(64) NULL,
	-- цвет номера ТС
	[TaxiNumberColor] NVARCHAR(64) NULL,
	-- VIN-код транспортного средства
	[TaxiVin] NVARCHAR(64) NULL,
	-- Дата реального внесения изменения записи
	[ChangeDate] DATETIME NULL,
	-- GUID ОДОПМ
	[Guid_OD] NVARCHAR(64) NULL,
	-- Признак передачи данных в ОДОПМ
	[Date_OD] DATETIME NULL,
	-- Признак создания разрешения на портале АСГУФ
	[FromPortal] BIT NULL,
	-- Фирменное наименование перевозчика
	[FirmName] NVARCHAR(128) NULL,
	-- Брэнд перевозчика
	[Brand] NVARCHAR(128) NULL,
	-- Серия и номер свидетельства
	[OgrnNum] NVARCHAR(64) NULL,
	-- Кем выдан ОГРН
	[OgrnName] NVARCHAR(256) NULL,
	-- Адрес органа, осуществившего регистрацию
	[GRAddress] NVARCHAR(256) NULL,
	-- Дата постановки на налоговый учет
	[InnDate] DATETIME NULL,
	-- Кем выдан ИНН
	[InnName] NVARCHAR(256) NULL,
	-- Серия и номер свидетельства
	[InnNum] NVARCHAR(64) NULL,
	-- Адрес перевозчика (фактический адрес)
	[Address_Fact] NVARCHAR(256) NULL,
	-- Страна (фактический адрес)
	[Country_Fact] NVARCHAR(32) NULL,
	-- Индекс (фактический адрес)
	[PostalCode_Fact] NVARCHAR(32) NULL,
	-- Область (фактический адрес)
	[Locality_Fact] NVARCHAR(64) NULL,
	-- Регион (фактический адрес)
	[Region_Fact] NVARCHAR(64) NULL,
	-- Город  (фактический адрес)
	[City_Fact] NVARCHAR(32) NULL,
	-- Населенный пункт (фактический адрес)
	[Town_Fact] NVARCHAR(64) NULL,
	-- Улица (фактический адрес)
	[Street_Fact] NVARCHAR(64) NULL,
	-- Дом  (фактический адрес)
	[House_Fact] NVARCHAR(16) NULL,
	-- Корпус (фактический адрес)
	[Building_Fact] NVARCHAR(16) NULL,
	-- Строение (фактический адрес
	[Structure_Fact] NVARCHAR(16) NULL,
	-- Сооружение (фактический адрес)
	[Facility_Fact] NVARCHAR(16) NULL,
	-- Владение (фактический адрес)
	[Ownership_Fact] NVARCHAR(16) NULL,
	-- Квартира (фактический адрес)
	[Flat_Fact] NVARCHAR(16) NULL,
	-- Наличие ГЛОНАСС/GPS
	[Gps] BIT NULL,
	-- Наличие таксометра
	[Taxometr] BIT NULL,
	-- Дата прохождения последнего ТО к моменту внесения записи в реестр разрешений
	[TODate] DATETIME NULL,
	-- Номер свидетельства о регистрации ТС
	[STSNumber] NVARCHAR(32) NULL,
	-- Дата выдачи свидетельства о регистрации ТС
	[STSDate] DATETIME NULL,
	-- Основания владения ТС
	[OwnType] INT NULL,
	-- Номер документа основания владения ТС
	[OwnNumber] NVARCHAR(32) NULL,
	-- Дата документа основания владения ТС
	[OwnDate] DATETIME NULL,
	-- Признак обращения МО
	[MO] bit NULL,
	-- GUID сообщения из МО
	[GUID_MO] NVARCHAR(64) NULL,
	-- Признак (дата) загрузки в систему МО
	[DATE_MO] DATETIME NULL
    CONSTRAINT [FK_License_ToLicense_Parent] FOREIGN KEY ([Parent]) REFERENCES [dbo].[License]([Id]),
	CONSTRAINT [FK_License_ToLicense_RootParent] FOREIGN KEY ([RootParent]) REFERENCES [dbo].[License]([Id]) 
)
