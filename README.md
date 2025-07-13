# 🌸 Çiçek Dükkanı - Flower Shop

Modern ve kullanıcı dostu bir çiçek satış platformu. Vue.js frontend ve .NET Core backend ile geliştirilmiştir.

## 🚀 Özellikler

### 👤 Kullanıcı Özellikleri
- **Çiçek Kataloğu**: Kategorilere göre çiçek görüntüleme
- **Özel Günler**: Doğum günü, düğün, yıldönümü gibi özel günlere özel çiçekler
- **Sepet Sistemi**: Ürünleri sepete ekleme ve yönetme
- **Sipariş Verme**: Teslimat bilgileri ile sipariş oluşturma
- **Sipariş Takibi**: Sipariş numarası ve e-posta ile sipariş durumu sorgulama
- **Hesap Sistemi**: Kullanıcı girişi ve sipariş geçmişi
- **Koyu/Açık Tema**: Tema değiştirme özelliği
- **Mobil Uyumlu**: Responsive tasarım

### 🔧 Admin Özellikleri
- **Çiçek Yönetimi**: Çiçek ekleme, düzenleme, silme
- **Kategori Yönetimi**: Kategori ve özel gün yönetimi
- **Sipariş Yönetimi**: Sipariş durumu güncelleme
- **Dashboard**: İstatistiksel veriler

## 🛠️ Teknolojiler

### Frontend
- **Vue.js 3** - Modern JavaScript framework
- **Vue Router** - Sayfa yönlendirme
- **Bootstrap 5** - UI framework
- **Vite** - Build tool

### Backend
- **.NET 8** - Web API
- **Entity Framework Core** - ORM
- **SQLite** - Veritabanı
- **JWT** - Authentication (gelecek özellik)

## 📋 Gereksinimler

- **Node.js** (v18 veya üzeri)
- **npm** veya **yarn**
- **.NET 8 SDK**
- **Git**

## 🚀 Kurulum ve Çalıştırma

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/ilkerAdanur/VueProjeCicekRepo.git
cd VueProjeCicekRepo
```

### 2. Backend Kurulumu
```bash
# Backend klasörüne gidin
cd FlowerShop.Backend/FlowerShop.API

# Paketleri yükleyin (otomatik)
dotnet restore

# Veritabanını oluşturun
dotnet ef database update

# Backend'i çalıştırın
dotnet run
```
Backend şu adreste çalışacak: `http://localhost:5133`

### 3. Frontend Kurulumu
```bash
# Yeni terminal açın ve frontend klasörüne gidin
cd FlowerShop.Frontend/flower-shop-frontend

# Paketleri yükleyin
npm install

# Frontend'i çalıştırın
npm run dev
```
Frontend şu adreslerde çalışacak:
- **Yerel**: `http://localhost:5173`
- **Network**: `http://[IP_ADRESINIZ]:5173` (mobil erişim için)

## 📱 Mobil Erişim

Aynı ağdaki mobil cihazlardan erişim için:
1. Bilgisayarınızın IP adresini öğrenin (`ipconfig` komutu ile)
2. Mobil cihazınızdan `http://[IP_ADRESI]:5173` adresine gidin

## 👥 Test Hesapları

### Admin Hesabı
- **Kullanıcı Adı**: `admin`
- **Şifre**: `admin123`

### Demo Hesabı
- **E-posta**: `demo@demo`
- **Şifre**: `demo123`

## 🗂️ Proje Yapısı

```
FlowerShop/
├── FlowerShop.Backend/
│   └── FlowerShop.API/
│       ├── Controllers/     # API kontrolcüleri
│       ├── Models/         # Veri modelleri
│       ├── Data/           # Veritabanı context
│       ├── Migrations/     # EF migrations
│       └── DTOs/           # Data transfer objects
└── FlowerShop.Frontend/
    └── flower-shop-frontend/
        ├── src/
        │   ├── components/ # Vue bileşenleri
        │   ├── views/      # Sayfa bileşenleri
        │   ├── router/     # Yönlendirme
        │   └── assets/     # Statik dosyalar
        └── public/         # Public dosyalar
```

## 🔧 Geliştirme

### Backend Geliştirme
```bash
cd FlowerShop.Backend/FlowerShop.API

# Yeni migration oluşturma
dotnet ef migrations add MigrationName

# Veritabanını güncelleme
dotnet ef database update

# Build
dotnet build
```

### Frontend Geliştirme
```bash
cd FlowerShop.Frontend/flower-shop-frontend

# Development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

## 📊 API Endpoints

### Çiçekler
- `GET /api/flowers` - Tüm çiçekleri listele
- `GET /api/flowers/{id}` - Çiçek detayı
- `POST /api/flowers` - Yeni çiçek ekle (Admin)
- `PUT /api/flowers/{id}` - Çiçek güncelle (Admin)
- `DELETE /api/flowers/{id}` - Çiçek sil (Admin)

### Siparişler
- `POST /api/orders` - Yeni sipariş oluştur
- `GET /api/orders/search` - Sipariş ara
- `GET /api/orders/customer/{email}` - Müşteri siparişleri

### Kategoriler
- `GET /api/categories` - Kategorileri listele
- `GET /api/occasions` - Özel günleri listele

### Kimlik Doğrulama
- `POST /api/auth/login` - Giriş yap

## 🎨 Tema Sistemi

Uygulama koyu ve açık tema desteği sunar:
- Header'daki ay/güneş ikonuna tıklayarak tema değiştirilebilir
- Tema tercihi localStorage'da saklanır
- Sayfa yenilendiğinde tema korunur

## 🛡️ Güvenlik

- Şifreler SHA256 ile hash'lenir
- Admin paneli giriş kontrolü
- SQL injection koruması (Entity Framework)
- XSS koruması

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit yapın (`git commit -m 'Add some AmazingFeature'`)
4. Push yapın (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 📞 İletişim

Proje Sahibi: İlker Adanur
- GitHub: [@ilkerAdanur](https://github.com/ilkerAdanur)

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!
