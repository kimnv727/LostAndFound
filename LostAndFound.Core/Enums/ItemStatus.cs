namespace LostAndFound.Core.Enums
{
    public enum ItemStatus
    {
        FOUND,
        ACTIVE,
        RETURNED,
        CLOSED
        /*
         *  
            found: có người lụm đồ & report lại
            active: chờ người claim hoặc báo tìm đc
            returned: đã có claim & trả lại
            closed: ko ai nhận & hết thời gian giữ
         * 
         */
    }
}