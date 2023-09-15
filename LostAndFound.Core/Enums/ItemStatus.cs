namespace LostAndFound.Core.Enums
{
    public enum ItemStatus
    {
        PENDING,
        ACTIVE,
        RETURNED,
        CLOSED,
        REJECTED
        /*
         *  
            pending: item chờ admin duyệt
            active: chờ người claim hoặc báo tìm đc
            returned: đã có claim & trả lại
            closed: ko ai nhận & hết thời gian giữ
            rejected: admin không approve post
         * 
         */
    }
}