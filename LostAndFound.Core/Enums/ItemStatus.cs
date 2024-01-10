namespace LostAndFound.Core.Enums
{
    public enum ItemStatus
    {
        PENDING,
        ACTIVE,
        RETURNED,
        CLOSED,
        REJECTED,
        DELETED,
        EXPIRED, 
        GAVEAWAY,
        ONHOLD
        /*
             pending: item chờ admin duyệt
             active: chờ người claim hoặc báo tìm đc
             returned: đã có claim & trả lại
             closed: người dùng tự đóng
             rejected: admin không approve item
             deleted: manager nuke the item,
             expired: hết thời gian giữ
        */
    }
}