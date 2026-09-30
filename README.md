# 🚚 Transp - Enterprise Lojistik & Kargo Takip Yönetim Sistemi

Transp, modern kurumsal lojistik ve kargo takip süreçlerini dijitalleştiren, **ASP.NET Core 9**, **MongoDB** ve **Onion / Clean Architecture (CQRS)** prensipleriyle geliştirilmiş çift panelli (Kullanıcı Arayüzü & Yönetim Paneli) tam kapsamlı bir web platformudur.

<!-- 📷 BANNER: Buraya banner fotoğrafını ekle -->
<!-- <p align="center"><img src="FOTOGRAF_YOLU" width="100%"></p> -->

---

## 📸 Ekran Görüntüleri

### 👤 Kullanıcı Arayüzü
<img width="1786" height="887" alt="Ekran görüntüsü 2026-09-30 164725" src="https://github.com/user-attachments/assets/ff700a03-13a2-4a34-831e-7d1527e4d2f4" />
<img width="1833" height="811" alt="Ekran görüntüsü 2026-09-30 164132" src="https://github.com/user-attachments/assets/7131c123-f0d4-441a-8dad-2e117f61083a" />
<img width="1830" height="870" alt="Ekran görüntüsü 2026-09-30 164124" src="https://github.com/user-attachments/assets/3b5aba70-8986-4f65-a342-7150be2c95c7" />
<img width="1845" height="871" alt="Ekran görüntüsü 2026-09-30 164100" src="https://github.com/user-attachments/assets/15918044-3f6d-4bb2-8247-8bce0a7572ae" />
<img width="1851" height="876" alt="Ekran görüntüsü 2026-09-30 164051" src="https://github.com/user-attachments/assets/af90d236-ed89-4a0f-bf97-8d63dc645fb8" />
<img width="1852" height="877" alt="Ekran görüntüsü 2026-09-30 164035" src="https://github.com/user-attachments/assets/c0c92801-7c54-4efe-8d47-49fd7461b7e5" />

---

### 🛡️ Yönetici Paneli
<img width="1827" height="872" alt="image" src="https://github.com/user-attachments/assets/5fb3bed3-ef51-42c7-8f82-dd83d00bfe2b" />
<img width="1826" height="880" alt="Ekran görüntüsü 2026-09-30 164330" src="https://github.com/user-attachments/assets/d7be9111-f448-4a92-9463-304d35eb8eaf" />
<img width="1823" height="872" alt="image" src="https://github.com/user-attachments/assets/273240d4-d861-4bda-975e-b581fedaa10e" />
<img width="1816" height="857" alt="image" src="https://github.com/user-attachments/assets/06aec5fe-2fee-4281-8d5f-a2d2434ea85f" />
<img width="1815" height="872" alt="image" src="https://github.com/user-attachments/assets/2dde661c-e317-4b95-ad33-c48303681202" />


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
