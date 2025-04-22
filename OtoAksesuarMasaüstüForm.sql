CREATE DATABASE OtoAksesuarsatis_db
GO

USE OtoAksesuarsatis_db
GO

CREATE TABLE Kategoriler (
    KategoriID INT IDENTITY(1,1),
    KategoriAdi NVARCHAR(100) NOT NULL,
    Durum BIT DEFAULT 1,
    Silinmis BIT DEFAULT 0,
    CONSTRAINT pk_Kategoriler PRIMARY KEY (KategoriID)
)
GO
CREATE TABLE Urunler (
    UrunID INT IDENTITY(1,1),
    UrunAdi NVARCHAR(100) NOT NULL,
    Marka NVARCHAR(100),
    KategoriID INT,
    Fiyat DECIMAL(10,2) NOT NULL,
    StokMiktari INT,
    Aciklama NVARCHAR(MAX),
    ResimYolu NVARCHAR(255),
    EklenmeTarihi DATETIME DEFAULT GETDATE(),
    AktifMi BIT DEFAULT 1,
    Silinmis BIT DEFAULT 0,
    CONSTRAINT pk_Urunler PRIMARY KEY (UrunID),
    CONSTRAINT fk_Urunler_Kategoriler FOREIGN KEY (KategoriID) REFERENCES Kategoriler(KategoriID)
)
GO
CREATE TABLE BayiTipleri (
    BayiTipiID INT IDENTITY(1,1),
    BayiTipiAdi NVARCHAR(50) NOT NULL,
    CONSTRAINT pk_BayiTipleri PRIMARY KEY (BayiTipiID)
)
GO
CREATE TABLE AltBayiler (
    AltBayiID INT IDENTITY(1,1),
    BayiAdi NVARCHAR(100),
    BayiTipiID INT,
    CONSTRAINT pk_AltBayiler PRIMARY KEY (AltBayiID),
    CONSTRAINT fk_AltBayiler_BayiTipi FOREIGN KEY (BayiTipiID) REFERENCES BayiTipleri(BayiTipiID)
)
GO
CREATE TABLE BayiFiyatlari (
    FiyatID INT IDENTITY(1,1),
    UrunID INT,
    BayiTipiID INT,
    Fiyat DECIMAL(10,2),
    XmlOlusturmaTarihi DATETIME DEFAULT GETDATE(),
    CONSTRAINT pk_BayiFiyatlari PRIMARY KEY (FiyatID),
    CONSTRAINT fk_BayiFiyatlari_Urun FOREIGN KEY (UrunID) REFERENCES Urunler(UrunID),
    CONSTRAINT fk_BayiFiyatlari_Tip FOREIGN KEY (BayiTipiID) REFERENCES BayiTipleri(BayiTipiID)
)
GO
CREATE TABLE AltBayiXmlGuncelleme (
    AltBayiID INT,
    SonXmlGuncellemeTarihi DATETIME DEFAULT GETDATE(),
    CONSTRAINT pk_AltBayiXmlGuncelleme PRIMARY KEY (AltBayiID),
    CONSTRAINT fk_Xml_AltBayi FOREIGN KEY (AltBayiID) REFERENCES AltBayiler(AltBayiID)
)
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
)
GO

INSERT INTO AnaBayi (Isim, Soyisim, KullaniciAdi, Mail, Sifre, Durum, Silinmis)
VALUES ('Furkan', 'Kocaoðlu', 'Admin', 'furkan.kocaoglu@gmail.com', '1234', 1, 0)
GO