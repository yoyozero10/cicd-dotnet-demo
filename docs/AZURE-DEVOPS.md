# Azure Pipelines đối chiếu

Chưa có organization Azure DevOps: file YAML được chuẩn bị để cấu hình sau; không được trình bày như pipeline đã chạy thành công.

1. Tạo organization và project tại https://dev.azure.com/ bằng tài khoản của bạn.
2. Kiểm tra agent/parallel jobs được cấp trong Organization settings → Pipelines → Parallel jobs. Nếu dùng agent máy cá nhân, cài .NET SDK 10 và đăng ký agent; thay pool vmImage bằng tên pool thực tế.
3. Pipelines → New pipeline → GitHub → chọn yoyozero10/cicd-dotnet-demo.
4. Chọn Existing Azure Pipelines YAML file → main → /azure-pipelines.yml.
5. Chạy thủ công, giữ deployToAzure = false. Kiểm tra test results và artifact webapp.
6. Chụp màn hình stage Build xanh để đối chiếu trong seminar.

## Nếu cần demo deploy bằng Azure Pipelines

Tạo environment seminar-reference và service connection Azure Resource Manager tên azure-seminar-connection, ưu tiên workload identity federation. Chỉ cấp quyền cần thiết trên Web App demo. Sau khi kiểm tra dịch vụ kết nối, chạy main với deployToAzure = true.

YAML đang trỏ cùng Web App cicd-demo để tránh tạo thêm tài nguyên. Không chạy deploy Azure Pipelines đồng thời với GitHub Actions: hai hệ thống không dùng chung khóa concurrency. Trong seminar chỉ GitHub Actions tự động deploy; Azure Pipelines mặc định chỉ build/test. Có thể tạo Web App F1 riêng nếu muốn thực hiện cả hai luồng độc lập.

Để lấy hình đối chiếu, chỉ cần chạy Build/Test, không cần service connection. Nếu chưa có agent khả dụng, trình bày YAML thay vì khẳng định đã chạy pipeline.
