# Demo trực quan: website lỗi → CI đỏ → bản sửa

Điểm bắt đầu trên Azure: phiên bản 1.1.0-demo-bug. Đây là bản lỗi có chủ đích được triển khai thủ công để dựng tình huống seminar, không phải bản đã vượt CI. Nhánh main và quy tắc required check giữ nguyên.

Website ban đầu: tiêu đề khung tính bị cắt, nút tính hẹp và lệch. Nhập 100000 và -10 trả 110000. Form cho phép gửi số âm để kiểm tra validation của backend.

PR #3 sửa bố cục nhưng giữ lỗi backend nên 2/15 test thất bại và merge bị chặn. Chạy các lệnh sau tại repo khi trình diễn:

```powershell
./scripts/Set-DemoBug.ps1 -Mode Fix
& '../../work/tooling/dotnet/dotnet.exe' test DiscountDemo.slnx -c Release
git diff
git add src/DiscountDemo/PriceCalculator.cs src/DiscountDemo/build-info.json
git commit -m 'fix: reject negative discounts and release v1.2.0'
git push
```

Chờ CI xanh rồi merge PR #3. Mở Actions theo dõi lần chạy main. Tải lại website (Ctrl+F5): phiên bản 1.2.0, bố cục đúng. Nhập lại -10: API trả HTTP 400 và giao diện hiển thị lỗi; nhập 10: kết quả 90000.

Form không chặn số âm ở trình duyệt trong cả hai bản, để kết quả phản ánh backend. Code fix không sửa test.

Nếu cần khôi phục bản tốt trước khi demo: chạy workflow dotnet-ci-cd thủ công trên main. Đợi kiểm tra deployed commit thành công. Không merge nhánh baseline có lỗi.
