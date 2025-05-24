import pyodbc # type: ignore
from flask import Flask, request, abort, session
from linebot import LineBotApi, WebhookHandler
from linebot.exceptions import InvalidSignatureError
from linebot.models import MessageEvent, TextMessage, TextSendMessage, QuickReply, QuickReplyButton, MessageAction
 
 
#======python的函數庫==========

app = Flask(__name__)
LINE_CHANNEL_ACCESS_TOKEN = 'OHB5l/qTe3jVYGFwdEZsePWxJ6sbkmZIIKqOcnNCtMOPtiwslXIvjUq9pin7ai/GUZJUT6P+jNg7RffUQFEpw3ceH6keFBQ2GqzbFXH3YKBBAHmzbb9A2tZCn2mQ0T7axkE/aSIpCB/57YL/VG4kqAdB04t89/1O/w1cDnyilFU='
 
LINE_CHANNEL_SECRET = '3b5e7cfddaefe9c7119cb1c87a4851a9'
line_bot_api = LineBotApi(LINE_CHANNEL_ACCESS_TOKEN)
handler = WebhookHandler(LINE_CHANNEL_SECRET)

# 資料庫連接配置
DATABASE_CONFIG = {
    'DRIVER': '{ODBC Driver 18 for SQL Server}',  # 根據安裝的驅動程式版本進行選擇
    'SERVER': '(localdb)\\MSSQLLocalDB',  # 使用本地資料庫實例
    'DATABASE': 'rent',  # 這裡可以是任意名稱，不需要真的存在於伺服器上
    'Trusted_Connection': 'yes',
    'AttachDbFilename': 'C:\\Users\\SEB\\OneDrive\\桌面\\rent.mdf'  # 替換為你的MDF文件路徑
}
user_states = {}
def get_db_connection():
    conn_str = (
        f"DRIVER={DATABASE_CONFIG['DRIVER']};"
        f"SERVER={DATABASE_CONFIG['SERVER']};"
        f"DATABASE={DATABASE_CONFIG['DATABASE']};"
        f"Trusted_Connection={DATABASE_CONFIG['Trusted_Connection']};"
        f"AttachDbFilename={DATABASE_CONFIG['AttachDbFilename']}"
    )
    return pyodbc.connect(conn_str)

def get_member_name(member_id):
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute("SELECT name FROM Member WHERE memberID = ?", member_id)
    row = cursor.fetchone()
    conn.close()
    if row:
        return row.name
    else:
        return "未知會員"

def get_rent_location_name(rent_id):
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute("SELECT rentLocation FROM RentLocation WHERE rentID = ?", rent_id)
    row = cursor.fetchone()
    conn.close()
    if row:
        return row.rentLocation
    else:
        return "未知租賃地點"

def get_return_location_name(return_id):
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute("SELECT returnLocation FROM ReturnLocation WHERE returnID = ?", return_id)
    row = cursor.fetchone()
    conn.close()
    if row:
        return row.returnLocation
    else:
        return "未知歸還地點"

def get_order_info(order_id):
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute("""
        SELECT orderID, memberID, rentID, returnID, orderDate, returnDate, carID, emailSent, payStat, rentC
        FROM [Order]
        WHERE orderID = ?
    """, order_id)
    row = cursor.fetchone()
    conn.close()
    if row:
        member_name = get_member_name(row.memberID)
        rent_location_name = get_rent_location_name(row.rentID)
        return_location_name = get_return_location_name(row.returnID)
        email_sent = "已發送" if row.emailSent else "未發送"
        pay_stat = "已付款" if row.payStat else "未付款"
        return {
            "orderID": row.orderID,
            "memberName": member_name,
            "rentLocationName": rent_location_name,
            "returnLocationName": return_location_name,
            "orderDate": row.orderDate,
            "returnDate": row.returnDate,
            "carID": row.carID,
            "emailSent": email_sent,
            "payStat": pay_stat,
            "rentC": row.rentC
        }
    else:
        return None

@app.route("/callback", methods=['POST'])
def callback():
    signature = request.headers['X-Line-Signature']
    body = request.get_data(as_text=True)
    app.logger.info("Request body: " + body)
    try:
        handler.handle(body, signature)
    except InvalidSignatureError:
        abort(400)
    return 'OK'

@handler.add(MessageEvent, message=TextMessage)
def handle_text_message(event):
    user_id = event.source.user_id
    msg = str(event.message.text)
    
    if msg == "選單":
        # 保存狀態，等待使用者輸入訂單編號或姓名
        user_states[user_id] = '等待輸入'
        line_bot_api.reply_message(
            event.reply_token,
            TextSendMessage(text="請輸入訂單編號或姓名")
        )
    elif user_states.get(user_id) == '等待輸入':
        # 使用者已輸入訂單編號或姓名，保存到狀態
        user_states[user_id] = msg
        if len(msg) > 4:
            quick_reply_buttons = QuickReply(items=[
                QuickReplyButton(action=MessageAction(label="查詢訂單", text=f"查詢訂單 {msg}"))
            ])
            line_bot_api.reply_message(
                event.reply_token,
                TextSendMessage(text="請選擇以下操作", quick_reply=quick_reply_buttons)
            )
    elif msg.startswith("查詢訂單"):
        order_id = msg.split(" ")[1]  # 假設命令格式為 "查詢訂單 訂單編號"
        order_info = get_order_info(order_id)
        if order_info:
            response = (
                f"訂單編號: {order_info['orderID']}\n"
                f"會員姓名: {order_info['memberName']}\n"
                f"租賃地點: {order_info['rentLocationName']}\n"
                f"歸還地點: {order_info['returnLocationName']}\n"
                f"訂單日期: {order_info['orderDate']}\n"
                f"歸還日期: {order_info['returnDate']}\n"
                f"車輛編號: {order_info['carID']}\n"
                f"郵件發送: {order_info['emailSent']}\n"
                f"支付狀態: {order_info['payStat']}\n"
                f"租賃成本: {order_info['rentC']}\n"
            )
        else:
            response = "找不到該訂單資訊。"
        line_bot_api.reply_message(event.reply_token, TextSendMessage(response))
    # 如果命令不是查詢訂單則不回應
        
        
import os
if __name__ == "__main__":
    port = int(os.environ.get('PORT', 5000))
    app.run(host='0.0.0.0', port=port,debug=True)