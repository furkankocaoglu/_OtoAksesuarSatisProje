CREATE DATABASE OtoAksesuarsatis_db;
GO


USE OtoAksesuarsatis_db;
GO


CREATE TABLE Kategoriler (
    KategoriID INT IDENTITY(1,1),
    KategoriAdi NVARCHAR(100) NOT NULL,
    Durum BIT DEFAULT 1,
    Silinmis BIT DEFAULT 0,
    CONSTRAINT pk_Kategoriler PRIMARY KEY (KategoriID)  
);
GO


CREATE TABLE Markalar (
    MarkaID INT IDENTITY(1,1),
    MarkaAdi NVARCHAR(100) NOT NULL,
    Durum BIT DEFAULT 1,
    Silinmis BIT DEFAULT 0,
    CONSTRAINT pk_Markalar PRIMARY KEY (MarkaID)  
);
GO


CREATE TABLE Urunler (
    UrunID INT IDENTITY(1,1),
    UrunAdi NVARCHAR(100) NOT NULL,
    MarkaID INT NOT NULL,  -- FK: Markalar(MarkaID)
    KategoriID INT NOT NULL,  -- FK: Kategoriler(KategoriID)
    StokMiktari INT,
    Aciklama NVARCHAR(500),
    ResimYolu NVARCHAR(500),
    BronzFiyat DECIMAL(10,2),
    SilverFiyat DECIMAL(10,2),
    GoldFiyat DECIMAL(10,2),
    EklenmeTarihi DATETIME DEFAULT GETDATE(),
    AktifMi BIT DEFAULT 1,
    Silinmis BIT DEFAULT 0,
    CONSTRAINT pk_Urunler PRIMARY KEY (UrunID),  
    CONSTRAINT fk_Urunler_Kategoriler FOREIGN KEY (KategoriID) REFERENCES Kategoriler(KategoriID),  
    CONSTRAINT fk_Urunler_Markalar FOREIGN KEY (MarkaID) REFERENCES Markalar(MarkaID)  
);
GO


CREATE TABLE AltBayiler (
    AltBayiID INT IDENTITY(1,1),
    BayiAdi NVARCHAR(100),
    Segment NVARCHAR(10) CHECK (Segment IN ('Bronz', 'Silver', 'Gold')),  
    SonXmlGuncellemeTarihi DATETIME DEFAULT GETDATE(),
    CONSTRAINT pk_AltBayiler PRIMARY KEY (AltBayiID)  
);
GO


CREATE TABLE AnaBayi (
    AnaBayiID INT IDENTITY(1,1),
    Isim NVARCHAR(50),
    Soyisim NVARCHAR(50),
    KullaniciAdi NVARCHAR(50),
    Mail NVARCHAR(120),
    Sifre NVARCHAR(20),
    Durum BIT DEFAULT 1,
    Silinmis BIT DEFAULT 0,
    CONSTRAINT pk_AnaBayi PRIMARY KEY (AnaBayiID)  
);
GO


INSERT INTO AnaBayi (Isim, Soyisim, KullaniciAdi, Mail, Sifre, Durum, Silinmis)
VALUES ('Furkan', 'Kocaoðlu', 'admin', 'furkan.kocaoglu@gmail.com', '1234', 1, 0);
GO