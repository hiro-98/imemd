# IME Mode Display for Google日本語入力
マウスクリック時に全角/半角キーを2回押下することで、カーソル位置にIMEの状態を表示します。

全角/半角キーを2回押下するだけですので、Google日本語入力を使用していない場合はステータス表示が行われません。

# ダウンロード
https://github.com/hiro-98/imemd/releases/tag/v1.0.0


# 実行方法
実行するとシステムトレイに配置され、ダブルクリックで設定画面、右クリックで終了メニューが表示されます。

## 動作例
マウスクリック時に以下のように表示されます。

![実行例](https://user-images.githubusercontent.com/36811209/127651790-b04f5df1-328b-4982-9465-6dd938e700ae.jpg)

## 設定画面
![setting](https://user-images.githubusercontent.com/36811209/127651589-b5392f64-5f24-473d-a0b4-47b241fd1af0.jpg)

"マウスカーソルがIビームの場合のみ表示" をチェックしても Google Chrome を使用しているときは判定を行いません。（新しいタブを開くときはマウスカーソルが矢印になっているため）


# アンインストール
imemd.exe と setting.ini を削除してください。レジストリなどは使用していません。


# 仕組み
グローバルフックで左クリックのイベントを検出し、全角/半角キーを2回押下しています。
Win32APIを使用しています。


# 開発環境など
* Visual Studio 2019
* C#


# その他
* バグがあったらツイッターなどで教えて下さい。 ( https://twitter.com/hiro98v7 )
* MIT License です。
