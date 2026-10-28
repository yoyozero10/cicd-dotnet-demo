# Discount Lab — CI/CD cho .NET

Ứng dụng seminar 15–20 phút: PR có bug, CI chặn merge, sửa code, merge và tự động deploy lên Azure App Service Linux F1.

## Liên kết

- Repository: https://github.com/yoyozero10/cicd-dotnet-demo
- Website: https://cicd-demo-c8gdcbfegbb9dga4.westus3-01.azurewebsites.net
- GitHub Actions: https://github.com/yoyozero10/cicd-dotnet-demo/actions

## Chạy local

Cần .NET SDK 10. Nếu dotnet có trong PATH:

```powershell
dotnet restore DiscountDemo.slnx --locked-mode
dotnet test DiscountDemo.slnx -c Release
dotnet run --project src/DiscountDemo --urls http://localhost:5080
```

Mở http://localhost:5080. Môi trường Codex dùng SDK riêng tại `../../work/tooling/dotnet/dotnet.exe`; chạy `./scripts/Start-Local.ps1` để tự chọn SDK này. Trên máy khác cài .NET SDK 10 hoặc dùng setup-dotnet trong CI.

## API

| Endpoint | Chức năng |
| --- | --- |
| GET / | Giao diện tính giảm giá |
| POST /api/discount | JSON `{"price":100,"percent":10}`, trả total = 90 |
| GET /api/version | Phiên bản, commit, số build |
| GET /health | Trạng thái ứng dụng |

Đầu vào âm hoặc phần trăm trên 100 trả HTTP 400. Làm tròn tiền đến 2 chữ số thập phân.

## Cấu trúc

- `src/DiscountDemo`: ASP.NET Core và giao diện HTML/CSS/JS.
- `tests/DiscountDemo.Tests`: 9 unit test và 6 trường hợp kiểm tra API.
- `.github/workflows/dotnet.yml`: CI cho PR, CI/CD trên main, xác thực Azure bằng OIDC.
- `azure-pipelines.yml`: pipeline đối chiếu; mặc định không deploy.
- `scripts/Set-DemoBug.ps1`: đưa bug vào hoặc sửa bug để diễn tập.
- `docs/SEMINAR.md`: lời dẫn và thao tác seminar.
- `docs/AZURE-DEVOPS.md`: cách thiết lập phần Azure Pipelines khi đã có organization.

## Mô hình pipeline

PR vào main chạy job `build-test`. Test thất bại chặn merge khi required status check đã được bật.
Push main chạy `build-test`, tạo webapp.zip rồi job `deploy-azure` tải artifact và deploy. Job deploy không checkout/build lại code. Website phải trả đúng commit mới ở `/api/version` thì kiểm tra sau deploy mới xanh.

Environment GitHub: `production`, chỉ cho nhánh main triển khai và không có required reviewer cho kịch bản tự động deploy.
Repo này thực tế phát OIDC subject có ID bất biến: `repo:yoyozero10@143635344/cicd-dotnet-demo@1424172591:environment:production`.
Federated credential Azure phải khớp chính xác subject, issuer và audience trong log Azure login. Repo khác có thể có subject khác; không sao chép cứng giá trị này khi fork.
GitHub secrets cần có: `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`. Không dùng client secret hoặc publish profile.

## Diễn tập bug

```powershell
git switch -c demo/negative-discount-v1.2
./scripts/Set-DemoBug.ps1 -Mode Introduce
dotnet test DiscountDemo.slnx -c Release
# Mong đợi: 2 test lỗi (unit test và API test cho phần trăm âm).
git add src/DiscountDemo/PriceCalculator.cs
git commit -m "demo: introduce missing negative discount validation"
git push -u origin demo/negative-discount-v1.2
# Mở PR vào main, xem check đỏ.
./scripts/Set-DemoBug.ps1 -Mode Fix
dotnet test DiscountDemo.slnx -c Release
git add src/DiscountDemo/PriceCalculator.cs src/DiscountDemo/build-info.json
git commit -m "fix: reject negative discounts and release v1.2.0"
git push
# Chờ check xanh, merge PR, theo dõi deploy, tải lại website.
```

Không push nhánh demo đang đỏ vào main. Không sửa test để che bug.

## Nguồn chính thức

- https://learn.microsoft.com/azure/app-service/deploy-github-actions
- https://docs.github.com/en/actions/tutorials/build-and-test-code/net
- https://learn.microsoft.com/azure/devops/pipelines/get-started/pipelines-get-started

