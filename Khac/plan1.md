# Plan Kiểm Tra Và Chuẩn Hóa CSS Toàn Project

## 1. Mục tiêu

Mục tiêu của kế hoạch này là rà soát và chuẩn hóa toàn bộ CSS/UI của các project trong workspace để:

- không còn lỗi vỡ layout, lệch spacing, sai font, sai màu, chồng lớp, cắt nội dung, overflow ngoài ý muốn
- không còn trạng thái giao diện thiếu đồng nhất giữa các trang
- hoạt động ổn định ở desktop, tablet, mobile
- hiển thị đúng ở các trạng thái tương tác: hover, focus, active, disabled, loading, error, empty state
- đạt cảm giác hoàn thiện như một website/admin chuyên nghiệp

Lưu ý kỹ thuật:

- Không có cách thực tế nào bảo đảm “0 lỗi tuyệt đối” nếu chưa kiểm hết toàn bộ flow, toàn bộ data state và toàn bộ môi trường trình duyệt.
- Mục tiêu đúng là: đưa toàn bộ CSS về trạng thái có checklist rõ ràng, có tiêu chí nghiệm thu đo được, có vòng kiểm thử lại sau sửa.

## 2. Phạm vi project cần kiểm tra

Theo workspace hiện tại, ưu tiên audit theo nhóm sau:

1. `QLKH_LTW`
2. `aznews`
3. `inapp-1.0.0/inapp-1.0.0`
4. `UIAdminDemo` nếu còn dùng làm nguồn tham chiếu giao diện

Nếu một project không còn dùng production thì vẫn audit ở mức:

- không để CSS lỗi lan sang project khác
- không dùng nhầm asset lỗi thời
- không để trùng class/global style gây side effect

## 3. Tiêu chuẩn hoàn thành

Một trang chỉ được coi là đạt khi đáp ứng đủ các tiêu chí sau:

### 3.1. Layout

- không có phần tử bị tràn ngang ở viewport phổ biến
- không có nội dung bị cắt mất chữ, icon, nút, badge, dropdown, tooltip, modal
- các cột, card, table, form, navbar, sidebar căn hàng nhất quán
- không có khoảng trắng thừa hoặc khoảng cách bất thường
- không có section bị lệch tâm hoặc lệch container

### 3.2. Typography

- hệ font thống nhất theo từng project
- font size có thang bậc rõ ràng: heading, subheading, body, helper text, caption
- line-height đủ dễ đọc
- weight dùng nhất quán, không lạm dụng quá nhiều cấp độ đậm nhạt
- text không bị nhảy size giữa các breakpoint nếu không có chủ đích

### 3.3. Màu sắc và độ tương phản

- màu primary, success, warning, danger, muted được dùng nhất quán
- text và background đủ contrast để đọc rõ
- badge, button, alert, trạng thái không bị sai màu ngữ nghĩa
- không tồn tại nhiều tone gần giống nhau nhưng mang cùng một ý nghĩa

### 3.4. Component

- button cùng loại phải có cùng padding, radius, font size, hover/focus style
- input, select, textarea đồng bộ về chiều cao, border, spacing, placeholder
- dropdown, modal, tooltip, popover đúng vị trí và không bị clipping
- table có spacing, alignment, sticky behavior và responsive behavior rõ ràng
- card có shadow, radius, border, hover state hợp lý, không lạm dụng hiệu ứng

### 3.5. Responsive

- kiểm tra tối thiểu các mốc: `320`, `375`, `390`, `414`, `768`, `1024`, `1280`, `1440`
- menu mobile hoạt động ổn định
- bảng lớn có chiến lược responsive rõ ràng: scroll ngang, stack, collapse hoặc priority columns
- form không bị vỡ hai cột hoặc đè nút
- ảnh, banner, hero, card grid co giãn hợp lý

### 3.6. Tương tác

- hover không gây nhảy layout
- focus phải nhìn thấy rõ, đặc biệt với input, button, link
- active/selected state phải phân biệt được
- disabled state phải đúng ngữ nghĩa và vẫn đọc được
- loading state không làm layout giật
- animation không làm sai vị trí tuyệt đối, sai z-index, sai dropdown placement

### 3.7. Khả năng bảo trì

