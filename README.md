# 電商網站API系統

## 環境安裝

1. 先到微軟官網(https://dotnet.microsoft.com/download)，下載 .Net 5.0的SDK

![下載.net5圖片](https://i.imgur.com/gYlBtqr.png)

2. 下載SQLServer(https://www.microsoft.com/zh-tw/sql-server/sql-server-downloads ，可選Express版或開發版)

![下載SQLServer](https://i.imgur.com/IQ29B3d.png)

## 下載專案及設定

1. 進入想要存放的資料夾位置

```bash
cd yourPath   //yourPath改成你的資料夾路徑
```

2. 從Github將專案Clone到本機端

```bash
 git clone https://github.com/goodness090807/EcommerceWebApi.git
```
3. 在跟appsettings.json同目錄底下，新增 appsettings.Development.json 檔案 (開發專用，如果不是開發直接使用appsettings.json就可以了)

![新增設定檔](https://i.imgur.com/Zc4OdYB.png)

4. 打開appsettings.json和appsettings.Development.json，並將appsettings.json的資料複製到appsettings.Development.json

![複製appsettings.json資訊](https://i.imgur.com/iaruGCt.png)

5. 在DefaultConnection設定資訊，Server放入資料庫(SqlServer)的伺服器名稱，Database可以隨意取自己想要的
6. 在JwtSettings設定Issuer和Tokenkey
    
    Issuer為發行者，可設定自己的網址或是名字。

    Tokenkey為用來做hash的Key，可隨意設置。

## 啟動專案

1. 開啟命令提示字元，並將執行位置設定在與Controller相同位置

```bash
cd yourPath\EcommerceWebApi\EcommerceWebApi
```
2. 執行專案

```bash
dotnet run
```

## 完成並顯示畫面

1. 預設API文件位置

    https://localhost:5001/swagger

2. 成功建置的畫面


![複製appsettings.json資訊](https://i.imgur.com/8iSh3LK.png)
