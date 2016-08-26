rd /q /s Help Pages
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe Bandwidth.Net.Html.shfbproj
git clone https://github.com/bandwidthcom/csharp-bandwidth.git  Pages
cd Pages
git checkout gh-pages || git checkout -b gh-pages 
git rm -rf .
xcopy ..\Help\* . /y /Q /S
git add .
git commit -m "Updated API docs"
git push origin gh-pages
cd ..