- hạn chế CSS trùng lặp
- tránh selector quá sâu hoặc quá phụ thuộc DOM
- ưu tiên token hóa màu, spacing, radius, shadow
- không dùng `!important` tràn lan
- không để style inline nếu có thể đưa về stylesheet

## 4. Danh sách lỗi CSS bắt buộc phải quét

### 4.1. Lỗi hiển thị trực tiếp

- text bị cắt do `overflow: hidden`
- dropdown/menu/modal bị lệch vị trí
- z-index sai khiến menu nằm dưới card, table, overlay
- ảnh méo tỉ lệ
- button/icon lệch tâm
- table row, badge, chip, tag bị lệch baseline
- scrollbar ngang ngoài ý muốn
- fixed/sticky element che nội dung

### 4.2. Lỗi responsive

- container quá rộng hoặc quá hẹp ở mobile
- font quá nhỏ dưới 14px ở nội dung chính
- tap target quá bé
- sidebar/topbar che nội dung
- popup vượt khỏi viewport
- form bị tràn hoặc mất label

### 4.3. Lỗi consistency

- cùng một loại nút nhưng khác style giữa các trang
- cùng một loại bảng nhưng khác padding/cỡ chữ không có lý do
- cùng trạng thái nhưng khác màu
- cùng card nhưng khác radius/shadow
- spacing không theo một scale chung

### 4.4. Lỗi kỹ thuật CSS

- CSS chết, selector không còn dùng
- class trùng tên giữa nhiều project
- import asset không còn tồn tại
- media query chồng chéo khó đoán
- animation dùng `transform` trên phần tử cha làm hỏng popup con
- style ghi đè framework nhưng không có chủ đích rõ ràng

## 5. Quy trình audit đề xuất

### Giai đoạn 1. Lập bản đồ UI

Mục tiêu:

- liệt kê toàn bộ page template, layout, partial, component, stylesheet chính

Việc cần làm:

1. xác định entry CSS/SCSS của từng project
2. xác định layout chính, component dùng chung, vendor CSS
3. nhóm trang theo loại:
   - auth
   - dashboard
   - listing/table
   - form/create-edit
   - detail
   - modal/dialog
   - public landing/news

Kết quả cần có:

- sơ đồ file CSS chính
- danh sách component dùng chung
- danh sách trang cần test thủ công

### Giai đoạn 2. Kiểm tra nền tảng design system

Mục tiêu:

- thống nhất nền móng giao diện trước khi sửa từng trang

Việc cần làm:

1. chuẩn hóa token:
   - màu
   - spacing
   - radius
   - shadow
   - font scale
2. xác định bộ class nền:
   - button
   - form control
   - badge
   - card
   - table
   - dropdown
   - modal
3. loại bỏ khác biệt vô lý giữa các component giống nhau

Kết quả cần có:

- một bộ quy ước UI nhất quán
- một danh sách override cần giữ lại và lý do

### Giai đoạn 3. Audit từng trang theo checklist

Mỗi trang phải được kiểm theo các nhóm:

1. desktop
2. tablet
3. mobile
4. empty state
5. long content state
6. validation/error state
7. loading state nếu có

Checklist cho mỗi trang:

- header có đúng spacing và hierarchy không
- các action có đủ rõ ràng không
- lưới/grid có đều không
- table có đọc được nhanh không
- form có scan dễ không
- focus state có rõ không
- component nổi như dropdown/modal có đúng vị trí không
- không có layout shift khi hover/click/open menu

### Giai đoạn 4. Sửa theo ưu tiên

Thứ tự ưu tiên sửa:

1. lỗi chặn thao tác người dùng
2. lỗi responsive/mobile
3. lỗi consistency toàn hệ thống
4. lỗi thẩm mỹ nhỏ
5. tối ưu dọn CSS thừa

Nguyên tắc sửa:

- sửa ở component gốc trước, tránh vá từng trang
- tránh fix cục bộ tạo side effect
- mỗi lần sửa phải retest lại các trang dùng chung component đó

### Giai đoạn 5. Regression test giao diện

Sau mỗi cụm sửa phải kiểm tra lại:

- trang nguồn bị lỗi
- các trang dùng chung layout
- các breakpoint chính
- trạng thái mở dropdown/modal/tooltip
- dark text trên nền sáng và ngược lại nếu có

