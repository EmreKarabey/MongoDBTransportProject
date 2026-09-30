# 🚚 Transp - Enterprise Lojistik & Kargo Takip Yönetim Sistemi

Transp, modern kurumsal lojistik ve kargo takip süreçlerini dijitalleştiren, **ASP.NET Core 9**, **MongoDB** ve **Onion / Clean Architecture (CQRS)** prensipleriyle geliştirilmiş çift panelli (Kullanıcı Arayüzü & Yönetim Paneli) tam kapsamlı bir web platformudur.

---

## 🌟 Öne Çıkan Özellikler

### 👤 1. Müşteri & Kullanıcı Arayüzü (`MongoDBProject`)
- **📦 Gelişmiş Kargo Takibi (`Shipment`):**
  - Kargo takip numarası veya kullanıcı hesabı üzerinden gerçek zamanlı gönderi takibi.
  - Gönderi adımları görselleştirme (Hazırlanıyor, Kargoya Verildi, Dağıtımda, Teslim Edildi).
  - Canlı durum filtreleme ve rota görünümü.
- **🖼️ Profil Yönetimi (`Profile`):**
  - Cloudinary entegrasyonu ile anlık profil fotoğrafı yükleme ve önizleme.
  - Kişisel bilgileri (Ad Soyad, Kullanıcı Adı, E-posta, Telefon) güncelleme.
  - Güvenli şifre değiştirme ve anında oturum çerezi yenileme (`RefreshSignInAsync`).
  - Gönderi istatistikleri ve kargolara hızlı erişim.
- **🔐 Kimlik Doğrulama & Güvenlik (`Account`):**
  - Google OAuth ile Tek Tıkla Giriş (`Google Identity Services`).
  - ASP.NET Core Identity & MongoDB Store entegrasyonu.
  - 6 haneli doğrulama kodu ve süreli token ile **Şifre Sıfırlama** (SMTP E-posta entegrasyonu).
  - Brute-force koruması ve geçici hesap kilitleme (Account Lockout).
- **🤖 Yapay Zeka Entegrasyonları:**
  - **ToxicBert:** Kullanıcı yorumlarında toksik/zararlı içerik tespiti.
  - **Helsinki-NLP:** Çok dilli metin ve yorum çevirisi.
- **🌐 Dinamik Ön Yüz Bileşenleri:**
  - Hizmetlerimiz, Hakkımızda, Projelerimiz, Müşteri Yorumları, Sıkça Sorulan Sorular (SSS), İletişim & Harita.

### 🛡️ 2. Yönetici & Moderatör Paneli (`MongoDBAdmin`)
- **📊 Canlı Yönetim Paneli:**
  - SignalR tabanlı anlık veri akışı ve istatistik sayaçları.
  - Silva Admin modern dashboard teması.
- **🚚 Gönderi (Kargo) Yönetimi:**
  - Yeni kargo oluşturma, durum güncelleme ve hareket geçmişi (tracking event) ekleme.
- **👥 Kullanıcı & Rol Yetkilendirme:**
  - Üye listeleme, Admin / Moderatör rolleri atama ve yetki yönetimi.
- **📝 Dinamik İçerik Yönetimi (CRUD):**
  - Slider, Markalar (Brands), Teklifler (Offers), SSS (FAQ), Neler Yaptık (WhatWeHaveDone), Nasıl Çalışır (HowItWorks), Hakkımızda ve İletişim mesajları.

---

## 🏗️ Mimari & Katmanlı Yapı (Onion Architecture)

```plaintext
DatabaseProject/
├── MongoDB/                            # Müşteri Odaklı Web Uygulaması & Onion Core
│   ├── Domain/                         # Çekirdek Varlıklar (Entities: AppUser, Shipment vb.)
│   ├── Application/                    # CQRS (MediatR), DTO'lar, Repository Arayüzleri, Servisler
│   ├── Persistence/                    # MongoDB Bağlantıları, Generic Repository, Paginate
│   ├── Infrastructure/                 # Cloudinary, Email (SMTP), Google OAuth, AI Servisleri
│   └── MongoDBProject/                 # ASP.NET Core 9 Web UI (Presentation Layer)
│
├── MongoDBAdmin/                       # Yönetim & Admin Paneli
│   ├── EntityLayer/                    # Admin Varlıkları
│   ├── DataAccessLayer/                # MongoDB DAL Uygulamaları
│   ├── BusinessLayer/                  # İş Mantığı (Managers & Services)
│   └── MongoDBAdmin/                   # ASP.NET Core 9 Admin UI
```

---

## 🛠️ Kullanılan Teknolojiler

| Kategori | Teknoloji / Kütüphane |
|---|---|
| **Çatı (Framework)** | .NET 9 (C# 13), ASP.NET Core MVC |
| **Veritabanı** | MongoDB (NoSQL) |
| **Mimari Desenler** | Onion Architecture, CQRS, Repository Pattern, Dependency Injection |
| **Kimlik & Yetkilendirme**| ASP.NET Core Identity, `AspNetCore.Identity.MongoDbCore` |
| **MediatR** | CQRS komut ve sorgu yönetimi |
| **Gerçek Zamanlı** | ASP.NET Core SignalR |
| **Medya Yönetimi** | Cloudinary API (`CloudinaryDotNet`) |
| **E-Posta** | MailKit / SmtpClient (HTML Şablonlu Doğrulama Kodları) |
| **Yapay Zeka (AI)** | HuggingFace Inference API (ToxicBert, Helsinki-NLP) |
| **Ön Yüz (Frontend)** | HTML5, CSS3, JavaScript, Bootstrap 5, SweetAlert, Bootstrap Icons |

---

## 🚀 Kurulum ve Çalıştırma

### 1. Gereksinimler
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MongoDB Community Server](https://www.mongodb.com/try/download/community) (veya MongoDB Atlas bağlantısı)

### 2. Yapılandırma (`appsettings.json`)
`MongoDB/MongoDBProject/appsettings.json` ve `MongoDBAdmin/MongoDBAdmin/appsettings.json` dosyalarındaki ilgili alanları kendi servis bilgilerinize göre doldurun:

```json
{
  "DatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "DBTransport"
  },
  "CloudinarySettings": {
    "CloudName": "YOUR_CLOUDINARY_CLOUD_NAME",
    "ApiKey": "YOUR_CLOUDINARY_API_KEY",
    "ApiSecret": "YOUR_CLOUDINARY_API_SECRET"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSsl": true,
    "User": "your-email@gmail.com",
    "Pass": "your-app-password"
  },
  "Google": {
    "ClientID": "YOUR_GOOGLE_CLIENT_ID",
    "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
  }
}
```

### 3. Uygulamayı Başlatma

**Kullanıcı Arayüzü:**
```bash
cd MongoDB/MongoDBProject
dotnet run
```

**Admin Paneli:**
```bash
cd MongoDBAdmin/MongoDBAdmin
dotnet run
```

---

## 📄 Lisans
Bu proje açık kaynaklı olup, eğitim ve portföy amaçlı geliştirilmiştir.
