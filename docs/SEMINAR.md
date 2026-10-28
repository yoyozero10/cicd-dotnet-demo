# Kịch bản seminar 15–20 phút

## Chuẩn bị trước giờ nói

Mở sẵn website, PR demo đỏ, tab Actions, file workflow và Azure App Service. Mở website trước giờ demo để khởi động ứng dụng F1. Cần mạng Internet; chuẩn bị bản ghi màn hình của một lần diễn tập thành công.

Kiểm tra `main` ở v1.1.0 và website trả đúng commit. PR demo chỉ bỏ kiểm tra phần trăm âm. Mọi test khác giữ nguyên.

## Kịch bản

| Phút | Màn hình và thao tác | Lời dẫn gợi ý |
| --- | --- | --- |
| 0–2 | Website, nhập 100000 và 10 | Đây là bản v1.1.0 trên Azure. Mục tiêu là cập nhật ứng dụng bằng pipeline. |
| 2–4 | PriceCalculatorTests và dotnet.yml | Mỗi PR phải build và test. Artifact là gói publish dùng cho deploy. |
| 4–6 | PR đỏ, log 2 test lỗi, trạng thái merge | Code đang bỏ sót đầu vào âm. Unit test và kiểm thử API phát hiện cùng bug. Quy tắc bảo vệ main yêu cầu check xanh. |
| 6–9 | Chạy script Fix, xem diff, commit và push | Sửa code kiểm tra đầu vào, không thay kỳ vọng của test. Tăng phiên bản lên 1.2.0. |
| 9–14 | Check xanh, merge PR, tab Actions | Merge tạo lần chạy trên main. Job deploy lấy artifact của job build và đăng nhập bằng token OIDC ngắn hạn. |
| 14–16 | Website, tải lại trang, /api/version | Nhãn v1.2.0 và mã commit xác nhận đúng bản vừa triển khai. |
| 16–18 | Azure Pipeline đã chạy hoặc YAML nếu chưa thiết lập | Cùng lệnh .NET nhưng Azure Pipelines tổ chức thành stage, job, task. Chỉ trình bày kết quả chạy thật nếu đã có. |
| 18–20 | Tổng kết, câu hỏi | CI phát hiện bug trước merge. CD tự đưa bản đạt yêu cầu lên Azure. |

## Lệnh khi demo

```powershell
git switch demo/negative-discount-v1.2
./scripts/Set-DemoBug.ps1 -Mode Fix
git diff
git add src/DiscountDemo/PriceCalculator.cs src/DiscountDemo/build-info.json
git commit -m "fix: reject negative discounts and release v1.2.0"
git push
```

Nếu đã sửa PR trong lần diễn tập trước, tạo nhánh mới từ main có phiên bản baseline mới; không force-push lên main hoặc tự xóa lịch sử.

## Dự phòng

Nếu đến phút 14 chưa deploy xong, mở lần chạy đã lưu hoặc video và nói rõ là bản diễn tập. Không hứa số phút cố định trước khi đo. Tắt tiếng thông báo cá nhân và phóng to trình duyệt để khán giả đọc log.

## Cập nhật slide

- Slide 17: environment production không có bước duyệt trong kịch bản này.
- Slide 18: chụp PR bị chặn và run thành công thực tế.
- Slide 27: dùng tên project DiscountDemo; GitHub Actions, OIDC, App Service Linux F1.
- Slide 28: lấy thời gian thật từ Actions; phân biệt thời gian job build với từng step deploy.
- Slide 22/23: slot và Docker chỉ giải thích, không nằm trong demo trực tiếp.

