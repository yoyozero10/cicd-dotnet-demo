const money = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 2 });
async function loadVersion() {
  try {
    const response = await fetch('/api/version', { cache: 'no-store' });
    if (!response.ok) throw new Error('Không tải được phiên bản');
    const info = await response.json();
    document.querySelector('#version').textContent = `Phiên bản ${info.version}`;
    document.querySelector('#commit').textContent = `Commit ${info.commit.slice(0, 7)} · Build ${info.build}`;
  } catch {
    document.querySelector('#version').textContent = 'Chưa tải được thông tin phiên bản';
  }
}
document.querySelector('#discount-form').addEventListener('submit', async (event) => {
  event.preventDefault();
  const button = event.currentTarget.querySelector('button');
  const result = document.querySelector('.result');
  button.disabled = true;
  result.classList.remove('error');
  try {
    const response = await fetch('/api/discount', {
      method: 'POST', headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ price: Number(document.querySelector('#price').value), percent: Number(document.querySelector('#percent').value) })
    });
    const data = await response.json();
    if (!response.ok) throw new Error(data.error || 'Dữ liệu không hợp lệ.');
    document.querySelector('#total').textContent = money.format(data.total);
    document.querySelector('#message').textContent = `Giảm ${data.percent}% từ ${money.format(data.originalPrice)}. Kết quả do API .NET tính.`;
  } catch (error) {
    result.classList.add('error');
    document.querySelector('#total').textContent = 'Không thể tính';
    document.querySelector('#message').textContent = error.message;
  } finally { button.disabled = false; }
});
loadVersion();
