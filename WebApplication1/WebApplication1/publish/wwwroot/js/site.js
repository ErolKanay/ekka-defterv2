// Modern Tablo İşlevleri
document.addEventListener('DOMContentLoaded', function() {
    // Tablo sıralama
    const tableHeaders = document.querySelectorAll('th[data-sort]');
    tableHeaders.forEach(header => {
        header.style.cursor = 'pointer';
        header.addEventListener('click', function() {
            const column = this.getAttribute('data-sort');
            const table = this.closest('table');
            const tbody = table.querySelector('tbody');
            const rows = Array.from(tbody.querySelectorAll('tr'));
            
            const isAscending = this.classList.contains('sort-asc');
            
            // Tüm sıralama sınıflarını temizle
            tableHeaders.forEach(h => {
                h.classList.remove('sort-asc', 'sort-desc');
                h.innerHTML = h.innerHTML.replace(' ↑', '').replace(' ↓', '');
            });
            
            // Yeni sıralama yönünü ayarla
            if (isAscending) {
                this.classList.add('sort-desc');
                this.innerHTML += ' ↓';
            } else {
                this.classList.add('sort-asc');
                this.innerHTML += ' ↑';
            }
            
            // Satırları sırala
            rows.sort((a, b) => {
                const aVal = a.querySelector(`td[data-sort="${column}"]`)?.textContent || '';
                const bVal = b.querySelector(`td[data-sort="${column}"]`)?.textContent || '';
                
                if (column === 'fiyat') {
                    const aNum = parseFloat(aVal.replace(/[^\d.-]/g, ''));
                    const bNum = parseFloat(bVal.replace(/[^\d.-]/g, ''));
                    return isAscending ? bNum - aNum : aNum - bNum;
                } else if (column === 'tarih') {
                    const aDate = new Date(aVal);
                    const bDate = new Date(bVal);
                    return isAscending ? bDate - aDate : aDate - bDate;
                } else {
                    return isAscending ? 
                        bVal.localeCompare(aVal, 'tr') : 
                        aVal.localeCompare(bVal, 'tr');
                }
            });
            
            // Sıralanmış satırları ekle
            rows.forEach(row => tbody.appendChild(row));
        });
    });
    
    // Arama fonksiyonu
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', function() {
            const searchTerm = this.value.toLowerCase();
            const table = document.querySelector('table');
            const rows = table.querySelectorAll('tbody tr');
            
            rows.forEach(row => {
                const text = row.textContent.toLowerCase();
                row.style.display = text.includes(searchTerm) ? '' : 'none';
            });
        });
    }
    
    // Modal işlevleri
    const editModal = document.getElementById('editModal');
    if (editModal) {
        editModal.addEventListener('show.bs.modal', function(event) {
            const button = event.relatedTarget;
            const urunId = button.getAttribute('data-urun-id');
            const urunAdi = button.getAttribute('data-urun-adi');
            const fiyat = button.getAttribute('data-fiyat');
            const beden = button.getAttribute('data-beden');
            const odemeBilgisi = button.getAttribute('data-odeme-bilgisi');
            const odemeTarihi = button.getAttribute('data-odeme-tarihi');
            
            const modalTitle = editModal.querySelector('.modal-title');
            const modalBody = editModal.querySelector('.modal-body');
            
            modalTitle.textContent = `Düzenle: ${urunAdi}`;
            
            modalBody.innerHTML = `
                <form id="editForm" method="post" action="/Urun/Duzenle/${urunId}">
                    <div class="mb-3">
                        <label for="UrunAdi" class="form-label">Ürün Adı</label>
                        <input type="text" class="form-control" id="UrunAdi" name="UrunAdi" value="${urunAdi}" required>
                    </div>
                    <div class="mb-3">
                        <label for="Fiyati" class="form-label">Fiyatı</label>
                        <input type="number" step="0.01" class="form-control" id="Fiyati" name="Fiyati" value="${fiyat}" required>
                    </div>
                    <div class="mb-3">
                        <label for="Bedeni" class="form-label">Bedeni</label>
                        <input type="text" class="form-control" id="Bedeni" name="Bedeni" value="${beden}" required>
                    </div>
                    <div class="mb-3">
                        <label for="OdemeBilgisi" class="form-label">Ödeme Bilgisi</label>
                        <input type="text" class="form-control" id="OdemeBilgisi" name="OdemeBilgisi" value="${odemeBilgisi}" required>
                    </div>
                    <div class="mb-3">
                        <label for="OdemeTarihi" class="form-label">Ödeme Tarihi</label>
                        <input type="date" class="form-control" id="OdemeTarihi" name="OdemeTarihi" value="${odemeTarihi}" required>
                    </div>
                </form>
            `;
        });
    }
    
    // Silme onayı
    const deleteButtons = document.querySelectorAll('[data-action="delete"]');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            const urunAdi = this.getAttribute('data-urun-adi');
            
            if (confirm(`"${urunAdi}" ürününü silmek istediğinizden emin misiniz?`)) {
                const form = this.closest('form');
                form.submit();
            }
        });
    });
    
    // Animasyonlar
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.classList.add('fade-in');
    });
    
});