## 6. Bộ tiêu chí cụ thể cho website chuyên nghiệp

Một giao diện được xem là đạt mức chuyên nghiệp khi:

- nhìn vào là thấy có hệ thống, không chắp vá
- typography có phân cấp rõ và dễ đọc
- màu sắc tiết chế, có chủ đích, không loạn
- spacing đều, rhythm tốt
- trạng thái tương tác rõ nhưng không phô trương
- animation nhanh, có lý do, không phá usability
- nội dung quan trọng luôn nổi bật hơn nội dung phụ
- thao tác quan trọng luôn nằm đúng nơi người dùng kỳ vọng
- responsive không chỉ “co lại được” mà vẫn giữ được trải nghiệm

## 7. Tool và cách kiểm tra

### 7.1. Kiểm tra thủ công

- mở từng trang ở các viewport chuẩn
- test bằng chuột và keyboard
- mở dropdown, modal, tooltip, datepicker, offcanvas
- nhập dữ liệu dài/ngắn bất thường
- thử text tiếng Việt có dấu, text dài, số lớn, badge dài

### 7.2. Kiểm tra bằng DevTools

- dùng inspect để xem overflow
- kiểm tra computed style khi component lỗi
- bật device toolbar để test responsive
- kiểm tra stacking context và z-index
- xem element nào tạo `transform`, `overflow`, `position`, `contain`

### 7.3. Kiểm tra tự động nên có

- screenshot compare cho page chính
- smoke test mở các dropdown/modal chính
- lint CSS hoặc stylelint nếu project có điều kiện tích hợp
- kiểm tra asset missing và CSS unused ở mức hỗ trợ

## 8. Ma trận kiểm thử tối thiểu

Mỗi page quan trọng cần được test trên:

1. Chrome desktop
2. Edge desktop
3. Mobile width giả lập trong DevTools

Mỗi page cần test ở các trạng thái:

1. mặc định
2. hover
3. focus
4. active
5. disabled
6. error
7. loading
8. dữ liệu dài
9. dữ liệu rỗng

## 9. Definition of Done cho từng hạng mục CSS

Một hạng mục chỉ được đóng khi:

1. lỗi đã được sửa ở đúng lớp component hoặc layout
2. không phát sinh regression ở trang liên quan
3. desktop và mobile đều ổn
4. không thêm CSS vá tạm thiếu kiểm soát
5. đã tự test lại các trạng thái tương tác
6. không còn lỗi nhìn thấy được bằng mắt thường trong flow chính

## 10. Kế hoạch triển khai thực tế

### Đợt 1. Audit nền tảng

- kiểm tra file CSS/SCSS chính của từng project
- xác định token, component, layout chung
- đánh dấu vùng CSS có rủi ro cao

### Đợt 2. Sửa component gốc

- button
- form
- table
- dropdown
- modal
- card
- navigation

### Đợt 3. Sửa từng nhóm trang

- admin dashboard và listing
- create/edit form
- detail page
- public/news page

### Đợt 4. Responsive + regression

- test toàn bộ breakpoint chính
- test lại các flow có popup, filter, table, menu

### Đợt 5. Dọn và khóa chuẩn

- dọn CSS thừa
- gom token
- ghi quy ước để các lần sửa sau không phá chuẩn

## 11. Danh sách deliverable cuối cùng cần có

1. bảng thống kê page đã audit
2. danh sách bug UI/CSS theo mức độ
3. danh sách component đã chuẩn hóa
4. danh sách regression đã retest
5. quy ước CSS/UI dùng chung cho các lần phát triển tiếp theo

## 12. Kết luận thực thi

Nếu muốn kết quả thật sự sạch và chuyên nghiệp, không nên sửa từng lỗi lẻ theo cảm tính. Cần đi theo đúng thứ tự:

1. map hệ thống
2. chuẩn hóa token và component
3. audit từng trang
4. sửa theo ưu tiên
5. regression đầy đủ

Kế hoạch này là baseline để kiểm tra lại toàn bộ CSS của workspace theo tiêu chuẩn production, giảm lỗi hiển thị, tăng tính đồng nhất và đưa giao diện gần mức website chuyên nghiệp.
